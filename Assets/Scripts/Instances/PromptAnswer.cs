using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Parent class of all PromptAnswers
public abstract class PromptAnswer
{
    public string Text { get { return GetAnswerText(); } }
    public UnityAction UnityAction { get { return new UnityAction(GetAnswerAction); } }
    public int RequiredItemId { get { return GetRequiredItemId(); } }

    public void OpenPrompt(Prompt prompt)
    {
        ModalPanelInstance.Instance.DisplayPrompt(prompt);
    }

    public void ClosePrompt()
    {
        ModalPanelInstance.Instance.ClosePanel();
    }

    protected abstract string GetAnswerText();
    protected abstract void GetAnswerAction();
    protected abstract int GetRequiredItemId();
}
