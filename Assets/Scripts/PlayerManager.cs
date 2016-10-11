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
    ProfileManager ProfileManagerScript;
    EquipmentManager EquipmentManagerScript;

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

        ProfileManagerScript = GetComponent<ProfileManager>();
        EquipmentManagerScript = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();

        InitializePlayerData();
        RefreshPlayerStats();
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

    private int _maxHpBonus;
    private int _actionPointsBonus;

    private int _moralityFluxBonus;
    private int _sanityFluxBonus;

    private int _attackBonus;
    private int _critBonus;

    private int _defenseBonus;
    private int _dodgeBonus;

    // TODO: Load data from save file.
    void InitializePlayerData()
    {
        Debug.Log("InitializePlayerData");

        _currentHp = 100;
        _maxHp = 100;
        _morality = 50;
        _sanity = 50;
        _moralityFlux = -5;
        _sanityFlux = -5;
        _actionPoints = 100;

        _attack = 0;
        _crit = 0;
        _defense = 0;
        _dodge = 0;

        // TEST Load example skills
        //_skills = new List<SkillData>();
        //AddSkill(SkillDatabase.Instance.getSkillByName("Fireball1"));
        //AddSkill(SkillDatabase.Instance.getSkillByName("FanOfTomatoes"));
    }

    // Refreshes the player stats by recalculating it based on current items and effects
    public void RefreshPlayerStats()
    {
        // Get character base stats

        // Stat modifiers from equipment

        // Stat modifiers from effects

        // Multiplicative stat modifiers

        // Prevent stats under 0

        CalculateStats();

        ProfileManagerScript.RefreshPlayerStatsText();
    }

    private void CalculateStats()
    {
        // reset base stats
        _maxHp -= _maxHpBonus;
        _actionPoints -= _actionPointsBonus;
        _moralityFlux -= _moralityFluxBonus;
        _sanityFlux -= _sanityFluxBonus;
        _attack -= _attackBonus;
        _crit -= _critBonus;
        _defense -= _defenseBonus;
        _dodge -= _dodgeBonus;

        // reset bonus stats
        _maxHpBonus = 0;
        _actionPointsBonus = 0;
        _moralityFluxBonus = 0;
        _sanityFluxBonus = 0;
        _attackBonus = 0;
        _critBonus = 0;
        _defenseBonus = 0;
        _dodgeBonus = 0;

        for (int i = 0; i < EquipmentManagerScript.equipItems.Count; i++)
        {
            if (EquipmentManagerScript.equipItems[i].ID != -1)
            {
                Item.Equipment equipment = (Item.Equipment)EquipmentManagerScript.equipItems[i];
                _maxHpBonus += equipment.MaxHp;
                _actionPointsBonus += equipment.ActionPoints;
                _moralityFluxBonus += equipment.MoralityFlux;
                _sanityFluxBonus += equipment.SanityFlux;
                _attackBonus += equipment.Attack;
                _critBonus += equipment.Crit;
                _defenseBonus += equipment.Defense;
                _dodgeBonus += equipment.Dodge;
            }
        }

        // recalculate stats
        _maxHp += _maxHpBonus;
        _actionPoints += _actionPointsBonus;
        _moralityFlux += _moralityFluxBonus;
        _sanityFlux += _sanityFluxBonus;
        _attack += _attackBonus;
        _crit += _critBonus;
        _defense += _defenseBonus;
        _dodge += _dodgeBonus;
    }
}
