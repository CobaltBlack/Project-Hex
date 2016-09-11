using UnityEngine;
using System.Collections;

/* EnemyObject
 * 
 * Base class for a generic enemy
 * 
 * Inheriting classes must implement QueueTurnActions()
 * to define the behaviour of the enemy per turn
 */
public abstract class EnemyObject : MovingObject
{
    public EnemyType Type;
    public EnemyRarity Rarity;

    public void SetData(EnemyData data)
    {
        Name = data.Name;
        Type = data.Type;
        Rarity = data.Rarity;
        MaxAp = data.MaxAp;
    }

    public abstract void QueueTurnActions();

    // X coordinate of player
    protected int GetPlayerCoordX()
    {
        return CombatManager.Instance.PlayerScript.X;
    }

    // Y coordinate of player
    protected int GetPlayerCoordY()
    {
        return CombatManager.Instance.PlayerScript.Y;
    }

    // Queues a MoveAction towards the player with remaining AP
    // Returns true if an action was queued. false if no action was queued.
    protected bool MoveTowardPlayer()
    {
        var playerX = GetPlayerCoordX();
        var playerY = GetPlayerCoordY();
        var path = CombatBoardManager.Instance.GetTilesInPath(X, Y, playerX, playerY, false);
        if (path.Count < 1)
        {
            return false;
        }

        // Remove extra tiles from the end of the path until its under the MoveRange
        if (path.Count > MoveRange)
        {
            path.RemoveRange(MoveRange, path.Count - MoveRange);
        }

        var moveAction = new MoveAction(path);
        ActionQueue.Add(moveAction);

        CombatBoardManager.Instance.SetObjectOnTileQueued(moveAction.TargetX, moveAction.TargetY, this);

        return true;
    }
}
