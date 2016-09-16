﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// TODO: Update skill to inherit from Ranged skill or something
[CreateAssetMenu(fileName = "Fireball", menuName = "Skills/Fireball")]
public class Fireball : SkillData
{
    public int BaseDamage;
    
    public override void PlaySkillAnimation(MovingObject sourceObject, List<MovingObject> affectedObjects, Action callback)
    {
        throw new NotImplementedException();
    }

    public override void SkillEffect(MovingObject sourceObject, List<MovingObject> affectedObjects)
    {
        Debug.Log(string.Format("Boom a fireball for {0} damage!", BaseDamage));
    }
}
