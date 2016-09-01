using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveAction : CombatAction
{
    public int TargetX;
    public int TargetY;
    public List<HexTile> Path;

    public MoveAction(List<HexTile> path)
    {
        this.ActionType = ActionType.Move;
        this.RequiredAp = path.Count * Constants.ApCostPerMove;
        this.TargetX = path[path.Count - 1].X;
        this.TargetY = path[path.Count - 1].Y;
        this.Path = path;
    }
}
