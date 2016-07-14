using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;

/*
 * BoardManager
 * 
 * This script is in charge of initializing the game board.
 * Also has functions to get various information about the state of the board.
 *  
 */

public class BoardManager : MonoBehaviour
{
    public int columns; // Gameboard dimension - columns
    public int rows; // Gameboard dimension - rows

    public int xWorldCenter;
    public int yWorldCenter;

    public GameObject[] hexRooms;
    public MapHex[,] gameBoard;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter;

    public void BoardSetup()
    {
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;

        InitializeGameBoard();
        InitializeWalls();
        InitializeWorldObjects();

        InstantiateGameObjects();
    }

    // Returns whether the hex is valid (not a wall or out of bounds)
    public bool isHexValid(int x, int y)
    {
        if (0 > x || x >= columns || 0 > y || y >= rows)
        {
            return false;
        }

        if (GetHexProperty(x, y) == HexType.WALL)
        {
            return false;
        }

        return true;
    }

    public bool isHexVisited(int x, int y)
    {
        if (gameBoard[x, y].visited == true)
        {
            return true;
        }

        return false;
    }

    // Returns the Transform position of a hex by coordinate
    public Vector3 GetHexPosition(int x, int y)
    {
        return gameBoard[x, y].position;
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

    public HexType GetHexProperty(int x, int y)
    {
        return gameBoard[x, y].property;
    }

    // Set up an empty game board
    void InitializeGameBoard()
    {
        gameBoard = new MapHex[columns, rows];
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
        // Make the border all walls
        // Top and Bottom edge
        for (int x = 0; x < columns; x++)
        {
            gameBoard[x, 0].property = HexType.WALL;
            gameBoard[x, rows - 1].property = HexType.WALL;
        }

        // Left and Right edge
        for (int y = 1; y < rows - 1; y++)
        {
            gameBoard[0, y].property = HexType.WALL;
            gameBoard[columns - 1, y].property = HexType.WALL;
        }
    }

    void InitializeWorldObjects()
    {

    }

    // Reads gameBoard and instantiates everything
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
