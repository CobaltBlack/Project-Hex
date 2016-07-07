using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Parent class of all Prompts
public abstract class Prompt
{ 
    abstract public string promptText { get; }
    abstract public List<PromptAnswer> promptAnswers { get; }
}
