using UnityEngine;
using System.Collections;

public class MoveAction : CombatAction
{
    public int TargetX, TargetY;

    public MoveAction(int targetX, int targetY)
    {
        ActionType = ActionType.Move;
        this.TargetX = targetX;
        this.TargetY = targetY;
    }
}
