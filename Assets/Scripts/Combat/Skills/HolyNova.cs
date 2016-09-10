using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "HolyNova", menuName = "Skills/HolyNova")]
public class HolyNova : SkillData
{
    public int BaseDamage;
    public int Range;

    public override void SkillEffect()
    {
        Debug.Log(string.Format("All enemies withing {0} range took {1} damage!", Range, BaseDamage));
    }
}
