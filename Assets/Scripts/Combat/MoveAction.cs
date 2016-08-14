using UnityEngine;
using System.Collections;

public class MoveAction : CombatAction
{
    public int targetX, targetY;

    public MoveAction(int targetX, int targetY)
    {
        actionType = ActionType.MOVE;
        this.targetX = targetX;
        this.targetY = targetY;
    }
}
