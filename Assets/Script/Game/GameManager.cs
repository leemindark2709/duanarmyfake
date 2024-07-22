using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PickPlayer pickPlayer;
    public static GameManager instance;
    public float requiredDistance = 0f;
    public int playerCount = 0; // Số lượng Player muốn tạo
    public int initialPlayerCount = 0; // Giá trị ban đầu của playerCount
    public List<Transform> points;
    public List<Transform> pointsTable;
    public List<Transform> players;
    public Transform ActivePlayer; // Biến để theo dõi Player đang hoạt động
    public float enableTime;
    [SerializeField] private Queue<Transform> queuePlayer = new Queue<Transform>();
    [SerializeField] private Queue<Transform> queueTable = new Queue<Transform>();
    public List<Transform> teamEven = new List<Transform>();
    public List<Transform> teamOdd = new List<Transform>();
   [SerializeField] private Coroutine switchCoroutine;
    private float defaultSwitchDelay = 10f;
    private float switchDelay = 10f;
    public bool playersCreated = false; // Biến cờ để theo dõi việc tạo player

    public bool delayStarted = false;
    private float delayStartTime;
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        instance = this;
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        pickPlayer = FindObjectOfType<PickPlayer>();
        players = new List<Transform>();
        PlayerTableManager.Instance.LoadPlayer();
        PlayerTableAppear.Instance.LoadCheckPoint();
        PlayerTableAppear.Instance.GetPoints(playerCount, requiredDistance, out pointsTable);
        

        // Lưu trữ giá trị ban đầu của playerCount
        initialPlayerCount = playerCount;
    }
    public void GetPointToSpaw()
    {
        PlayerAppear.Instance.LoadCheckPoint();
        PlayerAppear.Instance.GetPoints(playerCount, requiredDistance, out points);
       
    }

    private void Update()
    {
        if (!playersCreated && playerCount == pickPlayer.ListPlayerInGames.Count && playerCount > 0)
        {
            if (!delayStarted)
            {
                if (!playersCreated)
                {
                    MapManager.Instance.SpawnMap(); // Gọi phương thức spawn map
                   
                }
                delayStarted = true;
                delayStartTime = Time.time;
            }
            if (Time.time >= delayStartTime + 3f)
            {
                GetPointToSpaw(); 

                for (int i = 0; i < playerCount; i++)
                {
                    if (i < pickPlayer.ListPlayerInGames.Count && i < points.Count && i < pointsTable.Count)
                    {
                        Transform player = SpawnPlayer(pickPlayer.ListPlayerInGames[i].transform.name, points[i]);
                        player.gameObject.SetActive(true);
                        Transform playerTable = SpawnPlayerTable("PlayerTable", pointsTable[i]);
                        playerTable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>().timerImage.fillAmount = 0f;
                        AddPlayerTableToQueue(playerTable);
                       playerTable.gameObject.SetActive(true);
                        player.GetComponent<DamageReceiver>().playertable = playerTable;
                        if (player != null)
                        {
                            turnOffComponent(player);
                            player.Find("Status").Find("arrow").gameObject.SetActive(false);
                            AddPlayerToQueue(player);
                            players.Add(player);

                            // Thêm player vào team chẵn hoặc lẻ
                            if (i % 2 == 0)
                            {
                                teamEven.Add(player);
                                player.Find("Status").Find("Canvas").Find("teameven").gameObject.SetActive(true);
                                
                            }
                            else
                            {
                                teamOdd.Add(player);
                                player.Find("Status").Find("Canvas").Find("teamolder").gameObject.SetActive(true);
                            }
                        }
                    }
                }

                if (queuePlayer.Count > 1)
                {
                    ActivePlayer = queuePlayer.Peek();
                    if (ActivePlayer != null)
                    {
                        turnOnComponent(ActivePlayer);
                        ActivePlayer.Find("Status").gameObject.SetActive(true);
                        ActivePlayer.Find("Status").Find("arrow").gameObject.SetActive(true);

                    }

                }

                if (queuePlayer.Count > 1 && teamEven.Count > 0 && teamOdd.Count > 0)
                {

                    switchCoroutine = StartCoroutine(SwitchActivePlayerCycle());
                }


                playersCreated = true; // Đánh dấu rằng các player đã được tạo
            }
        }
    }

    // Hàm để chuyển đổi ActivePlayer và bật/tắt các component
    void SwitchActivePlayer()
    {
        if (queuePlayer.Count <= 1)
            return;
        Transform previousPlayer = queuePlayer.Dequeue();
        turnOffComponent(previousPlayer);
        previousPlayer.Find("Status").Find("arrow").gameObject.SetActive(false);
        StartCoroutine(DelayedSetActivePlayer(previousPlayer));
    }

    IEnumerator DelayedSetActivePlayer(Transform player)
    {
        yield return new WaitForSeconds(2f);

        queuePlayer.Enqueue(player);
        ActivePlayer = queuePlayer.Peek();
        if (ActivePlayer!=null)
        {
            turnOnComponent(ActivePlayer);
            ActivePlayer.Find("Status").Find("arrow").gameObject.SetActive(true);

            switchDelay = defaultSwitchDelay;
            ResetSwitchCycle();

        }
       
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
    protected void AddPlayerTableToQueue(Transform player)
    {
        queueTable.Enqueue(player);
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
        player.GetComponent<DamageReceiver>().playertable.Find("panel").GetComponent<SpriteRenderer>().enabled = false;
        var timeActive = player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>();
        player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>().timerImage.fillAmount = 0f;
        if (playerAttack != null) timeActive.enabled = false;
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
        player.GetComponent<DamageReceiver>().playertable.Find("panel").gameObject.SetActive(false);
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
            Debug.Log(GetRemainingTeam());
        }
        else if (teamOdd.Contains(player))
        {
            teamOdd.Remove(player);
            Debug.Log(GetRemainingTeam());
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
        Debug.Log("ok" + queuePlayer.Count);
        while (queuePlayer.Count>1)
        {
            yield return new WaitForSeconds(switchDelay);
            if (IsAnyQueueEmpty())
            {
                StopCoroutine(switchCoroutine);
                
                    turnOffComponent(ActivePlayer);
                
                yield break;
            }
            audioManger.PlaySFX(audioManger.NextTurn);

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

    public void RemoveAllPlayersAndTables()
    {
        // Xoá tất cả các player table
        foreach (Transform table in queueTable)
        {
            Destroy(table.gameObject);
        }
        queueTable.Clear();
        // Xoá tất cả các player
        foreach (Transform player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();

        // Đặt lại trạng thái của các biến và queue
        queuePlayer = new Queue<Transform>();
        teamEven.Clear();
        teamOdd.Clear();
        points.Clear();
        ActivePlayer = null;
        playersCreated = false;
        playerCount = 0;
        enableTime = 0;
        ResetSwitchCycle(); 
    }

    public IEnumerator SwitchActivePlayerAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchActivePlayer();
    }

    public void OnBulletSpawned()
    {
        switchDelay = 2f;
        if (queuePlayer.Count>0)
        {
            ResetSwitchCycle();
        }
       
    }

    public string GetRemainingTeam()
    {
        if (teamEven.Count == 0 && teamOdd.Count == 0)
            return "";
        if (teamEven.Count == 0)
        {
            GameObject.Find("Teamwin").transform.Find("Canvas").transform.Find("Panel").gameObject.SetActive(true);
           TeamWinManager.instance.Team1Win.SetActive(false);
            audioManger.PlaySFX(audioManger.Win);
            return "Team Lẻ win.";
        }
        else if (teamOdd.Count == 0)
        {
            GameObject.Find("Teamwin").transform.Find("Canvas").transform.Find("Panel").gameObject.SetActive(true);
            TeamWinManager.instance.Team2Win.SetActive(false);
            audioManger.PlaySFX(audioManger.Win);
            return "Team Chẵn win.";
        }
        return "";
    }

    // Hàm để khôi phục giá trị playerCount về giá trị ban đầu
    public void RestoreInitialPlayerCount()
    {
        playerCount = initialPlayerCount;
    }
}
