using UnityEngine;
using System.Collections;

public enum InstanceType
{
    BATTLE,
    QUEST,
    DIALOGUE,
    OTHER,
};

// Parent class of all Instances
public abstract class Instance
{
    abstract public string instanceName { get; }
    abstract public Prompt instancePrompt { get; }
    abstract public InstanceType instanceType { get; }
}
