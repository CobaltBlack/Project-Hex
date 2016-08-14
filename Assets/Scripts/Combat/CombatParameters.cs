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

// CombatParameters are used to describe the terrain and enemies of the combat encounter
public class CombatParameters
{
    public TerrainType terrainType;
    public int terrainSize; // exact metrics to be determined
    public EnemyObject[] enemies;
    public float difficultyMultiplier;
}