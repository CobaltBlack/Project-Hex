using UnityEngine;
using System.Collections;

public class FriendlyObject : MovingObject {

    void Start()
    {
        // Get hp, ap, etc from player script
        //int currentHp = PlayerManager.instance.maxHp;
        //int maxHp = PlayerManager.instance.maxHp;

        //int maxAp = PlayerManager.instance.actionPoints;
        //int currentAp = maxAp;
    }

    // Animate the shadow if queueing a move command for friendly characters
    public new void queueMoveAction(int x, int y)
    {
        base.queueMoveAction(x, y);
        moveShadow(x, y);
    }

    // ====================================
    // Functions for Shadow object
    // - The shadow is displayed during the player turn when planning Move actions
    // ====================================

    public int shadowPositionX
    {
        get
        {
            if (characterShadow)
                return characterShadow.GetComponent<MovingObject>().positionX;
            else
                return positionX;
        }
    }

    public int shadowPositionY
    {
        get
        {
            if (characterShadow)
                return characterShadow.GetComponent<MovingObject>().positionY;
            else
                return positionY;
        }
    }

    // Display the shadow for the current object
    // Activated when player attempts to move the current character
    void showShadow()
    {
        // Instantiates the shadow if it is not yet instantiated
        if (!characterShadow)
        {
            // Instantiate a copy of current object and make it transparent
            characterShadow = Instantiate(gameObject, CombatBoardManager.instance.GetHexPosition(positionX, positionY), Quaternion.identity) as GameObject;
            characterShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
        }
        // Otherwise, make it active
        else
        {
            characterShadow.SetActive(true);
        }

    }

    // Hide the shadow when it is not needed
    public void hideShadow()
    {
        if (characterShadow)
        {
            characterShadow.SetActive(false);
        }
    }

    void moveShadow(int x, int y)
    {
        if (!characterShadow || !characterShadow.activeSelf)
        {
            showShadow();
        }

        characterShadow.GetComponent<MovingObject>().moveToPosition(x, y);
    }
}
