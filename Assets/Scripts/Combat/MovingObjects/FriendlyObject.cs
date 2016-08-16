using UnityEngine;
using System.Collections;

public class FriendlyObject : MovingObject
{
    void Start()
    {
        // Get hp, ap, etc from player script
        //int currentHp = PlayerManager.instance.maxHp;
        //int maxHp = PlayerManager.instance.maxHp;

        //int maxAp = PlayerManager.instance.actionPoints;
        //int currentAp = maxAp;
    }

    // Animate the shadow if queueing a move command for friendly characters
    public new void QueueMoveAction(int x, int y)
    {
        base.QueueMoveAction(x, y);
        MoveShadow(x, y);
    }

    // ====================================
    // Functions for Shadow object
    // - The shadow is displayed during the player turn when planning Move actions
    // ====================================

    public int ShadowPositionX
    {
        get
        {
            if (CharacterShadow)
                return CharacterShadow.GetComponent<MovingObject>().PositionX;
            else
                return PositionX;
        }
    }

    public int ShadowPositionY
    {
        get
        {
            if (CharacterShadow)
                return CharacterShadow.GetComponent<MovingObject>().PositionY;
            else
                return PositionY;
        }
    }

    // Display the shadow for the current object
    // Activated when player attempts to move the current character
    void ShowShadow()
    {
        // Instantiates the shadow if it is not yet instantiated
        if (!CharacterShadow)
        {
            // Instantiate a copy of current object and make it transparent
            CharacterShadow = Instantiate(gameObject, CombatBoardManager.Instance.GetHexPosition(PositionX, PositionY), Quaternion.identity) as GameObject;
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
        if (CharacterShadow)
        {
            CharacterShadow.SetActive(false);
        }
    }

    void MoveShadow(int x, int y)
    {
        if (!CharacterShadow || !CharacterShadow.activeSelf)
        {
            ShowShadow();
        }

        CharacterShadow.GetComponent<MovingObject>().MoveToPosition(x, y);
    }
}
