using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

enum TurnState
{
    Initializing, // Setting up the gameboard
    StartPlayerTurn, // Enable player controls, buttons, etc
    PlayerTurn, // Player can input actions
    PlayerTurnProcessing, // Processing/animating player actions
    StartEnemyTurn, // Start enemy turn
    EnemyTurn, // Running enemy AI scripts to determine their actions
    EnemyTurnProcessing, // Processing/animating enemy actions
}

/* CombatManager
 * 
 * This scripts manages the top level flow of combat.
 * 
 * It is in charge of determining the turn state and handling player input.
 */
public class CombatManager : MonoBehaviour
{
    // Allows combat manager to be called from anywhere
    public static CombatManager Instance = null;

    // ======================================
    // Public Functions
    // ======================================

    // TODO: Handle multiple friendly characters
    public GameObject PlayerInstance;
    public PlayerObject PlayerScript;

    public void SetPlayerObject(GameObject player)
    {
        PlayerInstance = player;
        PlayerScript = player.GetComponent<PlayerObject>();
    }

    public void AddEnemyObject(GameObject enemy)
    {
        _enemies.Add(enemy.GetComponent<EnemyObject>());
    }

    // Decides what happens when tile [x, y] is clicked based on current state
    public void HandleHexClick(int x, int y)
    {
        // Only proceed if it is player turn
        if (_turnState != TurnState.PlayerTurn)
        {
            return;
        }

        // Do something based on currentAction
        switch (_currentAction)
        {
            case ActionType.Move:
                HandleMoveAction(x, y);
                break;
            case ActionType.Skill:
                // TODO Handle ActionType.Skill
                break;
            case ActionType.Item:
                // TODO Handle ActionType.Item
                break;
            case ActionType.None:
            default:
                break;
        }

        UpdateUI();
    }

    // Activates the skill clicked
    public void HandleSkillClick(int skillIndex)
    {
        // Only proceed if it is player turn, and skillIndex is valid
        if (_turnState != TurnState.PlayerTurn) return;
        if ( 0 > skillIndex || skillIndex >= _currentCharacter.Skills.Count) return;

        // TODO: Queue a skill action instead of activating it
        var clickedSkill = _currentCharacter.Skills[skillIndex];
        clickedSkill.SkillEffect();
    }

    // Shows the movement overlays for the currently selected character
    // Takes into account of current remaining AP and shadow
    public void ShowMovementOverlays()
    {
        int currentX, currentY;
        if (_currentCharacter.IsShadowActive)
        {
            currentX = _currentCharacter.ShadowX;
            currentY = _currentCharacter.ShadowY;
        }
        else
        {
            currentX = _currentCharacter.X;
            currentY = _currentCharacter.Y;
        }

        var overlayTiles = BoardManager.GetTraversableTiles(currentX, currentY, _currentCharacter.MoveRange);

        // TODO: Discard invalid tiles
        //foreach (var tile in overlayTiles)
        //{

        //}

        OverlayManager.InstantiateOverlays(overlayTiles);
    }

    // ======================================
    // Private Functions
    // ======================================

    CombatBoardManager BoardManager;
    HexOverlayManager OverlayManager;
    CombatUIManager UIManager;

    TurnState _turnState = TurnState.Initializing; // Current state of the game    
    ActionType _currentAction = ActionType.None; // Currently selected skill
    FriendlyObject _currentCharacter; // The character the player is currently controlling (hero or companions)
    List<FriendlyObject> _characterRunOrder; // The order in which the characters run their actions

    List<EnemyObject> _enemies = new List<EnemyObject>();
    int _currentEnemyIndex = 0; // Used to track which enemy's actions to perform next

    // Initialize 
    void Awake()
    {
        Debug.Log("CombatManager Awake");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        BoardManager = GetComponent<CombatBoardManager>();
        OverlayManager = GetComponent<HexOverlayManager>();
        UIManager = GetComponent<CombatUIManager>();

        // Combat entrance animations

        // Set up board, objects, UI
        InitializeCombat();

        // Initialize UI elements (player skills, health, items)
        InitializeUI();

        // Start the game
        _turnState = TurnState.StartPlayerTurn;
    }

    // Called on every frame
    // Use for listening to hotkeys?
    void Update()
    {
        // Listen for hotkeys

        // Determine available actions by current turn state
        switch (_turnState)
        {
            case TurnState.StartPlayerTurn:
                // Enable UI buttons and controls to start player turn
                _turnState = TurnState.PlayerTurn;
                StartPlayerTurn();
                break;

            case TurnState.PlayerTurn:
                // Right click first cancels the selected skill,
                // then dequeues the last action (if there are any)

                break;

            case TurnState.StartEnemyTurn:
                _turnState = TurnState.EnemyTurn;
                StartEnemyTurn();
                break;

            default:
                break;
        }
    }

    // Sets up the map, player, enemies, and UI in the combat screen
    void InitializeCombat()
    {
        // Initialize combat map and enemies
        BoardManager.SetupBoard(GameManager.Instance.CombatParameters);

        _currentCharacter = PlayerScript;
    }

    // Leave the combat scene.
    void EndCombat()
    {
    }

    void InitializeUI()
    {
        Debug.Log("Initialize UI");
        UIManager.SetupUI(_currentCharacter);
    }

    // Enables UI buttons and controls for player turn
    void StartPlayerTurn()
    {
        Debug.Log("Start Player Turn");
        _currentCharacter = PlayerScript;

        // Clear all queued actions and reset AP
        PlayerScript.DequeueAllActions();

        // Enable skill buttons and controls

        // Display overlays around player for movement
        ShowMovementOverlays();

        // Clicking overlays would cause the player to MOVE
        _currentAction = ActionType.Move;

        // Update UI
        UpdateUI();
    }

    // Handles click on "End Turn" button
    // Disables UI buttons and controls
    // Begins processing queued actions
    void EndPlayerTurn()
    {
        // Cannot end turn if currently not the player's turn
        if (_turnState != TurnState.PlayerTurn) { return; }

        Debug.Log("End player turn. Disabling controls");
        _turnState = TurnState.PlayerTurnProcessing;

        // Disable UI and controls
        OverlayManager.RemoveAllOverlays();

        // Hide shadows for all friendly characters
        PlayerScript.HideShadow();

        Debug.Log("Processing actions");
        PlayerScript.ActionsComplete = false;
        ProcessNextCharacterActions();
    }

    // TODO: Process actions of player and companions based on chosen order
    public void ProcessNextCharacterActions()
    {
        if (_turnState == TurnState.PlayerTurnProcessing)
        {
            // Perform player actions if they haven't been done yet
            if (!PlayerScript.ActionsComplete)
            {
                PlayerScript.RunCombatActions();
            }
            // Otherwise, the actions are done. The enemy turn can start
            else
            {
                _turnState = TurnState.StartEnemyTurn;
            }
        }
        else if (_turnState == TurnState.EnemyTurnProcessing)
        {
            if (_currentEnemyIndex < _enemies.Count)
            {
                _currentEnemyIndex++;
                _enemies[_currentEnemyIndex - 1].RunCombatActions();
            }
            else
            {
                // Back to player turn
                _turnState = TurnState.StartPlayerTurn;
            }
        }
    }

    // Process the actions for all enemies
    void StartEnemyTurn()
    {
        Debug.Log("Starting Enemy turn");

        // Determine actions for each enemy this turn
        foreach (var enemy in _enemies)
        {
            // Skip if enemy is dead

            // Queue enemy actions based on its script
            enemy.DequeueAllActions();
            enemy.QueueTurnActions();
        }

        // Process the enemy actions
        Debug.Log("Processing Enemy actions");
        _turnState = TurnState.EnemyTurnProcessing;
        _currentEnemyIndex = 0;
        ProcessNextCharacterActions();
    }

    // Called when a tile is clicked, with the intent of moving
    void HandleMoveAction(int x, int y)
    {
        // Clear the overlays
        // The overlays reappear once the shadow's movement is complete
        OverlayManager.RemoveAllOverlays();

        // Add MoveAction to the current character's action queue
        _currentCharacter.QueueMoveAction(x, y);

        // TODO: Do stuff with UI
    }

    // Called when a skill is selected
    void HandleUseSkillAction()
    {
        // If skill does not require a target, queue it right away

        // Otherwise:

        // Remove movement overlays

        // Display overlays for selected skill

        // Change mouse cursor (change it back afterwards)

    }

    // Refreshes all the UI elements based on current game state
    void UpdateUI()
    {
        // AP Display
        UIManager.UpdateApDisplay(_currentCharacter.CurrentAp, _currentCharacter.MaxAp);
    }

    // ==============================
    // Test button for debug purposes
    // ==============================
    public void TestButton()
    {
        EndPlayerTurn();
    }
}
