using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInGame : MonoBehaviour
{
    public static SkillInGame instance;

    public int numofusesq=2;
    public int numofusese=5;
    public int numofusesr=1;
    private void Awake()
    {
        instance = this;   
    }

}
