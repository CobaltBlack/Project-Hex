using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// Example Instant Skill
[CreateAssetMenu(fileName = "FanOfTomatoes", menuName = "Skills/FanOfTomatoes")]
public class FanOfTomatoes : InstantSkill
{
    // ==============================
    // Unity Inspector Values
    // ==============================

    public int DamageAmount;
    public int HealAmount;
    public int Range;
    public GameObject SkillAnimation;

    // ==============================
    // Public Functions
    // ==============================

    // Specify the animation when the skill is executed
    public override void PlaySkillAnimation(MovingObject sourceObject, List<MovingObject> affectedObjects, Action callback)
    {
        _callback = callback;

        enemiesHit = 0;
        enemiesTotal = affectedObjects.Count;

        // Create a projectile affect targeted at all affected objects
        var startPos = sourceObject.transform.position;
        foreach (var affectedObject in affectedObjects)
        {
            var animationObject = Instantiate(SkillAnimation, startPos, Quaternion.identity) as GameObject;
            animationObject.GetComponent<ProjectileEffect>().MoveTowards(affectedObject, AnimationCallback);
        }
    }

    // Specify what objects are affected by the skill
    public override List<MovingObject> GetAffectedObjects(MovingObject sourceObject)
    {
        return CombatBoardManager.Instance.GetObjectsInRange(sourceObject.X, sourceObject.Y, Range);
    }

    // Specify how the skill affects the targets
    public override void SkillEffect(MovingObject sourceObject, List<MovingObject> affectedObjects)
    {
        Debug.Log(string.Format("All enemies withing {0} range took {1} damage!", Range, DamageAmount));
    }

    // ==============================
    // Private Functions
    // ==============================

    int enemiesHit = 0;
    int enemiesTotal = 0;

    // This function runs when the animation completes
    void AnimationCallback()
    {
        // Check that all instances of the animation has completed
        enemiesHit++;
        if (enemiesHit < enemiesTotal) return;

        if (_callback == null)
        {
            Debug.LogError(String.Format("Animation callback for the skill {0} is not set!", this.DisplayName));
        }
        else
        {
            // Run the original callback 
            _callback();
        }
    }
}
