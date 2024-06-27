using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public PickPlayer pickPlayer;
    public Transform Character;
    // Start is called before the first frame update
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        // Gán AudioSource component
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
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
    public void OnButtonClick()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);
        pickPlayer.ActivePlayer.GetComponent<Image>().sprite= this.transform.GetComponent<Image>().sprite;
        pickPlayer.ActivePlayer.GetComponent<Player>().player=this.transform.GetComponent<CharacterButton>().Character;
    }
}
