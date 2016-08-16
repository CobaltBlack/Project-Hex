using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

enum TurnState
{
    INIT, // Setting up the gameboard
    START_PLAYER_TURN, // Enable player controls, buttons, etc
    PLAYER_TURN, // Player can input actions
    PROCESS_PLAYER_TURN, // Processing/animating player actions
    START_ENEMY_TURN, // Start enemy turn
    ENEMY_TURN, // Running enemy AI scripts to determine their actions
    PROCESS_ENEMY_TURN, // Processing/animating enemy actions
}

public class CombatManager : MonoBehaviour
{
    // Allows combat manager to be called from anywhere
    public static CombatManager instance = null;

    CombatBoardManager boardScript;
    HexOverlayManager overlayScript;
    
    // TODO: Handle multiple friendly characters
    public GameObject playerInstance;
    public PlayerObject playerScript;

    public int playerInitialX;
    public int playerInitialY;

    TurnState turnState = TurnState.INIT; // Current state of the game    
    ActionType currentAction = ActionType.NONE; // Currently selected skill
    FriendlyObject currentCharacter; // The character the player is currently controlling (hero or companions)
    List<FriendlyObject> characterRunOrder; // The order in which the characters run their actions

    // ======================================
    // Public Functions
    // ======================================

    // Decides what happens when tile [x, y] is clicked based on current state
    public void handleHexClick(int x, int y)
    {
        // Only proceed if it is player turn
        if (turnState != TurnState.PLAYER_TURN)
        {
            return;
        }

        // Do something based on currentAction
        switch (currentAction)
        {
            case ActionType.MOVE:
                // TODO
                HandleMoveAction(x, y);
                break;
            case ActionType.SKILL:
                // TODO
                break;
            case ActionType.ITEM:
                // TODO
                break;
            case ActionType.NONE:
            default:
                break;
        }
    }

    // Activates the skill clicked
    public void handleSkillClick()
    {
        // Only proceed if it is player turn
        if (turnState != TurnState.PLAYER_TURN)
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        boardScript = GetComponent<CombatBoardManager>();
        overlayScript = GetComponent<HexOverlayManager>();

        // Combat entrance animations

        // Set up board, objects, UI
        InitializeCombat();

        // Initialize UI elements (player skills, health, items)
        InitializeUI();

        // Start the game
        turnState = TurnState.START_PLAYER_TURN;
    }

    // Called on every frame
    // Use for listening to hotkeys?
    void Update()
    {
        // Listen for hotkeys

        // Determine available actions by current turn state
        switch (turnState)
        {
            case TurnState.START_PLAYER_TURN:
                // Enable UI buttons and controls to start player turn
                turnState = TurnState.PLAYER_TURN;
                StartPlayerTurn();
                break;

            case TurnState.PLAYER_TURN:
                // Right click first cancels the selected skill,
                // then dequeues the last action (if there are any)

                break;

            case TurnState.START_ENEMY_TURN:
                turnState = TurnState.ENEMY_TURN;
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
        boardScript.SetupBoard(combatParameters);
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
        currentCharacter = playerScript;

        // Clear all queued actions
        playerScript.dequeueAllActions();

        // Enable skill buttons and controls

        // Display overlays around player for movement
        HexTile[] overlayTiles = boardScript.GetSurroundingTiles(currentCharacter.positionX, currentCharacter.positionY);
        overlayScript.InstantiateOverlays(overlayTiles);

        // Clicking overlays cause the player to MOVE
        currentAction = ActionType.MOVE;
    }

    // Handles click on "End Turn" button
    // Disables UI buttons and controls
    void EndPlayerTurn()
    {
        // Cannot end turn if currently not the player's turn
        if (turnState != TurnState.PLAYER_TURN) { return; }

        Debug.Log("End player turn. Disabling controls");
        turnState = TurnState.PROCESS_PLAYER_TURN;

        // Disable UI and controls
        overlayScript.RemoveAllOverlays();

        // Hide shadows for all friendly characters
        playerScript.hideShadow();

        Debug.Log("Processing actions");
        playerScript.actionsComplete = false;
        ProcessNextCharacterActions();
    }

    // TODO: Process actions of player and companions based on chosen order
    public void ProcessNextCharacterActions()
    {
        // Perform player actions if they haven't been done yet
        if (!playerScript.actionsComplete)
        {
            playerScript.runCombatActions();
        }
        // Otherwise, the actions are done. The enemy turn can start
        else
        {
            turnState = TurnState.START_ENEMY_TURN;
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
        turnState = TurnState.START_PLAYER_TURN;
    }

    // Called when a tile is clicked, with the intent of moving
    void HandleMoveAction(int x, int y)
    {
        // Clear the overlays
        overlayScript.RemoveAllOverlays();

        // Add MoveAction to the current character's action queue
        currentCharacter.queueMoveAction(x, y);

        // Reinstantiate the hexoverlays at the position of the "shadow"
        HexTile[] overlayTiles = boardScript.GetSurroundingTiles(currentCharacter.shadowPositionX, currentCharacter.shadowPositionY);
        overlayScript.InstantiateOverlays(overlayTiles);
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

    // ==============================
    // Test button for debug purposes
    // ==============================
    public void TestButton()
    {
        EndPlayerTurn();
    }
    // ==============================
}