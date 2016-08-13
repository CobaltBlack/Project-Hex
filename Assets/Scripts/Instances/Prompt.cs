using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Parent class of all Prompts
public abstract class Prompt
{
    public abstract string getPromptText();
    public abstract List<PromptAnswer> getPromptAnswers();

    public string promptText { get { return getPromptText(); }  }
    public List<PromptAnswer> promptAnswers { get { return getPromptAnswers(); } }
}
