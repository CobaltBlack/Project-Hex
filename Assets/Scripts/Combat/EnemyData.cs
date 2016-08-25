using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Human,
    Monster,
    Dragon,
    Demon,
};

public enum EnemyRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[System.Serializable]
public class EnemyData {
    public int Id;
    public string Name;
    public EnemyType Type;
    public EnemyRarity Rarity;
    public int MaxAp;

    public GameObject Prefab;
}
