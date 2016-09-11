using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* PlayerManager
 * 
 * This script stores all information related to the player.
 * As a singleton, this can be accessed anywhere.
 */
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;

    // ======================================
    // Inspector variables
    // ======================================
    public int MaxMorality;
    public int MaxSanity;
    public int MaxNumOfSkills;
    public GameObject PlayerCharacterPrefab;

    // ======================================
    // Game related player stats
    // ======================================
    public int CurrentHp { get { return _currentHp; } }
    public int MaxHp { get { return _maxHp; } }

    public int Morality { get { return _morality; } }
    public int Sanity { get { return _sanity; } }

    public int ActionPoints { get { return _actionPoints; } }

    // Skills
    public List<SkillData> Skills { get { return _skills; } }

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
        _currentHp += modifyAmount;
        if (CurrentHp > MaxHp)
        {
            _currentHp = MaxHp;
        }
        else if (CurrentHp <= 0)
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
        else if (_morality > MaxMorality)
        {
            _morality = MaxMorality;
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
        else if (_sanity > MaxSanity)
        {
            _sanity = MaxSanity;
        }
    }

    // Add a skill to the player's learned skills
    // TODO: Handle replacing/removing skills
    public void AddSkill(SkillData skill)
    {
        _skills.Add(skill);
    }

    // ======================================
    // Private Variables and Functions
    // ======================================
    private int _currentHp;
    private int _maxHp;

    private int _morality;
    private int _sanity;

    private int _actionPoints;
    private List<SkillData> _skills;

    // TODO: Load data from save file.
    void InitializePlayerData()
    {
        _currentHp = 50;
        _maxHp = 50;
        _morality = 50;
        _sanity = 50;
        _actionPoints = 100;

        RefreshPlayerStats();

        // TEST Load example skills
        _skills = new List<SkillData>();
        AddSkill(SkillDatabase.Instance.getSkillByName("Fireball1"));
        AddSkill(SkillDatabase.Instance.getSkillByName("HolyNova"));
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
