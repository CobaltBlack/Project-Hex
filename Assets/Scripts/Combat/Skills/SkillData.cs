using UnityEngine;
using System.Collections;


/* SkillData
 *
 * Base class for a skill ScriptableObject. Contains properties common to all skills.
 *
 * To create a new skill follow these steps: 
 * 
 * 1. Make a new class that inherits from SkillData.
 * 2. Implement the SkillEffect function, which defines what happens when the skill is executed.
 * 3. Add extra public variables as needed to configure the skill values.
 * 4. Try to make skills as configurable as possible, so many different variations of the same skill script
 *    can be made just by changing the values in the Inspector.
 * 5. Add the new skill to the SkillList object, so that it is added to the lookup table.
 * 
 * Refer to existing skill scripts.
 */

public abstract class SkillData : ScriptableObject
{
    public string DisplayName = "New Skill";
    public string Description;
    public SkillType Type;
    public Sprite Icon;
    public int RequiredAp;
    public int CooldownTurns;

    // Define what happens when the skill is cast
    public abstract void SkillEffect();
}
