using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PickPlayer pickPlayer;
    public static GameManager instance;
    public float requiredDistance = 0f;
    public int playerCount=0; // Số lượng Player muốn tạo
    public List<Transform> points;
    public List<Transform> pointsTable;
    public List<Transform> players;
    public Transform ActivePlayer; // Biến để theo dõi Player đang hoạt động
    public float enableTime;
    private Queue<Transform> queuePlayer = new Queue<Transform>();
    private Queue<Transform> queueTable = new Queue<Transform>();
    public List<Transform> teamEven = new List<Transform>();
    public List<Transform> teamOdd = new List<Transform>();
    private Coroutine switchCoroutine;
    private float defaultSwitchDelay = 10f;
    private float switchDelay = 10f;
    private bool playersCreated = false; // Biến cờ để theo dõi việc tạo player
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pickPlayer = FindObjectOfType<PickPlayer>();
        players = new List<Transform>();
        PlayerAppear.Instance.LoadCheckPoint();
        PlayerAppear.Instance.GetPoints(playerCount, requiredDistance, out points);
        PlayerTableManager.Instance.LoadPlayer();
        PlayerTableAppear.Instance.LoadCheckPoint();
        PlayerTableAppear.Instance.GetPoints(playerCount, requiredDistance, out pointsTable);

        // Đặt Player đầu tiên trong hàng đợi làm ActivePlayer ban đầu
       
    }

    private void Update()
    {
        if (!playersCreated && playerCount == pickPlayer.ListPlayerInGames.Count)
        {
            for (int i = 0; i < playerCount; i++)
            {
                Transform player = SpawnPlayer(pickPlayer.ListPlayerInGames[i].transform.name, points[i]);
                player.gameObject.SetActive(true);
                Transform playerTable = SpawnPlayerTable("PlayerTable", pointsTable[i]);
                playerTable.gameObject.SetActive(true);
                player.GetComponent<DamageReceiver>().playertable = playerTable;

                if (player != null)
                {
                    turnOffComponent(player);
                    player.Find("Status").gameObject.SetActive(false);
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
            if (queuePlayer.Count > 0)
            {
                ActivePlayer = queuePlayer.Peek(); 
                turnOnComponent(ActivePlayer);
                ActivePlayer.Find("Status").gameObject.SetActive(true);
            }
            switchCoroutine = StartCoroutine(SwitchActivePlayerCycle());
            playersCreated = true; // Đánh dấu rằng các player đã được tạo
        }
        

        // Bắt đầu chu kỳ chuyển lượt
     
    }
    // Hàm để chuyển đổi ActivePlayer và bật/tắt các component
    void SwitchActivePlayer()
    {
        if (queuePlayer.Count == 0)
        return;
        Transform previousPlayer = queuePlayer.Dequeue();
        turnOffComponent(previousPlayer);
        previousPlayer.Find("Status").gameObject.SetActive(false);
        queuePlayer.Enqueue(previousPlayer);
        ActivePlayer = queuePlayer.Peek();
        turnOnComponent(ActivePlayer);
        ActivePlayer.Find("Status").gameObject.SetActive(true);
        switchDelay = defaultSwitchDelay;
        ResetSwitchCycle();
    }

    protected virtual Transform SpawnPlayer(string name, Transform point)
    {
        return PlayerManager.Instance.Spawn(name, point.position);
    }

    protected virtual Transform SpawnPlayerTable(string name, Transform point)
    {
        return PlayerTableManager.Instance.Spawn(name, point.position);
    }

    protected void AddPlayerToQueue(Transform player)
    {
        queuePlayer.Enqueue(player);
    }

    public void turnOffComponent(Transform player)
    {
        

        if (player == null) return;
        var playerMoving = player.GetComponent<PlayerMoving>();
        if (playerMoving != null) playerMoving.enabled = false;
        enableTime = 0;

        var animator = player.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        var canonRotation = player.GetComponentInChildren<CanonRotation>();
        if (canonRotation != null) canonRotation.enabled = false;

        var playerAttack = player.GetComponentInChildren<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = false;
        player.GetComponent<DamageReceiver>().playertable.Find("panel").GetComponent<SpriteRenderer>().enabled = false ;
    }

    public void turnOnComponent(Transform player)
    {   
        player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>().enabled = true;
        if (player == null) return;
        enableTime = Time.time;
        var playerMoving = player.GetComponent<PlayerMoving>();
        if (playerMoving != null) playerMoving.enabled = true;

        var animator = player.GetComponent<Animator>();
        if (animator != null) animator.enabled = true;

        var canonRotation = player.GetComponentInChildren<CanonRotation>();
        if (canonRotation != null) canonRotation.enabled = true;

        var playerAttack = player.GetComponentInChildren<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = true;
        var playerForce = player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>();
        if (playerAttack != null) playerForce.enabled = true;
        var timeActive = player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>();
        if (playerAttack != null) timeActive.enabled = true;
        player.GetComponent<DamageReceiver>().playertable.Find("panel").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void RemovePlayer(Transform player)
    {
        players.Remove(player);

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

        if (teamEven.Contains(player))
        {
            teamEven.Remove(player);
        }
        else if (teamOdd.Contains(player))
        {
            teamOdd.Remove(player);
        }

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

    public bool IsAnyQueueEmpty()
    {
        return teamEven.Count == 0 || teamOdd.Count == 0;
    }

    private IEnumerator SwitchActivePlayerCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchDelay);
            SwitchActivePlayer();
        }
    }

    public void ResetSwitchCycle()
    {
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }
        switchCoroutine = StartCoroutine(SwitchActivePlayerCycle());
    }

    public IEnumerator SwitchActivePlayerAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchActivePlayer();
    }

    public void OnBulletSpawned()
    {
        switchDelay = 2f;
        ResetSwitchCycle();
    }

    public string GetRemainingTeam()
    {
        if (teamEven.Count == 0 && teamOdd.Count == 0)
        {
            return "Cả hai team đều không có người.";
        }
        else if (teamEven.Count == 0)
        {
            return "Team Lẻ win.";
        }
        else if (teamOdd.Count == 0)
        {
            return "Team Chẵn win.";
        }
        return "";
    }
}
