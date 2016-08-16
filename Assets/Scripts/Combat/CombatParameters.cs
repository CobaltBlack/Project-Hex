using UnityEngine;
using System.Collections;

public enum TerrainType
{
    Castle,
    Cave,
    Grass,
};

public enum EnemyType
{
    Monsters,
    Thugs,
    Dragons,
    Demons,
};

// CombatParameters are used to describe the terrain and enemies of the combat encounter
public class CombatParameters
{
    public TerrainType TerrainType;
    public int TerrainSize; // exact metrics to be determined
    public EnemyObject[] Enemies;
    public float DifficultyMultiplier;
}