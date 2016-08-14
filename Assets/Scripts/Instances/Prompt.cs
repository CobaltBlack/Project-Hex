using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Parent class of all Prompts
public abstract class Prompt
{
    public string promptText { get { return getPromptText(); } }
    public List<PromptAnswer> promptAnswers { get { return getPromptAnswers(); } }

    protected abstract string getPromptText();
    protected abstract List<PromptAnswer> getPromptAnswers();
}
