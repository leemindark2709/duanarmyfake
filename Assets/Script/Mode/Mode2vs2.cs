using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode2vs2 : Mode
{
    public override void OnButtonClick()
    {
        Debug.Log("Button clicked in Mode1vs1");
        GameManager.instance.playerCount = 4;
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
