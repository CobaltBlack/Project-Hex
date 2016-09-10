using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/* CombatUIManager
 * 
 * This scripts manages the UI in the combat scene.
 * 
 * This includes, but not limited to:
 * - Skill bars
 * - Health bars
 * - Action queues
 * - Menu buttons
 * - Info popups
 */
public class CombatUIManager : MonoBehaviour
{
    public Text ApText;
    public List<Button> SkillButtons;

    // TODO: Load skills and what not?
    public void SetupUI(FriendlyObject currentCharacter)
    {
        // Setup the skill bar
        UpdateSkillsDisplay(currentCharacter);
    }

    // Updates the AP text display 
    public void UpdateApDisplay(int currentAp, int maxAp)
    {
        ApText.text = "AP: " + currentAp.ToString() + " / " + maxAp.ToString();
    }

    // Update the skill display bar based on the currently selected character
    public void UpdateSkillsDisplay(FriendlyObject currentCharacter)
    {
        var charSkills = currentCharacter.Skills;
        for (var i = 0; i < SkillButtons.Count; i++)
        {
            var skillButton = SkillButtons[i];

            if (i < charSkills.Count)
            {
                // Replace with new skill icon
                var charSkill = charSkills[i];
                skillButton.GetComponent<Image>().sprite = charSkill.Icon;

                // TODO: Display cooldown overlay
            }
            else
            {
                // Disable the button if character has less than max number of skills
                skillButton.interactable = false;

                // Clear current icon
                skillButton.GetComponent<Image>().sprite = null;
            }
        }
    }

    // Disables all skill buttons on the skill bar. 
    // Use UpdateSkillsDisplay() to re-enable them.
    public void DisableSkillButtons()
    {
        foreach(var button in SkillButtons)
        {
            button.interactable = false;
        }
    }
}
