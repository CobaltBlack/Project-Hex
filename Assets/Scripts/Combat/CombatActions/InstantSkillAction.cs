using UnityEngine;
using System.Collections;
using System;

// This class is a CombatAction that contains an InstantSkill
public class InstantSkillAction : SkillAction
{
    public InstantSkill Skill;

    public InstantSkillAction(InstantSkill skill) : base(skill)
    {
        Skill = skill;
        SkillType = skill.Type;
    }

    public override void ExecuteSkill(Action callback)
    {
        _processNextCombatActionCallback = callback;

        AffectedObjects = Skill.GetAffectedObjects(SourceObject);
        Skill.PlaySkillAnimation(SourceObject, AffectedObjects, ApplySkillEffects);
    }

    public override void ApplySkillEffects()
    {
        Skill.SkillEffect(SourceObject, AffectedObjects);
        _processNextCombatActionCallback();
    }
}
