using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource
    private void Awake()
    {
        // Gán AudioSource component
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    public void home()
    {

        //SceneManager.LoadScene("") 
        audioManger.PlaySFX(audioManger.ButtonClick);

        SceneManager.LoadScene("SampleScene");

        Time.timeScale = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
