using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InstanceType
{
    Battle,
    Quest,
    Dialogue,
    Boss,
    Other,
};

// Parent class of all Instances
public abstract class Instance : MonoBehaviour
{
    public string Name { get { return GetInstanceName(); } }
    public string Description { get { return GetInstanceDescription(); } }
    public Prompt InitialPrompt { get { return GetInitialPrompt(); } }
    public InstanceType Type { get { return GetInstanceType(); } }

    public List<Tileset> InstanceTileset { get { return GetInstanceTileset(); } }
    public List<Layout> InstanceLayout { get { return GetInstanceLayout(); } }

    protected abstract string GetInstanceName();
    protected abstract string GetInstanceDescription();
    protected abstract Prompt GetInitialPrompt();
    protected abstract InstanceType GetInstanceType();

    protected abstract List<Tileset> GetInstanceTileset();
    protected abstract List<Layout> GetInstanceLayout();
}
