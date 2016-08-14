using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

/*
 * GameManager
 * 
 * This script controls the general flow of the game 
 * and also initializes other scripts to do everything else.
 * 
 */

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    BoardManager boardManager;
    HexOverlayManager hexOverlayManager;
    InventoryManager inventoryManager;
    InstanceManager instanceManager;
    PlayerManager playerManager;

    public GameObject player;
    public GameObject playerInstance;

    public int playerInitialX;
    public int playerInitialY;

    public CombatParameters combatParameters;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // gameObject refers to game object this component is attached to // NOTE IS THERE A BUG? NO RETURN?
        }

        DontDestroyOnLoad(gameObject); // To preserve game data such as score between stages

        boardManager = GetComponent<BoardManager>();
        hexOverlayManager = GetComponent<HexOverlayManager>();
        inventoryManager = GetComponent<InventoryManager>();
        instanceManager = GetComponent<InstanceManager>();
        playerManager = GetComponent<PlayerManager>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        // set up board
        Debug.Log("Initialize gameboard");
        boardManager.BoardSetup();

        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player, boardManager.GetHexPosition(playerInitialX, playerInitialY), Quaternion.identity) as GameObject;

        // Create "ring" of overlays that moves with the player
        Debug.Log("Instantiate overlay");
        hexOverlayManager.InstantiateOverlayAroundPlayer(boardManager.xWorldCenter, boardManager.yWorldCenter);
        MovePlayer(playerInitialX, playerInitialY);

        // Initialize inventory
        Debug.Log("Instantiate inventory");
        inventoryManager.InventorySetup();

        playerManager.SetupPlayer();

        Debug.Log("Initialize game complete!");
    }

    public void StartCombat()
    {
        // Setup parameters

        // Load combat scene
        SceneManager.LoadScene("Combat");
    }

    public void MovePlayer(int x, int y)
    {
        playerInstance.transform.position = boardManager.GetHexPosition(x, y);
        hexOverlayManager.moveOverlay(x, y);

        if (boardManager.isHexVisited(x, y) == false && boardManager.GetHexProperty(x, y) == HexType.INSTANCE) // if visiting for the first time && property is INSTANCE, run testInstance
        {
            instanceManager.TestInstance();
        }

        boardManager.SetHexVisited(x, y, true);
    }

    public void handleHexClick(int x, int y)
    {
        MovePlayer(x, y);

        // DEBUG
        xDebug = x;
        yDebug = y;
    }

    // DEBUG
    private int xDebug;
    private int yDebug;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("xDebug, yDebug " + xDebug + ", " + yDebug);
            //Debug.Log("x(TRUE): " + boardScript.gameBoard[xDebug, yDebug].x);
            //Debug.Log("y(TRUE): " + boardScript.gameBoard[xDebug, yDebug].y);
            Debug.Log("property: " + boardManager.gameBoard[xDebug, yDebug].property);
            Debug.Log("visited: " + boardManager.gameBoard[xDebug, yDebug].visited);
        }
    }
}
// THIS IS A TEST - PLEASE DELETE!