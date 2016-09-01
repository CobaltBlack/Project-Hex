using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour {

    public Text ApText;

    // TODO: Load skills and what not? Maybe skills should be loaded by CombatManager instead
    public void SetupUI()
    {

    }

    // Updates the AP text display 
	public void UpdateApDisplay(int currentAp, int maxAp)
    {
        ApText.text = "AP: " + currentAp.ToString() + " / " + maxAp.ToString();
    }
}
