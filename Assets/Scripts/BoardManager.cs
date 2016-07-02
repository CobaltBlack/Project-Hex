using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public enum HexType { NORMAL, WALL };

    //[Serializable] // (allows us to embed a class with sub properties in the inspector)
    public class MapHex
    {
        public Vector3 transformPosition;
        public HexType property;

        //public bool player;
        //public bool traversable;
        public bool visited = false;

        public MapHex(Vector3 position, HexType hexProperty) // Constructor
        {
            transformPosition = position;
            property = hexProperty;

            //player = isPlayer; // can be kept track of by GameManager
            //traversable = isTraversable; // can be kept track of by using property
            //visited = isVisited;
        }
    }

    static int columns = 19; // Gameboard dimension - columns
    static int rows = 19; // Gameboard dimension - rows

    public int xWorldCenter;
    public int yWorldCenter;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter;

    void Awake()
    {
        // holds WORLD coordinate
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;
    }

    public GameObject[] hexRooms;

    public MapHex[,] gameBoard = new MapHex[columns, rows]; // removing static status from columns and rows will break this

    public void BoardSetup()
    {
        InitializeGameBoard();
        InitializeWalls();
        InitializeWorldObjects();

        InstantiateGameObjects();

    }

    // 
    public bool isHexValid(int x, int y)
    {
        if (0 > x || x >= columns || 0 > y || y >= rows)
        {
            return false;
        }

        return true;
    }

    // Returns the Transform position of a hex by coordinate
    public Vector3 GetHexPosition(int x, int y)
    {
        return gameBoard[x, y].transformPosition;
    }

    // Sets visited property of a hex
    public void SetHexVisited(int x, int y, bool visited)
    {
        if (0 > x || x >= columns || 0 > y || y >= rows)
        {
            return;
        }
        gameBoard[x, y].visited = visited;
    }

    // Set up an empty game board
    void InitializeGameBoard()
    {
        float xTemp, yTemp;
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // TODO: Select random hextype
                HexType hexType = HexType.NORMAL;

                // hex protocol
                Vector3 hexLocation;
                if (x % 2 == 0) // if even
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = y;
                    hexLocation = new Vector3(xTemp, yTemp, 0f);
                }
                else // if odd, shift tile up half a space (y + 0.5)
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = (float)(y + 0.5);
                    hexLocation = new Vector3(xTemp, yTemp, 0f);
                }

                // save center vector3
                if (x == xWorldCenter && y == yWorldCenter)
                {
                    trueCenter = hexLocation;
                }

                // Save in gameBoard
                gameBoard[x, y] = new MapHex(hexLocation, hexType); // log info to gameBoard
            }
        }
    }

    void InitializeWalls()
    {

    }

    void InitializeWorldObjects()
    {

    }

    void InstantiateGameObjects()
    {
        // parent
        Transform hexHolder = new GameObject("Board").transform; 
        GameObject instance, toInstantiate;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // randomly choose room 
                // TODO: Choose room based on HexType
                int hexChoice = Random.Range(0, hexRooms.Length);
                toInstantiate = hexRooms[hexChoice]; // Choose blank tile

                // instantiation
                instance = Instantiate(toInstantiate, GetHexPosition(x, y), Quaternion.identity) as GameObject;

                // editing GameObject
                instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
                instance.name = "hex_" + x + "_" + y; // name the hexes by coordinates
            }
        }
    }
}
