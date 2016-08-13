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
    public abstract string getInstanceName();
    public abstract Prompt getInstancePrompt();
    public abstract InstanceType getInstanceType();

    public string instanceName { get { return getInstanceName(); } }
    public Prompt instancePrompt { get { return getInstancePrompt(); } }
    public InstanceType instanceType { get { return getInstanceType(); } }
}
