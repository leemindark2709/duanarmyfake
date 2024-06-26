using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode1vs1 : Mode
{
    public override void OnButtonClick()
    {
        Debug.Log("Button clicked in Mode1vs1");
        GameManager.instance.playerCount = 2;
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
