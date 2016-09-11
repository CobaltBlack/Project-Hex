using UnityEngine;
using System.Collections;


/* PlayerObject
 *  
 * This script is attached to the in-combat object of the player
 */
public class PlayerObject : FriendlyObject
{
    public void InitializeData()
    {
        // Get hp, ap, skills from PlayerManager
        MaxHp = PlayerManager.Instance.MaxHp;
        CurrentHp = PlayerManager.Instance.CurrentHp;

        MaxAp = PlayerManager.Instance.ActionPoints;
        CurrentAp = MaxAp;

        Skills = PlayerManager.Instance.Skills;
    }
}
