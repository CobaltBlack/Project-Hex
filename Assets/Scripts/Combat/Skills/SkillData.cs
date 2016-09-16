using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum SkillType
{
    Instant,
    Target,
    Ranged,
    Directional,
};

/* SkillData
 *
 * Base class for a skill ScriptableObject. Contains properties common to all skills.
 *
 * To create a new skill follow these steps: 
 * 
 * 1. Create a new class that inherits from SkillData.
 * 2. Implement SkillEffect(), which defines what happens when the skill is executed.
 * 3. Add extra public variables as needed to configure the skill values (such as damage, range, anything really)
 * 4. Try to make your skill as configurable as possible, so many variations of the same skill
 *    can be made just by changing the values in the Inspector.
 * 5. Create the skill asset file in Unity (Create->Skills)
 * 6. Add the new skill asset to the SkillList asset, so that the skill can be found in the lookup table
 * 
 * Refer to existing skill scripts when writing a new one.
 */
public abstract class SkillData : ScriptableObject
{
    // ==========================
    // Inspector Input Fields
    // ==========================
    public string DisplayName = "New Skill";
    [TextArea]
    public string Description = "New Description";
    public int RequiredAp = 10;
    public int CooldownTurns = 1; // Number of turns between each execution of the skill
    public int MaxUsesPerTurn = 10; // Max times the skill can be used per turn
    public Sprite Icon;

    // ==========================
    // In-combat specific data
    // ==========================
    public bool IsOffCooldown { get { return (cooldownTurnsRemaining <= 0); } }
    public bool IsUsableThisTurn { get { return (timesUsedThisTurn < MaxUsesPerTurn); } }

    protected Action _callback = null;
    int timesUsedThisTurn = 0;
    int cooldownTurnsRemaining = 0;

    [HideInInspector]
    public SkillType Type;
    
    public void HandleTurnStart()
    {
        timesUsedThisTurn = 0;
        if (cooldownTurnsRemaining > 0) cooldownTurnsRemaining--;
    }

    public void HandleSkillQueued()
    {
        timesUsedThisTurn++;
        cooldownTurnsRemaining = CooldownTurns;
    }

    public abstract void PlaySkillAnimation(MovingObject sourceObject, List<MovingObject> affectedObjects, Action callback);

    // Defines what happens when the skill is cast
    // Must be implemented in inherited skill classes
    public abstract void SkillEffect(MovingObject sourceObject, List<MovingObject> affectedObjects);
}
