using UnityEngine;
using System.Collections;
using System.Reflection;

public class ItemBehaviours : MonoBehaviour
{
    PlayerManager PlayerManagerScript;

    void Start()
    {
        PlayerManagerScript = GameObject.Find("MapManager").GetComponent<PlayerManager>();
    }

    public void LoseHealth(int amount)
    {
        PlayerManagerScript.ModifyHp(amount);
    }

    public void GainHealth(int amount)
    {
        PlayerManagerScript.ModifyHp(amount);
    }

    public void ExecuteByName(string functionName, int parameter)
    {
        switch (functionName)
        {
            case "LoseHealth":
                LoseHealth(parameter);
                break;
            case "GainHealth":
                GainHealth(parameter);
                break;
            default:
                Debug.Log("Error in finding ItemAction in ItemBehaviours");
                break;
        }

        PlayerManagerScript.RefreshPlayerStats();
    }
}