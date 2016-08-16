using UnityEngine;
using System.Collections;


/*
 * Player
 *  
 * This script is attached to the in-combat object of the player
 * 
 */
public class PlayerObject : FriendlyObject
{
    void Start()
    {
        // Get hp, ap, etc from player script
        CurrentHp = PlayerManager.Instance.MaxHp;
        MaxHp = PlayerManager.Instance.MaxHp;

        MaxAp = PlayerManager.Instance.ActionPoints;
        CurrentAp = MaxAp;
    }
}
