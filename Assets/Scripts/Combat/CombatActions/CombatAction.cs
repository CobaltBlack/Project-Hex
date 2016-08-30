using UnityEngine;
using System.Collections;
public enum ActionType
{
    None,
    Move,
    Skill,
    Item,
}

public class CombatAction {
    public int RequiredAp;
    public ActionType ActionType;
}
