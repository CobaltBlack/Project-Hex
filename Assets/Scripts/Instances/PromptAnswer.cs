using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Parent class of all PromptAnswers
public abstract class PromptAnswer
{
    public string answerText { get { return getAnswerText(); } }
    public UnityAction answerUnityAction { get { return new UnityAction(getAnswerAction); } }
    public int requiredItemId { get { return getRequiredItemId(); } }

    public void OpenPrompt(Prompt prompt)
    {
        ModalPanelInstance.Instance.DisplayPrompt(prompt);
    }

    public void ClosePrompt()
    {
        ModalPanelInstance.Instance.ClosePanel();
    }

    protected abstract string getAnswerText();
    protected abstract void getAnswerAction();
    protected abstract int getRequiredItemId();
}
