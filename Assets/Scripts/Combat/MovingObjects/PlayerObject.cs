using UnityEngine;
using System.Collections;


/*
 * Player
 *  
 * This script is attached to the in-combat object of the player
 * 
 */
public class PlayerObject : MovingObject
{
    void Start()
    {
        // Get hp, ap, etc from player script
        int currentHp = PlayerManager.instance.maxHp;
        int maxHp = PlayerManager.instance.maxHp;

        int maxAp = PlayerManager.instance.actionPoints;
        int currentAp = maxAp;

        isFriendly = true;
    }
}
