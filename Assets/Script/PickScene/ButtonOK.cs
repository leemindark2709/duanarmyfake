using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOK : MonoBehaviour
{
    public PickPlayer pickPlayer;

    // Start is called before the first frame update
    void Start()
    {
        pickPlayer = FindObjectOfType<PickPlayer>(); // Tìm đối tượng PickPlayer trong scene
        if (pickPlayer == null)
        {
            Debug.LogError("PickPlayer not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Phương thức để xử lý sự kiện click button
    public void OnButtonClick()
    {

        if (pickPlayer != null)
        {
            pickPlayer.ListPlayerInGames.Add(pickPlayer.ActivePlayer.GetComponent<Player>().player);
            pickPlayer.NextPlayer(); // Gọi phương thức NextPlayer từ PickPlayer
         

        }
    }
}
