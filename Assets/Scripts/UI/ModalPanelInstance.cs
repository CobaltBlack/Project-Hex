using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/*
 * ModalPanelInstance
 * 
 * This script controls the main Instance modal panel that lets the player
 * make a choice by pressing on a button
 * 
 */

public class ModalPanelInstance : MonoBehaviour
{
    public Text question;

    // no images in current iteration
    //public Image iconImage; 
    public GameObject modalPanelObject; // assign "Modal Panel - Instance" in inspector. Also, remember to tick off "Modal Panel - Instance" before running game.
    public List<Button> answerButtons;

    private static ModalPanelInstance modalPanel;
    public static ModalPanelInstance Instance
    {
        get
        {
            if (!modalPanel)
            {
                modalPanel = FindObjectOfType(typeof(ModalPanelInstance)) as ModalPanelInstance;
                if (!modalPanel)
                {
                    Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
                }
            }
            return modalPanel;
        }
    }

    public void DisplayPrompt(Prompt prompt)
    {
        Debug.Log(prompt.Text);

        // Turn all buttons off
        TurnButtonsOff();

        // Fill buttons with data from prompt
        this.question.text = prompt.Text;
        for (int i = 0; i < prompt.Answers.Count; i++)
        {
            PromptAnswer currAnswer = prompt.Answers[i];
            Button currButton = answerButtons[i];

            currButton.GetComponentInChildren<Text>().text = currAnswer.Text;
            currButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
            currButton.onClick.AddListener(currAnswer.UnityAction);
        }

        // Set buttons active
        for (int i = 0; i < prompt.Answers.Count; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
        }

        modalPanelObject.SetActive(true); // turn modal panel ON
    }

    public void TurnButtonsOff()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }
}
