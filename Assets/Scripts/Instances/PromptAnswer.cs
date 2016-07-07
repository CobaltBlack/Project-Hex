using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Parent class of all PromptAnswers
public abstract class PromptAnswer
{
    abstract public string answerText { get; }
    abstract public void answerAction();
    abstract public int requiredItemId { get; }

    public UnityAction answerUnityAction { get { return new UnityAction(answerAction); } }

    public void OpenPrompt(Prompt prompt)
    {
        ModalPanelInstance.Instance.DisplayPrompt(prompt);
    }

    public void ClosePrompt()
    {
        ModalPanelInstance.Instance.ClosePanel();
    }
}
