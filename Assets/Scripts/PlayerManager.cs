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
    public static PlayerManager instance = null;
    public const int MAX_MORALITY = 100;
    public const int MAX_SANITY = 100;

    // ======================================
    // Game related player stats
    // ======================================
    public GameObject playerCharacter;

    public int currentHp { get { return _currentHp; } }
    public int maxHp { get { return _maxHp; } }

    public int morality { get { return _morality; } }
    public int sanity { get { return _sanity; } }

    public int actionPoints;

    // Skills

    // Items
    
    // Companions

    // ======================================
    // Public Functions
    // ======================================

    // Load save file, initialize player data
    public void SetupPlayer()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
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
        _currentHp += modifyAmount;
        if (currentHp > maxHp)
        {
            _currentHp = maxHp;
        }
        else if (currentHp <= 0)
        {
            // Call GameOver()
        }
    }

    // Modifies the player's morality by modifyAmount
    public void ModifyMorality(int modifyAmount)
    {
        _morality += modifyAmount;
        if (_morality < 0)
        {
            _morality = 0;
        }
        else if (_morality > MAX_MORALITY)
        {
            _morality = MAX_MORALITY;
        }
    }

    // Modifies the player's sanity by modifyAmount
    public void ModifySanity(int modifyAmount)
    {
        _sanity += modifyAmount;
        if (_sanity < 0)
        {
            _sanity = 0;
        }
        else if (_sanity > MAX_SANITY)
        {
            _sanity = MAX_SANITY;
        }
    }

    // ======================================
    // Private Variables and Functions
    // ======================================
    private int _currentHp;
    private int _maxHp;

    private int _morality;
    private int _sanity;

    // TODO:
    // Load data from save file.
    void InitializePlayerData()
    {
        _currentHp = 50;
        _maxHp = 50;
        _morality = 50;
        _sanity = 50;
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
