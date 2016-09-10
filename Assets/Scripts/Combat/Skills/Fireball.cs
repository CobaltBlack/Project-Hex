using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Fireball", menuName = "Skills/Fireball")]
public class Fireball : SkillData
{
    public int BaseDamage;

    public override void SkillEffect()
    {
        Debug.Log(string.Format("Boom a fireball for {0} damage!", BaseDamage));
    }
}
