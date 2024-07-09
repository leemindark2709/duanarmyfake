using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public GameObject pickScene;
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Start()
    {
        pickScene = ButtonOK.instance.pickScene;   // Gán AudioSource component
    }
    private void Awake()
    {
       
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    public void Pause()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

    } 
    public void Home()
    {
        
        //SceneManager.LoadScene("") 
        audioManger.PlaySFX(audioManger.ButtonClick);
       
        SceneManager.LoadScene("SampleScene");
   
        Time.timeScale = 1f;
    }
    public void Remake()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);
        GameObject.Find("Teamwin").transform.Find("Canvas").transform.Find("Panel").gameObject.SetActive(false);
        GameManager.instance.RemoveAllPlayersAndTables();
        PickPlayer.instance.ListPlayerInGames.Clear();
        GameManager.instance.RestoreInitialPlayerCount(); // Khôi phục giá trị playerCount ban đầu
      

        pickScene.SetActive(true);
        PickPlayer.instance.SetFalseImage();
        //Time.timeScale = 1f;
    }
    public void Resume()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
