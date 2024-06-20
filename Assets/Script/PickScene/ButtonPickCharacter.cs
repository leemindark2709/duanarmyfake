using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPickCharacter : MonoBehaviour
{
     public string ButtonPickName = "ButtonPick";
    public Transform ButtonPicks;
    public List<Transform> ListButtonPick;
    public int countCharacter;

    public virtual void LoadButtonPick()
    {
        ButtonPicks = GameObject.Find(ButtonPickName).transform;
        ListButtonPick = new List<Transform>();
        foreach (Transform button in ButtonPicks)
        {
            ListButtonPick.Add(button);
            // Tắt button (nếu cần)
            button.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        
        //PlayerManager.Instance.LoadPlayer(); // Load players first
        countCharacter = PlayerManager.Instance.playerPrefabs.Count; // Đếm số lượng playerPrefabs
        LoadButtonPick(); // Tải và tắt các button khi bắt đầu

        for (int i = 0; i < countCharacter && i < ListButtonPick.Count; i++)
        {
            ListButtonPick[i].GetComponent<CharacterButton>().Character = PlayerManager.Instance.playerPrefabs[i];
            Image buttonImage = ListButtonPick[i].GetComponent<Image>();
            SpriteRenderer playerSpriteRenderer = PlayerManager.Instance.playerPrefabs[i].GetComponent<SpriteRenderer>();

            if (buttonImage != null && playerSpriteRenderer != null)
            {
                buttonImage.sprite = playerSpriteRenderer.sprite; 
                ListButtonPick[i].gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {

    }
}
