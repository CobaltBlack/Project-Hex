using UnityEngine;
using System.Collections;

public class ExampleEnemy : EnemyObject
{   
    // Define the behavior of the enemy by queuing actions per turn
    public override void QueueTurnActions()
    {
        MoveTowardPlayer();
    }
}
