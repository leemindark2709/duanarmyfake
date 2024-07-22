using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneManager : MonoBehaviour
{
    public static HomeSceneManager instance;
    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    void Start()
    {
        //Example usage of AudioManager
        if (audioManager != null)
        {
            audioManager.PlayBackgroundMusic(); // Assume PlayBackgroundMusic() is a method in your AudioManager
        }
        else
        {
            Debug.LogError("AudioManager is not assigned!");
        }
    }
    void OnDisable()
    {
        // Stop background music when HomeSceneManager is disabled
        if (audioManager != null)
        {
            audioManager.StopBackgroundMusic();
        }
    }

    private void OnEnable()
    {
        //Additional logic when scene is enabled
    }

    void Update()
    {
        // Your update logic here if needed
    }
}
