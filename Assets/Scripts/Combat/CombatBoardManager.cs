using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatBoardManager : MonoBehaviour
{
    public static CombatBoardManager instance = null;

    public HexTile[,] gameBoard;
    public int columns, rows;

    public GameObject[] normalTiles;
    public GameObject[] rockTiles;
    public GameObject[] wallTiles;
    public GameObject[] lavaTiles;

    public int playerInitX = 5;
    public int playerInitY = 5;

    // Use this for initialization
    public void SetupBoard(CombatParameters parameters)
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InitializeGameBoard();
        InitializeTerrain();

        InstantiateGameBoard();
        InstantiateObjects();
    }

    // Returns array of all tiles
    public HexTile[] GetSurroundingTiles(int x, int y)
    {
        List<HexTile> adjacentTiles = new List<HexTile>();

        // Above
        if (isHexValid(x, y + 1))
        {
            adjacentTiles.Add(gameBoard[x, y + 1]);
        }

        // Below
        if (isHexValid(x, y - 1))
        {
            adjacentTiles.Add(gameBoard[x, y - 1]);
        }

        // x is even
        if (x % 2 == 0)
        {
            // Top Left
            if (isHexValid(x - 1, y))
            {
                adjacentTiles.Add(gameBoard[x - 1, y]);
            }
            // Top Right
            if (isHexValid(x + 1, y))
            {
                adjacentTiles.Add(gameBoard[x + 1, y]);
            }
            // Bottom Left
            if (isHexValid(x - 1, y - 1))
            {
                adjacentTiles.Add(gameBoard[x - 1, y - 1]);
            }
            // Bottom Right
            if (isHexValid(x + 1, y - 1))
            {
                adjacentTiles.Add(gameBoard[x + 1, y - 1]);
            }
        }
        // x is odd
        else
        {
            // Top Left
            if (isHexValid(x - 1, y + 1))
            {
                adjacentTiles.Add(gameBoard[x - 1, y + 1]);
            }
            // Top Right
            if (isHexValid(x + 1, y + 1))
            {
                adjacentTiles.Add(gameBoard[x + 1, y + 1]);
            }
            // Bottom Left
            if (isHexValid(x - 1, y))
            {
                adjacentTiles.Add(gameBoard[x - 1, y]);
            }
            // Bottom Right
            if (isHexValid(x + 1, y))
            {
                adjacentTiles.Add(gameBoard[x + 1, y]);
            }
        }

        return adjacentTiles.ToArray();
    }

    // Returns the Transform position of a hex by coordinate
    public Vector3 GetHexPosition(int x, int y)
    {
        return gameBoard[x, y].position;
    }

    // Returns whether the hex is valid (not a wall or out of bounds)
    public bool isHexValid(int x, int y)
    {
        if (0 > x || x >= columns || 0 > y || y >= rows)
        {
            return false;
        }

        if (GetHexType(x, y) == HexTileType.WALL)
        {
            return false;
        }

        return true;
    }

    public HexTileType GetHexType(int x, int y)
    {
        return gameBoard[x, y].tileType;
    }

    // Set up an empty game board
    void InitializeGameBoard()
    {
        gameBoard = new HexTile[columns, rows];
        float xTemp, yTemp;
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                HexTileType tileType = HexTileType.NORMAL;

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

                // Save in gameBoard
                gameBoard[x, y] = new HexTile(x, y, hexLocation, tileType);
            }
        }
    }

    // Run algorithm to generate board terrain
    // Modifies the gameBoard[]
    void InitializeTerrain()
    {
        // Make the border all walls
        // Top and Bottom edge
        for (int x = 0; x < columns; x++)
        {
            gameBoard[x, 0].tileType = HexTileType.WALL;
            gameBoard[x, rows - 1].tileType = HexTileType.WALL;
        }

        // Left and Right edge
        for (int y = 1; y < rows - 1; y++)
        {
            gameBoard[0, y].tileType = HexTileType.WALL;
            gameBoard[columns - 1, y].tileType = HexTileType.WALL;
        }
    }
    
    // Reads gameBoard and instantiates everything
    void InstantiateGameBoard()
    {
        // parent to hold all hex tiles
        Transform hexHolder = new GameObject("Board").transform;

        GameObject instance, toInstantiate;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Get tile prefab
                toInstantiate = getTileObjectByType(gameBoard[x, y].tileType);
                instance = Instantiate(toInstantiate, GetHexPosition(x, y), Quaternion.identity) as GameObject;

                // Add hexes to parent
                instance.transform.SetParent(hexHolder);
                instance.name = "hex_" + x + "_" + y;
            }
        }
    }

    // Player and enemy objects
    void InstantiateObjects()
    {
        // TODO: Somehow determine the starting positions of the player and enemies

        // Instantiate the player
        GameObject toInstantiate = PlayerManager.instance.playerCharacter;
        GameObject playerInstance = Instantiate(toInstantiate, GetHexPosition(playerInitX, playerInitY), Quaternion.identity) as GameObject;
        CombatManager.instance.playerInstance = playerInstance;
        CombatManager.instance.playerScript = playerInstance.GetComponent<PlayerObject>();
        CombatManager.instance.playerScript.positionX = playerInitX;
        CombatManager.instance.playerScript.positionY = playerInitY;

        // Instantiate companions

        // Instantiate enemies based on combatParameters

    }

    GameObject getTileObjectByType(HexTileType type)
    {
        GameObject tilePrefab;
        if (type == HexTileType.NORMAL)
        {
            tilePrefab = normalTiles[Random.Range(0, normalTiles.Length)];
        }
        else if (type == HexTileType.WALL)
        {
            tilePrefab = normalTiles[Random.Range(0, normalTiles.Length)];
        }
        else
        {
            // ERROR
            Debug.LogError("ERROR: TileType not handled");
            tilePrefab = new GameObject();
        }

        return tilePrefab;
    }
}