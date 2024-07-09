using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode1vs1 : Mode
{
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        // Gán AudioSource component
        audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public override void OnButtonClick()
    {
        audioManger.PlaySFX(audioManger.ButtonClick);
        Debug.Log("Button clicked in Mode1vs1");
        GameManager.instance.playerCount = 2;
        GameManager.instance.initialPlayerCount=2;
        PickPlayer.instance.isInitialized = false;
        GameObject.Find("HomeScene").SetActive(false);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
