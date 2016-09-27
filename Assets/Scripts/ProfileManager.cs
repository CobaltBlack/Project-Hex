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

    void Start()
    {
        playerManagerScript = GetComponent<PlayerManager>();

        RefreshStats();
    }

    void RefreshStats()
    {
        ProfileText.text = "Place Holder Name" + "\n\n"
                        + playerManagerScript.CurrentHp + " / " + playerManagerScript.MaxHp + "\n\n"
                        + playerManagerScript.ActionPoints + "\n\n"
                        + playerManagerScript.Morality + "\n\n"
                        + playerManagerScript.Sanity + "\n\n";
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
