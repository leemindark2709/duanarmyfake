using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode2vs2 : Mode
{
   
    [SerializeField]private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        // Gán AudioSource component
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public override void OnButtonClick()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);
        GameManager.instance.playerCount = 4;
        PickPlayer.instance.isInitialized = false;
        GameObject.Find("HomeScene").SetActive(false);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
