﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/* CombatBoardManager
 * 
 * This scripts manages the combat board and provides useful functions for accessing tiles.
 * 
 * The board is made of hex tiles, arranged in "odd-q" vertical layout.
 * Reference: http://www.redblobgames.com/grids/hexagons
 * 
 * The origin (0, 0) starts in the top left corner. 
 * x increases moving towards the right.
 * y increases moving downwards.
*/

public class CombatBoardManager : MonoBehaviour
{
    // Allows board manager to be called from anywhere
    public static CombatBoardManager Instance = null;

    // =========================
    // Public functions
    // =========================

    public HexTile[,] GameBoard;
    public int Columns, Rows;

    public GameObject[] NormalTiles;
    public GameObject[] RockTiles;
    public GameObject[] WallTiles;
    public GameObject[] LavaTiles;

    public int PlayerInitX = 5;
    public int PlayerInitY = 5;

    // Use this for initialization
    public void SetupBoard(CombatParameters parameters)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _parameters = parameters;

        InitializeGameBoard();
        InitializeTerrain();

        InstantiateGameBoard();
        InstantiateFriendlyObjects();
        InstantiateEnemyObjects();
    }

    // Returns array of all tiles
    public List<HexTile> GetSurroundingTiles(int x, int y)
    {
        List<HexTile> adjacentTiles = new List<HexTile>();

        // Above
        if (IsHexValid(x, y - 1))
        {
            adjacentTiles.Add(GameBoard[x, y - 1]);
        }

        // Below
        if (IsHexValid(x, y + 1))
        {
            adjacentTiles.Add(GameBoard[x, y + 1]);
        }

        // x is even
        // (&) is the bitwise AND operator
        if ((x & 1) == 0)
        {
            // Top Left
            if (IsHexValid(x - 1, y - 1))
            {
                adjacentTiles.Add(GameBoard[x - 1, y - 1]);
            }
            // Top Right
            if (IsHexValid(x + 1, y - 1))
            {
                adjacentTiles.Add(GameBoard[x + 1, y - 1]);
            }
            // Bottom Left
            if (IsHexValid(x - 1, y))
            {
                adjacentTiles.Add(GameBoard[x - 1, y]);
            }
            // Bottom Right
            if (IsHexValid(x + 1, y))
            {
                adjacentTiles.Add(GameBoard[x + 1, y]);
            }
        }
        // x is odd
        else
        {
            // Top Left
            if (IsHexValid(x - 1, y))
            {
                adjacentTiles.Add(GameBoard[x - 1, y]);
            }
            // Top Right
            if (IsHexValid(x + 1, y))
            {
                adjacentTiles.Add(GameBoard[x + 1, y]);
            }
            // Bottom Left
            if (IsHexValid(x - 1, y + 1))
            {
                adjacentTiles.Add(GameBoard[x - 1, y + 1]);
            }
            // Bottom Right
            if (IsHexValid(x + 1, y + 1))
            {
                adjacentTiles.Add(GameBoard[x + 1, y + 1]);
            }
        }

        return adjacentTiles;
    }

    // Get all tiles within range r, centered at (x, y)
    // - includeCenter decides if the center tile is returned
    // See reference for explanation of algorithm 
    public List<HexTile> GetTilesInRange(int x, int y, int r, bool includeCenter)
    {
        List<HexTile> inRangeTiles = new List<HexTile>();

        // Convert center offset coord to cube coord
        var centerCube = new CubeHex(x, y);

        // Loop over all tiles within the range based on cube coordinates
        for (var dx = -r; dx <= r; dx++)
        {
            var start = Mathf.Max(-r, -dx - r);
            var end = Mathf.Min(r, -dx + r);
            for (var dy = start; dy <= end; dy++)
            {
                var dz = -dx - dy;

                // Add the dx, dy, and dz to the center coord
                var tempCube = new CubeHex(centerCube.X + dx, centerCube.Y + dy, centerCube.Z + dz);
                var tile = GetHexTileByCube(tempCube);

                // Skip if tile is invalid, or it's the center tile and includeCenter is false
                if (tile == null || (!includeCenter && tile.X == x && tile.Y == y))
                {
                    continue;
                }
                inRangeTiles.Add(tile);
            }
        }

        return inRangeTiles;
    }

    // Returns a List of HexTiles that defines the shortest path from start to end
    // Uses A* pathfinding algorithm
    // Algorithm adapted from the reference site
    // The returned List does not contain the starting node
    public List<HexTile> GetTilesInPath(int startX, int startY, int endX, int endY)
    {
        var startNode = GetHexTile(startX, startY);
        var endNode = GetHexTile(endX, endY);
        bool goalReached = false;

        // Use SortedList as a priority queue
        // Frontier represents the nodes to be visitied next, 
        // ordered by low to high priority
        var frontier = new SortedList<float, HexTile>();
        frontier.Add(0, startNode);

        // Define dictionaries for "previous node" and "current cost" for each node
        var cameFrom = new Dictionary<HexTile, HexTile>();
        var costSoFar = new Dictionary<HexTile, float>();
        cameFrom[startNode] = startNode;
        costSoFar[startNode] = 0;

        // Loop while nodes are in the frontier
        while (frontier.Count > 0)
        {
            printFrontier(frontier);

            // Get last node in the frontier (highest priority)
            var lastIndex = frontier.Count - 1;
            var currentNode = frontier.Values[0];
            frontier.RemoveAt(0);

            // If its the goal, then we are done
            if (currentNode.Equals(endNode))
            {
                goalReached = true;
                break;
            }

            // For each neighbor of currentNode
            foreach (var neighbor in currentNode.Neighbors)
            {
                // Get cost from current node to neighbor
                var newCost = costSoFar[currentNode] + 1;

                // If neighbor does not have a cost yet, 
                // or the cost from currentNode is smaller than the previously calculate cost
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    // Update the cost for neighbor
                    costSoFar[neighbor] = newCost;

                    // Add to priorty queue based on cost and distance to the goal
                    var distanceToGoal = DistanceBetween(neighbor, endNode);
                    var priority = newCost + distanceToGoal + UnityEngine.Random.Range(0f, 0.5f); // + Heuristic(next, goal)

                    // Get another random value if the priority already exists.
                    // This hack is necessary because I couldn't find a sorted structure
                    // that supports duplicate keys.
                    while (frontier.ContainsKey(priority))
                    {
                        priority = newCost + distanceToGoal + UnityEngine.Random.Range(0f, 0.5f);
                    }

                    frontier.Add(priority, neighbor);

                    // Set the neighbor node to "come from" the current node
                    cameFrom[neighbor] = currentNode;
                }
            }

        }

        // If goalReached flag is not true, the return empty path
        List<HexTile> pathTiles = new List<HexTile>();
        if (!goalReached) return pathTiles;

        // Otherwise, follow the nodes from endNode all the way back to startNode
        var pathNode = endNode;
        pathTiles.Add(pathNode);
        while (cameFrom[pathNode] != startNode)
        {
            pathNode = cameFrom[pathNode];
            pathTiles.Add(pathNode);
        }

        // Reverse the list so that the start of path is in the beginning
        pathTiles.Reverse();
        return pathTiles;
    }

    private void printFrontier(SortedList<float, HexTile> frontier)
    {
        string keys = "";
        string values = "";
        foreach (var key in frontier.Keys)
        {
            keys += key.ToString() + ", ";
            //values += "[" + frontier[key].X.ToString() + ", " + frontier[key].Y.ToString() + "], ";
        }

        print(keys);
        //print(values);
    }

    // Returns the hex tile by coordinate
    public HexTile GetHexTile(int x, int y)
    {
        if (IsHexWithinBounds(x, y))
        {
            return GameBoard[x, y];
        }
        else
        {
            return null;
        }
    }

    // Returns whether the hex is valid (not a wall or out of bounds)
    public bool IsHexValid(int x, int y)
    {
        if (!IsHexWithinBounds(x, y))
        {
            return false;
        }

        if (GetHexTile(x, y).TileType == HexTileType.Wall)
        {
            return false;
        }

        return true;
    }

    public bool IsHexWithinBounds(int x, int y)
    {
        if (0 > x || x >= Columns || 0 > y || y >= Rows)
        {
            return false;
        }
        return true;
    }

    public void SetMovingObjectAt(MovingObject obj, int x, int y)
    {

    }

    // =========================
    // Private functions
    // =========================

    CombatParameters _parameters;

    // Set up an empty game board
    void InitializeGameBoard()
    {
        GameBoard = new HexTile[Columns, Rows];
        float xTemp, yTemp;
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                HexTileType tileType = HexTileType.Normal;

                // hex protocol
                Vector3 hexLocation;
                if (x % 2 == 0) // if even
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = -y;
                    hexLocation = new Vector3(xTemp, yTemp, 0f);
                }
                else // if odd, shift tile downwards half a space (-y - 0.5)
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = (float)(-y - 0.5);
                    hexLocation = new Vector3(xTemp, yTemp, 0f);
                }

                // Save in gameBoard
                GameBoard[x, y] = new HexTile(x, y, hexLocation, tileType, true);
            }
        }
    }

    // Run algorithm to generate board terrain
    // Modifies the gameBoard[]
    void InitializeTerrain()
    {
        // Make the border all walls
        // Top and Bottom edge
        for (int x = 0; x < Columns; x++)
        {
            GameBoard[x, 0].TileType = HexTileType.Wall;
            GameBoard[x, Rows - 1].TileType = HexTileType.Wall;
        }

        // Left and Right edge
        for (int y = 1; y < Rows - 1; y++)
        {
            GameBoard[0, y].TileType = HexTileType.Wall;
            GameBoard[Columns - 1, y].TileType = HexTileType.Wall;
        }
    }

    // Reads gameBoard and instantiates everything
    void InstantiateGameBoard()
    {
        // parent to hold all hex tiles
        Transform hexHolder = new GameObject("Board").transform;

        GameObject instance, toInstantiate;

        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                // Get tile prefab
                toInstantiate = GetTileObjectByType(GameBoard[x, y].TileType);
                instance = Instantiate(toInstantiate, GetHexTile(x, y).Position, Quaternion.identity) as GameObject;

                // Add hexes to parent
                instance.transform.SetParent(hexHolder);
                instance.name = "hex_" + x + "_" + y;
            }
        }
    }

    // Player and enemy objects
    void InstantiateFriendlyObjects()
    {
        // TODO: Somehow determine the starting positions of the player

        // Instantiate the player
        var toInstantiate = PlayerManager.Instance.PlayerCharacterPrefab;
        var playerInstance = Instantiate(toInstantiate, GetHexTile(PlayerInitX, PlayerInitY).Position, Quaternion.identity) as GameObject;
        playerInstance.GetComponent<PlayerObject>().X = PlayerInitX;
        playerInstance.GetComponent<PlayerObject>().Y = PlayerInitY;
        CombatManager.Instance.SetPlayerObject(playerInstance);

        // Instantiate companions


    }

    void InstantiateEnemyObjects()
    {
        // TODO: Somehow determine the starting positions of the enemies
        var EnemyInitX = 10;
        var EnemyInitY = 10;

        // TODO: Instantiate enemies based on combatParameters
        foreach (var enemyData in _parameters.Enemies)
        {
            var toInstantiate = enemyData.Prefab;
            var enemyInstance = Instantiate(toInstantiate, GetHexTile(EnemyInitX, EnemyInitY).Position, Quaternion.identity) as GameObject;
            enemyInstance.GetComponent<EnemyObject>().X = EnemyInitX;
            enemyInstance.GetComponent<EnemyObject>().Y = EnemyInitY;
            enemyInstance.GetComponent<EnemyObject>().SetData(enemyData);
            CombatManager.Instance.AddEnemyObject(enemyInstance);
            EnemyInitX++;
            EnemyInitY++;
        }
    }

    GameObject GetTileObjectByType(HexTileType type)
    {
        GameObject tilePrefab;
        if (type == HexTileType.Normal)
        {
            tilePrefab = NormalTiles[UnityEngine.Random.Range(0, NormalTiles.Length)];
        }
        else if (type == HexTileType.Wall)
        {
            tilePrefab = NormalTiles[UnityEngine.Random.Range(0, NormalTiles.Length)];
        }
        else
        {
            // ERROR
            Debug.LogError("ERROR: TileType not handled");
            tilePrefab = new GameObject();
        }

        return tilePrefab;
    }

    // Returns a HexTile based on its cube coordinates
    // See reference for info on cube coordinates
    HexTile GetHexTileByCube(CubeHex cube)
    {
        var offsetHex = new OffsetHex(cube);
        if (!IsHexWithinBounds(offsetHex.X, offsetHex.Y))
        {
            return null;
        }
        return GameBoard[offsetHex.X, offsetHex.Y];
    }

    // Gets tile distance between A and B
    int DistanceBetween(HexTile tileA, HexTile tileB)
    {
        var a = new CubeHex(tileA);
        var b = new CubeHex(tileB);
        var maxParam = new int[3];
        maxParam[0] = Mathf.Abs(a.X - b.X);
        maxParam[1] = Mathf.Abs(a.Y - b.Y);
        maxParam[2] = Mathf.Abs(a.Z - b.Z);
        return Mathf.Max(maxParam);
    }

    class CubeHex
    {
        public int X;
        public int Y;
        public int Z;

        // Constructor using odd-q offset coordinates
        public CubeHex(int xOffset, int yOffset)
        {
            X = xOffset;
            Z = yOffset - (xOffset - (xOffset & 1)) / 2;
            Y = -X - Z;
        }

        // Constructor using cube coordinates
        public CubeHex(int xCube, int yCube, int zCube)
        {
            X = xCube;
            Y = yCube;
            Z = zCube;
        }

        // Constructor using OffsetHex
        public CubeHex(OffsetHex offsetHex) : this(offsetHex.X, offsetHex.Y) { }

        // Constructor using HexTile
        public CubeHex(HexTile tile) : this(tile.X, tile.Y) { }
    }

    class OffsetHex
    {
        public int X;
        public int Y;

        // Constructor using odd-q offset coordinates
        public OffsetHex(int xOffset, int yOffset)
        {
            X = xOffset;
            Y = yOffset;
        }

        // Constructor using cube coordinates
        public OffsetHex(int xCube, int yCube, int zCube)
        {
            X = xCube;
            Y = zCube + (xCube - (xCube & 1)) / 2;
        }

        // Constructor using CubeHex
        public OffsetHex(CubeHex cube) : this(cube.X, cube.Y, cube.Z) { }
    }
}

