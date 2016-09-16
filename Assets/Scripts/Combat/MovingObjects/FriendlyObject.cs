using UnityEngine;
using System.Collections;

public class FriendlyObject : MovingObject
{
    public GameObject CharacterShadow = null;

    // Animate the shadow if queueing a move command for friendly characters
    public override void QueueMoveAction(int targetX, int targetY)
    {
        var path = CombatBoardManager.Instance.GetTilesInPath(ShadowX, ShadowY, targetX, targetY, false);
        var moveAction = new MoveAction(path);
        ActionQueue.Add(moveAction);

        // Decrease currentAp
        CurrentAp -= path.Count * Constants.ApCostPerMove;

        // Move shadow
        MoveShadow(moveAction);


        // Update UI visuals for new action queued
    }

    public bool IsShadowActive
    {
        get
        {
            if (CharacterShadow != null && CharacterShadow.activeSelf) return true;
            else return false;
        }
    }

    public int ShadowX
    {
        get
        {
            if (CharacterShadow)
                return CharacterShadow.GetComponent<MovingObject>().X;
            else
                return X;
        }
    }

    public int ShadowY
    {
        get
        {
            if (CharacterShadow)
                return CharacterShadow.GetComponent<MovingObject>().Y;
            else
                return Y;
        }
    }

    // Display the shadow for the current object
    // Activated when player attempts to move the current character
    public void ShowShadow()
    {
        // Instantiates the shadow if it is not yet instantiated
        if (!CharacterShadow)
        {
            // Instantiate a copy of current object and make it transparent
            CharacterShadow = Instantiate(gameObject, CombatBoardManager.Instance.GetHexTile(X, Y).Position, Quaternion.identity) as GameObject;
            CharacterShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
        }
        // Otherwise, make it active
        else
        {
            CharacterShadow.SetActive(true);
        }
    }

    // Hide the shadow when it is not needed
    public void HideShadow()
    {
        if (IsShadowActive)
        {
            CharacterShadow.SetActive(false);
        }
    }

    // Moves the shadow based on the moveAction
    // Once movement is done, it redisplays the overlays
    void MoveShadow(MoveAction moveAction)
    {
        if (!IsShadowActive)
        {
            ShowShadow();
        }

        CombatBoardManager.Instance.SetObjectOnTileQueued(moveAction.TargetX, moveAction.TargetY, this);

        System.Action ShowOverlayCallback = CombatManager.Instance.ShowMovementOverlays;
        CharacterShadow.GetComponent<FriendlyObject>().RunMoveAction(moveAction, ShowOverlayCallback, false);
    }
}
