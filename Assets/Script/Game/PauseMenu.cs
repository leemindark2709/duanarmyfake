using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        // Gán AudioSource component
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
    public void Resume()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
