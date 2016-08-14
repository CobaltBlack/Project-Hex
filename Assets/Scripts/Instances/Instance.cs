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
    public string instanceName { get { return getInstanceName(); } }
    public Prompt instancePrompt { get { return getInstancePrompt(); } }
    public InstanceType instanceType { get { return getInstanceType(); } }

    protected abstract string getInstanceName();
    protected abstract Prompt getInstancePrompt();
    protected abstract InstanceType getInstanceType();
}
