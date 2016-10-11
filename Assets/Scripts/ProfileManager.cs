using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ProfileManager : MonoBehaviour
{
    PlayerManager playerManagerScript;

    // the following items must be assigned in the inspector
    public GameObject profilePanel;
    public Text ProfileText; // assign Profile Text under Profile Panel

    void Awake()
    {
        Debug.Log("profile start");

        playerManagerScript = GetComponent<PlayerManager>();
    }

    void Update()
    {
        RefreshPlayerStatsText(); // removing this will result in no update after PlayerManager ProcessFlux
    }

    public void RefreshPlayerStatsText()
    {
        ProfileText.text = "empty" + "\n\n"
                        + playerManagerScript.CurrentHp + " / " + playerManagerScript.MaxHp + "\n\n"
                        + playerManagerScript.ActionPoints + "\n\n"
                        + playerManagerScript.Morality + "\n\n"
                        + playerManagerScript.Sanity + "\n\n"
                        + playerManagerScript.Attack + "\n\n"
                        + playerManagerScript.Crit + "\n\n"
                        + playerManagerScript.Defense + "\n\n"
                        + playerManagerScript.Dodge + "\n\n";
    }

    public void ToggleProfile()
    {
        if (profilePanel.activeSelf)
        {
            profilePanel.SetActive(false);
        }
        else
        {
            profilePanel.SetActive(true);
        }
    }
}
