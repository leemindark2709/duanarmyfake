using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOK : MonoBehaviour
{
    public static ButtonOK  instance;
    public PickPlayer pickPlayer;
    public GameObject pickScene;
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    public static object Instance { get; internal set; }

    private void Awake()
    {
        instance = this;
        // Gán AudioSource component
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        pickScene = GameObject.Find("PickScene");
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
        audioManger.PlaySFX(audioManger.ButtonClick);
        if (pickPlayer != null)
        {
            pickPlayer.ListPlayerInGames.Add(pickPlayer.ActivePlayer.GetComponent<Player>().player);
            pickPlayer.NextPlayer(); // Gọi phương thức NextPlayer từ PickPlayer
            

        }
        if (pickPlayer.ListPlayerInGames.Count == GameManager.instance.playerCount)
        {
            pickScene.SetActive(false);
        }
    }
}
