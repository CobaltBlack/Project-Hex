using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* SkillList
 * 
 * This scriptable object is used to store a list of Skills.
 * 
 * Storing the list of skills in a seperate object lets us avoid 
 * making updates to the database object in the Scene whenever we want to add skills.
 */

[CreateAssetMenu(fileName = "SkillList", menuName = "Skill List")]
public class SkillList : ScriptableObject
{
    public string Name;
    public List<SkillData> Skills;
}
