using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    public List<Transform> skillPrefabs; // List of skill prefabs


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        HideAll();
        LoadSkills();
    }

    public virtual void LoadSkills()
    {
        // Ensure skillPrefabs is initialized
        if (skillPrefabs == null)
        {
            skillPrefabs = new List<Transform>();
        }

        foreach (Transform skill in transform)
        {
            skillPrefabs.Add(skill);
        }
    }

    public virtual Transform Spawn(string skillName, Transform player)
    {
        Transform skillPrefab = GetSkillByName(skillName);
        skillPrefab.GetComponent<PlayerSkillHeal>().player = player;
        if (skillPrefab == null)
        {
            Debug.LogError("Skill prefab not found: " + skillName);
            return null;
        }

        Transform newSkill = Instantiate(skillPrefab, player);
       
        return newSkill;
    }

    public virtual Transform GetSkillByName(string skillName)
    {
        foreach (Transform skill in skillPrefabs)
        {
            if (skill.name == skillName)
            {
                return skill;
            }
        }
        return null;
    }

   

    protected virtual void HideAll()
    {
        foreach (Transform skill in this.transform)
        {
            skill.gameObject.SetActive(false);
        }
    }

    
}
