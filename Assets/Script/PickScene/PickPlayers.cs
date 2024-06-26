using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayer : MonoBehaviour
{   public static PickPlayer instance;
    public string PickPlayerName = "PickPlayer";
    public Transform PlayerPicks;
    public List<Transform> ListPlayer; // Ensure this is a member variable
    public int playerCount = 0;
    public Transform ActivePlayer;
    private int activePlayerIndex = 0;
    public List<Transform> ListPlayerInGames;

    public bool isInitialized = true; // Flag to ensure Update logic runs only once

    private void Awake()
    {
        instance = this;
    }
    // Phương thức để tải các players
    public virtual void LoadPlayer()
    {
        PlayerPicks = GameObject.Find(PickPlayerName).transform;
        ListPlayer = new List<Transform>(); // Initialize ListPlayer as a member variable
        foreach (Transform t in PlayerPicks)
        {
            ListPlayer.Add(t);
            //t.gameObject.SetActive(false); // Tắt tất cả các player
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer(); // Tải các player
    }

    void Update()
    {
       
        playerCount = GameManager.instance.playerCount;

        if (!isInitialized && playerCount != 0)
        {
            if (ListPlayer.Count > 0)
            {
                ActivePlayer = ListPlayer[0];
                ActivePlayer.gameObject.SetActive(true); // Bật ActivePlayer
            }

            if (playerCount==2)
            {
                ListPlayer[3].gameObject.SetActive(false);
                ListPlayer[2].gameObject.SetActive(false);

            }
           
            

            isInitialized = true; // Set the flag to true to ensure this logic runs only once
        }
    }

    // Phương thức để chọn player tiếp theo
    public void NextPlayer()
    {
        if (ListPlayer.Count == 0)
        {
            return;
        }

        ActivePlayer.GetComponent<Player>().enabled=false; // Tắt ActivePlayer hiện tại
        activePlayerIndex = (activePlayerIndex + 1) % ListPlayer.Count; // Chuyển sang player tiếp theo, nếu vượt quá thì quay về 0
        ActivePlayer = ListPlayer[activePlayerIndex];
        ActivePlayer.gameObject.SetActive(true); // Bật player mới
    }
}