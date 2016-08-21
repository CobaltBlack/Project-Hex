using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveAction : CombatAction
{
    public int TargetX;
    public int TargetY;
    public List<HexTile> Path;

    public MoveAction(int targetX, int targetY, List<HexTile> path)
    {
        ActionType = ActionType.Move;
        this.TargetX = targetX;
        this.TargetY = targetY;
        this.Path = path;
    }
}
