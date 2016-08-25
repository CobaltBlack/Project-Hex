using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TerrainType
{
    Castle,
    Cave,
    Grass,
};

// CombatParameters are used to describe the terrain and enemies of the combat encounter
public class CombatParameters
{
    public TerrainType TerrainType;
    public int TerrainSize; // exact metrics to be determined
    public List<EnemyData> Enemies = new List<EnemyData>();
    public float DifficultyMultiplier = 1f;
}
