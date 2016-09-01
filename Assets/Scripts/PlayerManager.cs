using UnityEngine;
using System.Collections;

/*
 * PlayerManager
 * 
 * This script stores all information related to the player.
 * As a singleton, this can be accessed anywhere.
 * 
 */
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;
    public const int MaxMorality = 100;
    public const int MaxSanity = 100;

    // ======================================
    // Game related player stats
    // ======================================
    public GameObject PlayerCharacterPrefab;

    public int CurrentHp { get { return _CurrentHp; } }
    public int MaxHp { get { return _MaxHp; } }

    public int Morality { get { return _Morality; } }
    public int Sanity { get { return _Sanity; } }

    public int ActionPoints { get { return _ActionPoints; } }

    // Skills

    // Items

    // Companions

    // ======================================
    // Public Functions
    // ======================================

    // Load save file, initialize player data
    public void SetupPlayer()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InitializePlayerData();
    }

    // Modifies player's currentHp by modifyAmount (can be negative to represent damage)
    // currentHp cannot exceed maxHp
    // currentHp cannot be under 0
    // If the player's health is 0, the player dies and the game is over
    public void ModifyHp(int modifyAmount)
    {
        _CurrentHp += modifyAmount;
        if (CurrentHp > MaxHp)
        {
            _CurrentHp = MaxHp;
        }
        else if (CurrentHp <= 0)
        {
            // Call GameOver()
        }
    }

    // Modifies the player's morality by modifyAmount
    public void ModifyMorality(int modifyAmount)
    {
        _Morality += modifyAmount;
        if (_Morality < 0)
        {
            _Morality = 0;
        }
        else if (_Morality > MaxMorality)
        {
            _Morality = MaxMorality;
        }
    }

    // Modifies the player's sanity by modifyAmount
    public void ModifySanity(int modifyAmount)
    {
        _Sanity += modifyAmount;
        if (_Sanity < 0)
        {
            _Sanity = 0;
        }
        else if (_Sanity > MaxSanity)
        {
            _Sanity = MaxSanity;
        }
    }

    // ======================================
    // Private Variables and Functions
    // ======================================
    private int _CurrentHp;
    private int _MaxHp;

    private int _Morality;
    private int _Sanity;

    private int _ActionPoints;

    // TODO: Load data from save file.
    void InitializePlayerData()
    {
        _CurrentHp = 50;
        _MaxHp = 50;
        _Morality = 50;
        _Sanity = 50;
        _ActionPoints = 100;
        RefreshPlayerStats();
    }

    // Refreshes the player stats by recalculating it based on current items and effects
    void RefreshPlayerStats()
    {
        // Get character base stats

        // Stat modifiers from equipment

        // Stat modifiers from effects

        // Multiplicative stat modifiers

        // Prevent stats under 0
    }
}
