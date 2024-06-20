using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
   public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

    } 
    public void Home()
    {
            //SceneManager.LoadScene("") 
    }
    public void Resume()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
