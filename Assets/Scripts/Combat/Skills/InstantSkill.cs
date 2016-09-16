using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class InstantSkill : SkillData
{
    public InstantSkill()
    {
        Type = SkillType.Instant;
    }

    // Returns list of objects affected by the skill
    public abstract List<MovingObject> GetAffectedObjects(MovingObject sourceObject);
}
