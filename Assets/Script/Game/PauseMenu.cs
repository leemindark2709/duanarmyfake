
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject darkpanel;
    [SerializeField] GameObject pause; 
    [SerializeField] GameObject reamake;


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
        darkpanel.SetActive(true);
        pause.SetActive(false);


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
        GameManager.instance.ActivePlayer = null;
        GameManager.instance.RemoveAllPlayersAndTables();
        GameManager.instance.delayStarted = false;
        PickPlayer.instance.ListPlayerInGames.Clear();
        GameManager.instance.RestoreInitialPlayerCount(); // Khôi phục giá trị playerCount ban đầu
        pickScene.SetActive(true);
        pauseMenu.SetActive(false);
        if (darkpanel!=null)
        {
            darkpanel.SetActive(false);
        }
        if (pause!=null)
        {
            pause.SetActive(true);
        }
        PickPlayer.instance.setnoneplayer();
       

        TeamWinManager.instance.Team1Win.SetActive(true);
        TeamWinManager.instance.Team2Win.SetActive(true);

        PickPlayer.instance.SetFalseImage();

        // Dừng coroutine cũ và bắt đầu lại coroutine mới
        //GameManager.instance.ResetSwitchCycle();

        Time.timeScale = 1f;
    }

    public void Resume()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);
        darkpanel.SetActive(false);
        pauseMenu.SetActive(false);
        pause.SetActive(true);
        Time.timeScale = 1f;
    }

}
