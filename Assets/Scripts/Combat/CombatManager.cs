using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum TurnState
{
    INIT, // Setting up the gameboard
    NEW_TURN, // Ready to start new turn for player or enemies
    WAIT_FOR_ACTION, // Waiting for player to input an action
    PROCESS_ACTION, // Processing/animating player action
    ENEMY_TURN, // Processing/animating enemy action
}

enum PlayerAction
{
    NONE,
    MOVE,
    USE_SKILL,
    USE_CONSUMABLE,
}

public class CombatManager : MonoBehaviour
{
    // Allows combat manager to be called from anywhere
    public static CombatManager instance = null;

    CombatBoardManager boardScript;
    PlayerManager playerScript;
    HexOverlayManager overlayScript;

    public int playerInitialX;
    public int playerInitialY;

    // An item in the turn queue to determine whose turn it is next
    class TurnQueueItem
    {
        public int moveTime;
        public string ownerOfTurn; // could be player, or name of specific enemy
        public TurnQueueItem(int moveTime, string ownerOfTurn)
        {
            this.moveTime = moveTime;
            this.ownerOfTurn = ownerOfTurn;
        }
    }
    List<TurnQueueItem> turnQueue = new List<TurnQueueItem>();
    int currentTurnTime = 0;

    TurnState turnState = TurnState.INIT;
    PlayerAction currentAction = PlayerAction.NONE;

    // ======================================
    // Public Functions
    // ======================================

    // Decides what happens when tile [x, y] is clicked based on current state
    public void handleHexClick(int x, int y)
    {
        // Only proceed if game is waiting for player action
        if (turnState != TurnState.WAIT_FOR_ACTION)
        {
            return;
        }

        // Do something based on currentAction
        switch (currentAction)
        {
            case PlayerAction.MOVE:
                // TODO
                HandleMoveAction(x, y);
                break;
            case PlayerAction.USE_SKILL:
                // TODO
                break;
            case PlayerAction.USE_CONSUMABLE:
                // TODO
                break;
            case PlayerAction.NONE:
            default:
                break;
        }
    }

    // ======================================
    // Private Functions
    // ======================================

    // Initialize 
    void Awake()
    {
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
        playerScript = GetComponent<PlayerManager>();
        overlayScript = GetComponent<HexOverlayManager>();

        // Combat entrance animations

        // Set up board, objects, UI
        InitializeCombat();

        // Start the game
        turnState = TurnState.NEW_TURN;
    }

    // Called on every frame
    void Update()
    {
        // Do things based on current combat state
        if (turnState == TurnState.WAIT_FOR_ACTION
            || turnState == TurnState.PROCESS_ACTION
            || turnState == TurnState.ENEMY_TURN
            || turnState == TurnState.INIT)
        {
            // Do nothing
            return;
        }
        else if (turnState == TurnState.NEW_TURN)
        {
            StartNextTurn();
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

        playerScript.InstantiatePlayer(playerInitialX, playerInitialY);

        // Initialize UI elements (player skills, health, items)

        // Add player to turn queue
        Debug.Log("add player to queue");
        AddToTurnQueue(playerScript.turnInterval, "player");
    }

    // Process turn for next character (player or enemy)
    void StartNextTurn()
    {
        Debug.Log("StartNextTurn");
        // Get id of character whose turn is next
        string nextTurnOwner = DequeueTurnQueue();
        if (nextTurnOwner.Equals("player"))
        {
            turnState = TurnState.WAIT_FOR_ACTION;
            StartPlayerTurn();
        }
        else
        {
            turnState = TurnState.ENEMY_TURN;
            StartEnemyTurn(nextTurnOwner);
        }
    }

    // 
    void StartPlayerTurn()
    {
        Debug.Log("StartPlayerTurn");

        // Enable skill buttons

        // Display overlays around player for movement
        HexTile[] overlayTiles = boardScript.GetSurroundingTiles(playerScript.positionX, playerScript.positionY);
        overlayScript.InstantiateOverlays(overlayTiles);

        // Clicking overlays cause the player to MOVE
        currentAction = PlayerAction.MOVE;

        AddToTurnQueue(playerScript.turnInterval, "player");
    }

    void EndPlayerTurn()
    {
        overlayScript.RemoveAllOverlays();
        turnState = TurnState.NEW_TURN;
    }

    void StartEnemyTurn(string enemyName)
    {
        // Exit if enemy is dead

        // Find enemy by name

        // Do enemy action based on its script

        // Add enemy back into the turn queue
        int enemyTurnInterval = 50;
        AddToTurnQueue(enemyTurnInterval, enemyName);
    }

    // Inserts an item to the turn queue based on turnInterval 
    void AddToTurnQueue(int turnInterval, string ownerString)
    {
        int nextTurnTime = currentTurnTime + turnInterval;
        if (turnQueue.Count == 0)
        {
            turnQueue.Add(new TurnQueueItem(nextTurnTime, ownerString));
        }
        else
        {
            int i = 0;
            while (i < turnQueue.Count && turnQueue[i].moveTime > nextTurnTime)
            {
                i++;
            }
            turnQueue.Insert(i, new TurnQueueItem(nextTurnTime, ownerString));
        }
    }

    // Returns owner of next turn and removes it from the queue
    // Updates current time to next time
    string DequeueTurnQueue()
    {
        TurnQueueItem item = turnQueue[turnQueue.Count - 1];
        currentTurnTime = item.moveTime;
        string owner = item.ownerOfTurn;
        turnQueue.RemoveAt(turnQueue.Count - 1);
        return owner;
    }

    // Called when a skill is selected
    void HandleUseSkillAction()
    {
        // If skill is not targeted, execute it right away

        // Otherwise:

        // Remove movement overlays

        // Display overlays for selected skill

        // Change mouse cursor (change it back afterwards)

    }

    // Called when a skill is selected
    void HandleMoveAction(int x, int y)
    {
        // Move player
        playerScript.MovePlayer(x, y);
        // End turn
        EndPlayerTurn();
    }

    // ==============================
    // Test button for debug purposes
    // ==============================
    public void TestButton()
    {
        overlayScript.RemoveAllOverlays();
        turnState = TurnState.NEW_TURN;
    }
    // ==============================
}