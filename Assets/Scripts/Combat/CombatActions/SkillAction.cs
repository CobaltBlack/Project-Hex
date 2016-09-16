using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Base class for an CombatAction that contains a Skill
public abstract class SkillAction : CombatAction
{
    public MovingObject SourceObject;
    public List<MovingObject> AffectedObjects;
    public SkillType SkillType;

    // Reference to MovingObject.ProcessNextCombatAction()
    protected Action _processNextCombatActionCallback = null;

    public SkillAction(SkillData skill)
    {
        RequiredAp = skill.RequiredAp;
        ActionType = ActionType.Skill;
    }

    //
    public abstract void ExecuteSkill(Action callback);
    public abstract void ApplySkillEffects();
}
