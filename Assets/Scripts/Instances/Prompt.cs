using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Parent class of all Prompts
public abstract class Prompt
{
    public string Text { get { return GetPromptText(); } }
    public List<PromptAnswer> Answers { get { return GetPromptAnswers(); } }

    protected abstract string GetPromptText();
    protected abstract List<PromptAnswer> GetPromptAnswers();
}
