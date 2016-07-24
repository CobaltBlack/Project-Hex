using UnityEngine;
using System.Collections;

public enum TerrainType
{
    CASTLE,
    CAVE,
    GRASS,
};

public enum EnemyType
{
    MONSTERS,
    THUGS,
    DRAGONS,
    DEMONS,
};

public class CombatParameters
{
    public TerrainType terrainType;
    public int terrainSize; // exact metrics to be determined
    public Enemy[] enemies;
    public float difficultyMultiplier;
}