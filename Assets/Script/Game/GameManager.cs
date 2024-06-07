using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float requiredDistance = 2f;
    public int playerCount = 2; // Số lượng Player muốn tạo
    public List<Transform> points;
    public List<Transform> players;
    public Transform ActivePlayer; // Biến để theo dõi Player đang hoạt động
    private Queue<Transform> queuePlayer = new Queue<Transform>();

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

        // Tạo và thêm các Player vào hàng đợi
        for (int i = 0; i < playerCount; i++)
        {
            Transform player = SpawnPlayer("Player", points[i]);
            if (player != null)
            {
                turnOffComponent(player);
                AddPlayerToQueue(player);
                players.Add(player);
            }
        }

        // Đặt Player đầu tiên trong hàng đợi làm ActivePlayer ban đầu
        if (queuePlayer.Count > 0)
        {
            ActivePlayer = queuePlayer.Peek();
            turnOnComponent(ActivePlayer);
        }

        // Lặp lại việc cập nhật ActivePlayer sau mỗi 10 giây
        InvokeRepeating("SwitchActivePlayer", 0f, 10f);
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
    }

    // Hàm để tạo ra một player mới
    protected virtual Transform SpawnPlayer(string name, Transform point)
    {
        return PlayerManager.Instance.Spawn(name, point.position);
    }

    // Hàm để thêm player vào hàng đợi
    protected void AddPlayerToQueue(Transform player)
    {
        queuePlayer.Enqueue(player);
    }

    // Hàm để tắt các component của một player
    protected void turnOffComponent(Transform player)
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
    }

    // Hàm để bật các component của một player
    protected void turnOnComponent(Transform player)
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
    }

    // Hàm để xóa player khỏi hàng đợi và danh sách
    public void RemovePlayer(Transform player)
    {
        // Xóa player khỏi danh sách
        players.Remove(player);

        // Xóa player khỏi hàng đợi
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
}
