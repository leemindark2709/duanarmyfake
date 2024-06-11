using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float requiredDistance = 2f;
    public int playerCount = 2; // Số lượng Player muốn tạo
    public List<Transform> points;
    public List<Transform> pointsTable;
    public List<Transform> players;
    public Transform ActivePlayer; // Biến để theo dõi Player đang hoạt động

    private Queue<Transform> queuePlayer = new Queue<Transform>(); 
    private Queue<Transform> queueTable = new Queue<Transform>();

    public List<Transform> teamEven = new List<Transform>();
    public List<Transform> teamOdd = new List<Transform>();
    private Coroutine switchCoroutine;
    private float defaultSwitchDelay = 10f;
    private float switchDelay = 10f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        players = new List<Transform>();
       
        PlayerManager.Instance.LoadPlayer();
        PlayerAppear.Instance.LoadCheckPoint();
        PlayerAppear.Instance.GetPoints(playerCount, requiredDistance, out points);
        PlayerTableManager.Instance.LoadPlayer();
        PlayerTableAppear.Instance.LoadCheckPoint();
        PlayerTableAppear.Instance.GetPoints(playerCount, requiredDistance, out pointsTable);
        //Tạo và thêm các Player vào hàng đợi
        for (int i = 0; i < playerCount; i++)
        {
            Transform player = SpawnPlayer("Player", points[i]);
            Transform playerTable = SpawnPlayerTable("PlayerTable", pointsTable[i]);
            player.GetComponent<DamageReceiver>().playertable = playerTable;
 
            if (player != null)
            {
                turnOffComponent(player);
                AddPlayerToQueue(player);
                players.Add(player);

                // Thêm player vào team chẵn hoặc lẻ
                if (i % 2 == 0)
                {
                    teamEven.Add(player);
                }
                else
                {
                    teamOdd.Add(player);
                }
            }
        }

        // Đặt Player đầu tiên trong hàng đợi làm ActivePlayer ban đầu
        if (queuePlayer.Count > 0)
        {
            ActivePlayer = queuePlayer.Peek();
            turnOnComponent(ActivePlayer);
        }

        // Bắt đầu chu kỳ chuyển lượt
        switchCoroutine = StartCoroutine(SwitchActivePlayerCycle());
    }

    // Hàm để chuyển đổi ActivePlayer và bật/tắt các component
    void SwitchActivePlayer()
    {
        // Nếu hàng đợi rỗng, không làm gì cả
        if (queuePlayer.Count == 0)
            return;

        // Lấy và loại bỏ player hiện tại khỏi đầu hàng đợi
        Transform previousPlayer = queuePlayer.Dequeue();

        // Tắt component của player hiện tại
        turnOffComponent(previousPlayer);

        // Đưa player hiện tại vào cuối hàng đợi
        queuePlayer.Enqueue(previousPlayer);

        // Lấy player mới từ đầu hàng đợi
        ActivePlayer = queuePlayer.Peek();

        // Bật component của player mới
        turnOnComponent(ActivePlayer);

        // Đặt lại thời gian chuyển lượt về giá trị mặc định
        switchDelay = defaultSwitchDelay;
        ResetSwitchCycle();
    }

    // Hàm để tạo ra một player mới
    protected virtual Transform SpawnPlayer(string name, Transform point)
    {
        return PlayerManager.Instance.Spawn(name, point.position);

    } 
    protected virtual Transform SpawnPlayerTable(string name, Transform point)
    {
        return PlayerTableManager.Instance.Spawn(name, point.position);

    }
    //protected virtual Transform SpawnPlayerTable(string name, Transform point)
    //{
    //    return PlayerTableManager.Instance.Spawn(name, point.position);
    //}

    // Hàm để thêm player vào hàng đợi
    protected void AddPlayerToQueue(Transform player)
    {
        queuePlayer.Enqueue(player);
    }

    // Hàm để tắt các component của một player
    public void turnOffComponent(Transform player)
    {
        if (player == null) return;

        var playerMoving = player.GetComponent<PlayerMoving>();
        if (playerMoving != null) playerMoving.enabled = false;

        var animator = player.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        var canonRotation = player.GetComponentInChildren<CanonRotation>();
        if (canonRotation != null) canonRotation.enabled = false;

        var playerAttack = player.GetComponentInChildren<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = false;
        
        //player.GetComponent<DamageReceiver>().playertable.GetComponent<PlayerHP>().enabled = false; 
        //player.GetComponent<DamageReceiver>().playertable.GetComponent<PlayerForce>().enabled = false;
        
    }

    // Hàm để bật các component của một player
    public void turnOnComponent(Transform player)
    {
        if (player == null) return;

        var playerMoving = player.GetComponent<PlayerMoving>();
        if (playerMoving != null) playerMoving.enabled = true;

        var animator = player.GetComponent<Animator>();
        if (animator != null) animator.enabled = true;

        var canonRotation = player.GetComponentInChildren<CanonRotation>();
        if (canonRotation != null) canonRotation.enabled = true;

        var playerAttack = player.GetComponentInChildren<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = true;
        //player.GetComponent<DamageReceiver>().playertable.GetComponent<PlayerHP>().enabled = true;
        //player.GetComponent<DamageReceiver>().playertable.GetComponent<PlayerForce>().enabled = true;
    }

    // Hàm để xóa player khỏi hàng đợi và danh sách
    public void RemovePlayer(Transform player)
    {
        // Xóa player khỏi danh sách
        players.Remove(player);

        // Xóa player khỏi hàng đợi chính
        Queue<Transform> newQueue = new Queue<Transform>();
        while (queuePlayer.Count > 0)
        {
            Transform dequeuedPlayer = queuePlayer.Dequeue();
            if (dequeuedPlayer != player)
            {
                newQueue.Enqueue(dequeuedPlayer);
            }
        }
        queuePlayer = newQueue;

        // Xóa player khỏi team chẵn hoặc lẻ
        if (teamEven.Contains(player))
        {
            teamEven.Remove(player);
        }
        else if (teamOdd.Contains(player))
        {
            teamOdd.Remove(player);
        }

        // Kiểm tra nếu ActivePlayer bị tiêu diệt, chuyển sang player mới
        if (ActivePlayer == player)
        {
            if (queuePlayer.Count > 0)
            {
                ActivePlayer = queuePlayer.Peek();
                turnOnComponent(ActivePlayer);
            }
            else
            {
                ActivePlayer = null;
            }
        }
    }

    // Hàm để kiểm tra player thuộc team nào
    public string GetPlayerTeam(Transform player)
    {
        if (teamEven.Contains(player))
        {
            return "Team Chẵn";
        }
        else if (teamOdd.Contains(player))
        {
            return "Team Lẻ";
        }
        else
        {
            return "Không thuộc team nào";
        }
    }

    // Hàm để kiểm tra xem có hàng đợi nào rỗng không
    public bool IsAnyQueueEmpty()
    {
        return teamEven.Count == 0 || teamOdd.Count == 0;
    }

    // Coroutine để chuyển lượt theo chu kỳ
    private IEnumerator SwitchActivePlayerCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchDelay);
            SwitchActivePlayer();
        }
    }

    // Hàm để đặt lại chu kỳ chuyển lượt
    public void ResetSwitchCycle()
    {
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }
        switchCoroutine = StartCoroutine(SwitchActivePlayerCycle());
    }

    // Hàm để chuyển lượt sau một khoảng thời gian ngắn
    public IEnumerator SwitchActivePlayerAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchActivePlayer();
    }

    // Hàm để xử lý sự kiện SpawnBullet
    public void OnBulletSpawned()
    {
        // Đặt thời gian chuyển lượt thành 3 giây và reset chu kỳ
        switchDelay = 0f;
        ResetSwitchCycle();
    }
}
