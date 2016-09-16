using UnityEngine;
using System.Collections;

public abstract class RangedSkill : SkillData
{
    public int Range;
    public int AoeRange;

    public RangedSkill()
    {
        Type = SkillType.Ranged;
    }
}
