using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * SkillDatabase
 * 
 * This script file manages the skill database
 * 
 */
public class SkillDatabase : MonoBehaviour
{
    // Singleton pattern allows SkillDatabase to be called anywhere
    public static SkillDatabase Instance = null;

    // List of items defined in the Unity Inspector
    public SkillList SkillList;

    Dictionary<string, SkillData> SkillDict = new Dictionary<string, SkillData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Put the skill list into a dictionary for fast lookup
        foreach (var skill in SkillList.Skills)
        {
            SkillDict.Add(skill.name, skill);
        }
    }

    // Returns a skill by its unique name (file name of the asset)
    // Returns null if the skill name does not exist
    public SkillData getSkillByName(string name)
    {
        if (!SkillDict.ContainsKey(name))
        {
            Debug.LogErrorFormat("Skill not found: {0}", name);
            return null;
        }
        else
        {
            // Use a clone so the database values cannot be edited
            var clone = Instantiate(SkillDict[name]) as SkillData;
            return clone;
        }
    }
}
