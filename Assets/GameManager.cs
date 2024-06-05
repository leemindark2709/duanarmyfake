using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float requiredDistance = 2f;
    public int playerCount = 4; // Số lượng Player muốn tạo
    public List<Transform> points;
    public List<Transform> players;
    public Transform ActivePlayer; // Biến để theo dõi Player đang hoạt động
    Queue<Transform> QueuePlayer = new Queue<Transform>();

    private void Start()
    {
        players = new List<Transform>();

        PlayerManager.Instance.LoadPlayer();
        PlayerAppear.Instance.LoadCheckPoint();
        PlayerAppear.Instance.GetPoints(playerCount, requiredDistance, out points);

        // Tạo và thêm các Player vào hàng đợi
        for (int i = 0; i < points.Count; i++)
        {
            Transform player = SpawnPlayer("Player", points[i]);
            turnOffComponent(player);
            AddPlayerToQueue(player);
            players.Add(player);
        }

        // Đặt Player đầu tiên trong hàng đợi làm ActivePlayer ban đầu
        if (QueuePlayer.Count > 0)
        {
            ActivePlayer = QueuePlayer.Peek();
            turnOnComponent(ActivePlayer);
        }

        // Lặp lại việc cập nhật ActivePlayer sau mỗi 2 giây
        InvokeRepeating("SwitchActivePlayer", 0f, 2f);
    }

    void Update()
    {
        // Bạn có thể thực hiện các công việc khác trong Update() ở đây nếu cần thiết
    }

    // Hàm để chuyển đổi ActivePlayer và bật/tắt các component
    void SwitchActivePlayer()
    {
        // Nếu hàng đợi rỗng, không làm gì cả
        if (QueuePlayer.Count == 0)
            return;

        // Lấy và loại bỏ player hiện tại khỏi đầu hàng đợi
        Transform previousPlayer = QueuePlayer.Dequeue();

        // Tắt component của player hiện tại
        turnOffComponent(previousPlayer);

        // Đưa player hiện tại vào cuối hàng đợi
        QueuePlayer.Enqueue(previousPlayer);

        // Lấy player mới từ đầu hàng đợi
        ActivePlayer = QueuePlayer.Peek();

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
        QueuePlayer.Enqueue(player);
    }

    // Hàm để tắt các component của một player
    protected void turnOffComponent(Transform Player)
    {
        Player.GetComponent<PlayerMoving>().enabled = false;
        Player.GetComponent<Animator>().enabled = false;
        Player.GetComponentInChildren<CanonRotation>().enabled = false;
    }

    // Hàm để bật các component của một player
    protected void turnOnComponent(Transform Player)
    {
        Player.GetComponent<PlayerMoving>().enabled = true;
        Player.GetComponent<Animator>().enabled = true;
        Player.GetComponentInChildren<CanonRotation>().enabled = true;
    }
}
