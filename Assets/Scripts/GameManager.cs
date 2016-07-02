using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public BoardManager boardScript;
    public HexOverlayManager hexOverlayScript;

    public GameObject player;
    public GameObject playerInstance;

    //[HideInInspector]
    public Vector3 playerPosition;

    //test
    public int x = 0;

    void Awake()
    {
        // test
        x = 1;

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

        InitializeGame();
    }

    public void InitializeGame()
    {
        boardScript.BoardSetup(); // set up board
        playerInstance = Instantiate(player, boardScript.trueCenter, Quaternion.identity) as GameObject; // initialize player

        //Debug.Log(playerInstance);

        //MovePlayer(0, 0); // move player // MUST FIX X AND Y ARE NOOOOOOOOOOT WORLD COORDINATES

        hexOverlayScript.createOverlay(boardScript.trueCenter.x, boardScript.trueCenter.y); // only here for testing, move to MovePlayer later
    }

    public void MovePlayer(int x, int y)
    {
        playerInstance.transform.position = new Vector3(boardScript.gameBoard[x, y].x, boardScript.gameBoard[x, y].y, 0f);
        hexOverlayScript.moveOverlay(x, y);
    }

    public void UpdatePlayer(int x, int y)
    {
        boardScript.gameBoard[x, y].visited = true;
    }

    public void inputSequence(int x, int y)
    {
        MovePlayer(x, y);
        UpdatePlayer(x, y);

        // DEBUG
        xDebug = x;
        yDebug = y;
    }

    // DEBUG
    private int xDebug;
    private int yDebug;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("xDebug, yDebug " + xDebug + ", " + yDebug);
            Debug.Log("x(TRUE): " + boardScript.gameBoard[xDebug, yDebug].x);
            Debug.Log("y(TRUE): " + boardScript.gameBoard[xDebug, yDebug].y);
            Debug.Log("property: " + boardScript.gameBoard[xDebug, yDebug].property);
            Debug.Log("visited: " + boardScript.gameBoard[xDebug, yDebug].visited);
        }
    }
}
