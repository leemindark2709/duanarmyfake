using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP instance;
    private void Awake()
    {
        instance = this;
    }
    public Image imagehp;
    // Start is called before the first frame update
    public void UpdateHP(float CurrentHP, float MaxHP)
    {
        imagehp.fillAmount = CurrentHP / MaxHP;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
