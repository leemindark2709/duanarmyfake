using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayer : MonoBehaviour
{
    public string PickPlayerName = "PickPlayer";
    public Transform PlayerPicks;
    public List<Transform> ListPlayer; // Ensure this is a member variable
    public int CountPlayer;
    public Transform ActivePlayer;
    private int activePlayerIndex = 0;
    public List<Transform>ListPlayerInGames;

    // Phương thức để tải các players
    public virtual void LoadPlayer()
    {

        PlayerPicks = GameObject.Find(PickPlayerName).transform;
        ListPlayer = new List<Transform>(); // Initialize ListPlayer as a member variable
        foreach (Transform t in PlayerPicks)
        {
            ListPlayer.Add(t);
            t.gameObject.SetActive(false); // Tắt tất cả các player
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CountPlayer = 4; // Đặt số lượng player cần hiển thị
        LoadPlayer(); // Tải các player
        if (ListPlayer.Count > 0)
        {
            ActivePlayer = ListPlayer[0];
            ActivePlayer.gameObject.SetActive(true); // Bật ActivePlayer
        }
        for (int i = 0; i < CountPlayer && i < ListPlayer.Count; i++)
        {
            ListPlayer[i].gameObject.SetActive(true); // Bật các player đầu tiên theo CountPlayer
        }
    }

    // Phương thức để chọn player tiếp theo
    public void NextPlayer()
    {
        if (ListPlayer.Count == 0)
        {
            return;
        }

        //ActivePlayer.gameObject.SetActive(false); // Tắt ActivePlayer hiện tại
        activePlayerIndex = (activePlayerIndex + 1) % ListPlayer.Count; // Chuyển sang player tiếp theo, nếu vượt quá thì quay về 0
        ActivePlayer = ListPlayer[activePlayerIndex];
        //ActivePlayer.gameObject.SetActive(true); // Bật player mới
    }

    // Update is called once per frame
    void Update()
    {

    }
}
