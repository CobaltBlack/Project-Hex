using UnityEngine;
using System.Collections;
public enum ActionType
{
    NONE,
    MOVE,
    SKILL,
    ITEM,
}

public class CombatAction {
    public int requiredAp;
    public ActionType actionType;
}
