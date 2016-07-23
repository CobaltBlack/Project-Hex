using UnityEngine;
using System.Collections;

public enum TerrainType
{
    CASTLE,
    CAVE,
    GRASS,
};

public class CombatParameters
{
    public TerrainType terrainType;
}

public class CombatManager : MonoBehaviour
{

    void Awake()
    {
        // Get combat parameters

        // Initialize combat map and enemies

        // Initialize UI elements (player skills, health, items)

    }
}