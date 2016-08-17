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

public class CombatManager : MonoBehaviour
{
    // Allows combat manager to be called from anywhere
    public static CombatManager Instance = null;

    // TODO: Handle multiple friendly characters
    public GameObject PlayerInstance;
    public PlayerObject PlayerScript;

    CombatBoardManager BoardScript;
    HexOverlayManager OverlayScript;

    TurnState turnState = TurnState.Initializing; // Current state of the game    
    ActionType currentAction = ActionType.None; // Currently selected skill
    FriendlyObject currentCharacter; // The character the player is currently controlling (hero or companions)
    List<FriendlyObject> characterRunOrder; // The order in which the characters run their actions

    // ======================================
    // Public Functions
    // ======================================

    public void SetPlayerObject(GameObject player)
    {
        PlayerInstance = player;
        PlayerScript = player.GetComponent<PlayerObject>();
    }

    // Decides what happens when tile [x, y] is clicked based on current state
    public void HandleHexClick(int x, int y)
    {
        // Only proceed if it is player turn
        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }

        // Do something based on currentAction
        switch (currentAction)
        {
            case ActionType.Move:
                // TODO
                HandleMoveAction(x, y);
                break;
            case ActionType.Skill:
                // TODO
                break;
            case ActionType.Item:
                // TODO
                break;
            case ActionType.None:
            default:
                break;
        }
    }

    // Activates the skill clicked
    public void HandleSkillClick()
    {
        // Only proceed if it is player turn
        if (turnState != TurnState.PlayerTurn)
        {
            return;
        }
    }

    // ======================================
    // Private Functions
    // ======================================

    // Initialize 
    void Awake()
    {
        Debug.Log("CombatManager Awake");
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        BoardScript = GetComponent<CombatBoardManager>();
        OverlayScript = GetComponent<HexOverlayManager>();

        // Combat entrance animations

        // Set up board, objects, UI
        InitializeCombat();

        // Initialize UI elements (player skills, health, items)
        InitializeUI();

        // Start the game
        turnState = TurnState.StartPlayerTurn;
    }

    // Called on every frame
    // Use for listening to hotkeys?
    void Update()
    {
        // Listen for hotkeys

        // Determine available actions by current turn state
        switch (turnState)
        {
            case TurnState.StartPlayerTurn:
                // Enable UI buttons and controls to start player turn
                turnState = TurnState.PlayerTurn;
                StartPlayerTurn();
                break;

            case TurnState.PlayerTurn:
                // Right click first cancels the selected skill,
                // then dequeues the last action (if there are any)

                break;

            case TurnState.StartEnemyTurn:
                turnState = TurnState.EnemyTurn;
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
        // TODO: Configure combat parameters elsewhere before entering combat
        //boardScript.SetupBoard(GameManager.instance.combatParameters);
        CombatParameters combatParameters = new CombatParameters();
        BoardScript.SetupBoard(combatParameters);
    }

    // Leave the combat scene.
    void EndCombat()
    {

    }

    void InitializeUI()
    {
        Debug.Log("Initilaize UI");
    }

    // Enables UI buttons and controls for player turn
    void StartPlayerTurn()
    {
        Debug.Log("Start Player Turn");
        currentCharacter = PlayerScript;

        // Clear all queued actions
        PlayerScript.DequeueAllActions();

        // Enable skill buttons and controls

        // Display overlays around player for movement
        HexTile[] overlayTiles = BoardScript.GetTilesInRange(currentCharacter.PositionX, currentCharacter.PositionY, GetMoveRange(), false);
        OverlayScript.InstantiateOverlays(overlayTiles);

        // Clicking overlays cause the player to MOVE
        currentAction = ActionType.Move;
    }

    // Handles click on "End Turn" button
    // Disables UI buttons and controls
    // Begins processing queued actions
    void EndPlayerTurn()
    {
        // Cannot end turn if currently not the player's turn
        if (turnState != TurnState.PlayerTurn) { return; }

        Debug.Log("End player turn. Disabling controls");
        turnState = TurnState.PlayerTurnProcessing;

        // Disable UI and controls
        OverlayScript.RemoveAllOverlays();

        // Hide shadows for all friendly characters
        PlayerScript.HideShadow();

        Debug.Log("Processing actions");
        PlayerScript.ActionsComplete = false;
        ProcessNextCharacterActions();
    }

    // TODO: Process actions of player and companions based on chosen order
    public void ProcessNextCharacterActions()
    {
        // Perform player actions if they haven't been done yet
        if (!PlayerScript.ActionsComplete)
        {
            PlayerScript.RunCombatActions();
        }
        // Otherwise, the actions are done. The enemy turn can start
        else
        {
            turnState = TurnState.StartEnemyTurn;
        }
    }

    // Process the actions for all enemies
    void StartEnemyTurn()
    {
        Debug.Log("Starting Enemy turn");

        // Loop through each enemy

        // Skip if enemy is dead

        // Do enemy action based on its script

        // Back to player turn
        turnState = TurnState.StartPlayerTurn;
    }

    // Called when a tile is clicked, with the intent of moving
    void HandleMoveAction(int x, int y)
    {
        // Clear the overlays
        OverlayScript.RemoveAllOverlays();

        // Add MoveAction to the current character's action queue
        currentCharacter.QueueMoveAction(x, y);

        // Reinstantiate the hexoverlays at the position of the "shadow"
        HexTile[] overlayTiles = BoardScript.GetTilesInRange(currentCharacter.ShadowPositionX, currentCharacter.ShadowPositionY, GetMoveRange(), false);
        OverlayScript.InstantiateOverlays(overlayTiles);
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

    // Calculates the move range for the current character based on remaining AP, terrain
    int GetMoveRange()
    {
        return 2;
    }

    // ==============================
    // Test button for debug purposes
    // ==============================
    public void TestButton()
    {
        EndPlayerTurn();
    }
    // ==============================
}