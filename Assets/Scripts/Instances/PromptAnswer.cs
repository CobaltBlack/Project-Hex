using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Parent class of all PromptAnswers
public abstract class PromptAnswer
{
    public string Text { get { return GetAnswerText(); } }
    public UnityAction UnityAction { get { return new UnityAction(GetAnswerAction); } }

    // All Answers must have a text and action
    protected abstract string GetAnswerText();
    protected abstract void GetAnswerAction();

    // This answer is available only if the player has at least 1 of the following requirements
    public int RequiredItemId = -1;
    public string RequiredSkill = null;
    public int RequiredSanity = -1;
    public int RequiredMorality = -1;

    public void OpenPrompt(Prompt prompt)
    {
        ModalPanelInstance.Instance.DisplayPrompt(prompt);
    }

    public void ClosePrompt()
    {
        ModalPanelInstance.Instance.ClosePanel();

        // start second part of Move Player when the panel closes
        MapGameManager.instance.MovePlayerPartB();
    }
}
