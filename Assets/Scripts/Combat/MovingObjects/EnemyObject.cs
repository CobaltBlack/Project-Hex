using UnityEngine;
using System.Collections;

public abstract class EnemyObject : MovingObject
{
    EnemyType Type;
    EnemyRarity Rarity;

    public void SetData(EnemyData data)
    {
        Name = data.Name;
        Type = data.Type;
        Rarity = data.Rarity;
        MaxAp = data.MaxAp;
    }

    public abstract void QueueTurnActions();

    protected int GetPlayerCoordX()
    {
        return CombatManager.Instance.PlayerScript.X;
    }

    protected int GetPlayerCoordY()
    {
        return CombatManager.Instance.PlayerScript.Y;
    }

    // Queues a MoveAction towards the player with remaining AP
    protected void MoveTowardPlayer()
    {
        var path = CombatBoardManager.Instance.GetTilesInPath(X, Y, GetPlayerCoordX(), GetPlayerCoordY());

        // Remove last tile because that's where the player is
        path.RemoveAt(path.Count - 1);

        // Remove extra tiles from the end of the path 
        if (MoveRange < path.Count)
        {
            path.RemoveRange(MoveRange, path.Count - MoveRange);
        }

        var targetX = path[path.Count - 1].X;
        var targetY = path[path.Count - 1].Y;
        MoveAction moveAction = new MoveAction(targetX, targetY, path);
        ActionQueue.Add(moveAction);
    }
}
