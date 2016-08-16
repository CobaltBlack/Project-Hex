using UnityEngine;
using System.Collections;

public enum InstanceType
{
    Battle,
    Quest,
    Dialogue,
    Boss,
    Other,
};

// Parent class of all Instances
public abstract class Instance
{
    public string Name { get { return GetInstanceName(); } }
    public Prompt InitialPrompt { get { return GetInitialPrompt(); } }
    public InstanceType Type { get { return GetInstanceType(); } }

    protected abstract string GetInstanceName();
    protected abstract Prompt GetInitialPrompt();
    protected abstract InstanceType GetInstanceType();
}
