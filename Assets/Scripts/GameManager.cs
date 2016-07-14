using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;

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

    BoardManager boardScript;
    HexOverlayManager hexOverlayScript;
    InventoryManager inventoryScript;
    InstanceManager instanceScript;

    public GameObject player;
    public GameObject playerInstance;

    public int playerInitialX;
    public int playerInitialY;

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

        boardScript = GetComponent<BoardManager>();
        hexOverlayScript = GetComponent<HexOverlayManager>();
        inventoryScript = GetComponent<InventoryManager>();
        instanceScript = GetComponent<InstanceManager>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        // set up board
        Debug.Log("Initialize gameboard");
        boardScript.BoardSetup();

        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player, boardScript.GetHexPosition(playerInitialX, playerInitialY), Quaternion.identity) as GameObject;

        // Create "ring" of overlays that moves with the player
        Debug.Log("Instantiate overlay");
        hexOverlayScript.InstantiateOverlayAroundPlayer(boardScript.xWorldCenter, boardScript.yWorldCenter);
        MovePlayer(playerInitialX, playerInitialY);

        // Initialize inventory
        Debug.Log("Instantiate inventory");
        inventoryScript.InventorySetup();

        Debug.Log("Initialize game complete!");
    }

    public void MovePlayer(int x, int y)
    {
        playerInstance.transform.position = boardScript.GetHexPosition(x, y);
        hexOverlayScript.moveOverlay(x, y);

        if (boardScript.isHexVisited(x, y) == false && boardScript.GetHexProperty(x, y) == HexType.INSTANCE) // if visiting for the first time && property is INSTANCE, run testInstance
        {
            instanceScript.TestInstance();
        }

        boardScript.SetHexVisited(x, y, true);
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
            Debug.Log("property: " + boardScript.gameBoard[xDebug, yDebug].property);
            Debug.Log("visited: " + boardScript.gameBoard[xDebug, yDebug].visited);
        }
    }
}
// THIS IS A TEST - PLEASE DELETE!