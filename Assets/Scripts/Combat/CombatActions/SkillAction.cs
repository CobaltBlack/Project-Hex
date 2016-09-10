using UnityEngine;
using System.Collections;

public enum SkillType
{
    Target,
    Instant,
};

[System.Serializable]
public class SkillAction : CombatAction
{
    public string Name;
    public string Description;
    public SkillType Type;
    public Sprite Icon; // Set in inspector

    //public string Name { get { return GetName(); } }
    //public string Description { get { return GetDescription(); } }
    //public SkillType Type { get { return GetSkillType(); } }
    //public Sprite Icon; // Set in inspector

    //protected abstract string GetName();
    //protected abstract string GetDescription();
    //protected abstract SkillType GetSkillType();
    //protected abstract int GetRequiredAp();

    public SkillAction()
    {
        //RequiredAp = GetRequiredAp();
    }
}
