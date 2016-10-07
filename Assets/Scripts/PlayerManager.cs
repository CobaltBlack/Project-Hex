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

    // GENERAL
    public int CurrentHp { get { return _currentHp; } }
    public int MaxHp { get { return _maxHp; } }

    public int Morality { get { return _morality; } }
    public int Sanity { get { return _sanity; } }

    public int MoralityFlux { get { return _moralityFlux; } }
    public int SanityFlux { get { return _sanityFlux; } }

    public int ActionPoints { get { return _actionPoints; } }

    // OFFENSE
    public int Attack { get { return _attack; } }
    public int Crit { get { return _crit; } }

    // DEFENSE
    public int Defense { get { return _defense; } }
    public int Dodge { get { return _dodge; } }

    // RESISTENCE


    // Skills
    public List<SkillData> Skills { get { return _skills; } }

    // Items

    // Companions

    // ======================================
    // Public Functions
    // ======================================

    // temporary
    void Awake()
    {
        Debug.Log("player awake");

        InitializePlayerData();
    }

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
        if (CurrentHp <= 0)
        {
            _currentHp = 0;
            // Call GameOver()
        }
        else if (CurrentHp > MaxHp)
        {
            _currentHp = MaxHp;
        }
    }

    // Modifies the player's morality by modifyAmount
    public void ModifyMorality(int modifyAmount)
    {
        _morality += modifyAmount;
        if (_morality < 0)
        {
            _morality = 0;
            // Call GameOver()
        }
        else if (_morality > MaxMorality)
        {
            _morality = MaxMorality;
            // Call GameOver()
        }
    }

    // Modifies the player's sanity by modifyAmount
    public void ModifySanity(int modifyAmount)
    {
        _sanity += modifyAmount;
        if (_sanity < 0)
        {
            _sanity = 0;
            // Call GameOver()
        }
        else if (_sanity > MaxSanity)
        {
            _sanity = MaxSanity;
            // Call GameOver()
        }
    }

    public void ProcessFlux()
    {
        ModifyMorality(MoralityFlux);
        ModifySanity(SanityFlux);
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
    private int _moralityFlux;
    private int _sanityFlux;

    private int _actionPoints;

    private int _attack;
    private int _crit;

    private int _defense;
    private int _dodge;

    private List<SkillData> _skills;

    // TODO: Load data from save file.
    void InitializePlayerData()
    {
        _currentHp = 50;
        _maxHp = 50;
        _morality = 50;
        _sanity = 50;
        _moralityFlux = -1;
        _sanityFlux = -1;
        _actionPoints = 100;

        RefreshPlayerStats();

        // TEST Load example skills
        //_skills = new List<SkillData>();
        //AddSkill(SkillDatabase.Instance.getSkillByName("Fireball1"));
        //AddSkill(SkillDatabase.Instance.getSkillByName("FanOfTomatoes"));
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
