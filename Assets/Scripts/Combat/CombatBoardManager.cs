using UnityEngine;
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

    // Get all tiles within range r, centered at (x, y)
    // - includeCenter decides if the center tile is returned
    // See reference for explanation of algorithm 
    // 
    // Used for AOE skills
    public List<HexTile> GetTilesInRange(int x, int y, int r)
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
                if (tile == null)
                {
                    continue;
                }
                inRangeTiles.Add(tile);
            }
        }

        return inRangeTiles;
    }

    // Get traversable tiles around x, y with given moveRange
    // Uses Dijkstra because we want to find ALL reachable nodes from (x, y)
    // Mainly used for player movement
    public List<HexTile> GetTraversableTiles(int x, int y, int moveRange)
    {
        var traversableTiles = new List<HexTile>();
        var startNode = GetHexTile(x, y);

        // Define dictionaries for "current cost" for each node
        var costSoFar = new Dictionary<HexTile, float>();
        costSoFar[startNode] = 0;

        // Use SortedList as a priority queue
        // Frontier represents the nodes to be visited next, 
        // ordered by low to high estimated distance to goal
        var frontier = new SortedList<float, HexTile>();
        frontier.Add(0, startNode);

        // Loop while nodes are in the frontier
        while (frontier.Count > 0)
        {
            // Get a node in the frontier and add it to traversable
            var currentNode = frontier.Values[0];
            frontier.RemoveAt(0);

            // For each neighbor of currentNode
            foreach (var neighbor in currentNode.TraversableNeighbors)
            {
                // Get cost from current node to neighbor
                var newCost = costSoFar[currentNode] + 1;

                // If the newCost exceed moveRange, it is unreachable
                if (newCost > moveRange)
                    continue;

                // If neighbor already has a previous cost smaller than newCost, skip it
                if (costSoFar.ContainsKey(neighbor) && costSoFar[neighbor] <= newCost)
                    continue;

                // Update the cost for neighbor
                costSoFar[neighbor] = newCost;

                // Add to priorty queue based on cost and distance to the goal
                var priority = newCost + UnityEngine.Random.Range(0f, 0.5f);

                // Get another random value if the priority already exists.
                // This hack is necessary because I couldn't find a sorted structure
                // that supports duplicate keys in C#.
                while (frontier.ContainsKey(priority))
                {
                    priority = newCost + UnityEngine.Random.Range(0f, 0.5f);
                }

                frontier.Add(priority, neighbor);
            }
        }

        // Remove the start node (we don't want the start node for movement)
        costSoFar.Remove(startNode);

        // Add all the tiles visited
        foreach (var tile in costSoFar.Keys)
        {
            traversableTiles.Add(tile);
        }

        return traversableTiles;
    }

    // Returns a List of HexTiles that defines the shortest path from start to end
    // Uses A* pathfinding algorithm
    // Algorithm adapted from the reference site
    // The returned List does not contain the starting node
    public List<HexTile> GetTilesInPath(int startX, int startY, int endX, int endY, bool ignoreObjects)
    {
        var startNode = GetHexTile(startX, startY);
        var endNode = GetHexTile(endX, endY);
        var goalReached = false;

        // If goal is not reachable, the path goes as close to the goal as possible
        var closestDistanceToGoal = DistanceBetween(startNode, endNode);
        var validNodeClosestToGoal = startNode;

        // Use SortedList as a priority queue
        // Frontier represents the nodes to be visited next, 
        // ordered by low to high estimated distance to goal
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
            // Get first node in the frontier (smallest estimated distance to the goal)
            var currentNode = frontier.Values[0];
            frontier.RemoveAt(0);

            // If it's the goal, then we are done
            if (currentNode.Equals(endNode))
            {
                goalReached = true;
                break;
            }

            // Get neighbors depending on if want to ignore objects in the path
            List<HexTile> neighborsList;
            if (ignoreObjects) neighborsList = currentNode.Neighbors;
            else neighborsList = currentNode.TraversableNeighbors;

            // For each neighbor of currentNode
            foreach (var neighbor in neighborsList)
            {
                // Get cost from current node to neighbor
                var newCost = costSoFar[currentNode] + 1;

                // If neighbor already has a previous cost smaller than newCost, skip it
                if (costSoFar.ContainsKey(neighbor) && costSoFar[neighbor] <= newCost)
                    continue;

                // Update the cost for neighbor
                costSoFar[neighbor] = newCost;

                // Add to priorty queue based on cost and distance to the goal
                var distanceToGoal = DistanceBetween(neighbor, endNode);
                var priority = newCost + distanceToGoal + UnityEngine.Random.Range(0f, 0.5f);

                // Get another random value if the priority already exists.
                // This hack is necessary because I couldn't find a sorted structure
                // that supports duplicate keys in C#.
                while (frontier.ContainsKey(priority))
                {
                    priority = newCost + distanceToGoal + UnityEngine.Random.Range(0f, 0.5f);
                }

                frontier.Add(priority, neighbor);

                // Set the neighbor node to "come from" the current node
                cameFrom[neighbor] = currentNode;

                // Update validNodeClosestToGoal in case endNode is unreachable
                if (distanceToGoal < closestDistanceToGoal)
                {
                    closestDistanceToGoal = distanceToGoal;
                    validNodeClosestToGoal = neighbor;
                }
            }
        }

        var pathTiles = new List<HexTile>();
        var pathNode = endNode;

        // If goalReached flag is false, then return path to validNodeClosestToGoal instead
        if (!goalReached)
        {
            pathNode = validNodeClosestToGoal;
        }

        // Follow the nodes from endNode all the way back to startNode
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

    // Returns the hex tile by coordinate
    public HexTile GetHexTile(int x, int y)
    {
        if (!IsHexWithinBounds(x, y))
        {
            return null;
        }

        return GameBoard[x, y];
    }

    // Returns whether the hex is valid (not a wall or out of bounds)
    public bool IsHexValid(int x, int y)
    {
        if (!IsHexWithinBounds(x, y))
        {
            return false;
        }

        if (!GetHexTile(x, y).IsTraversable)
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

    // Sets obj to be queued on tile (x, y), so that no other objects can move to (x, y)
    // Also makes the obj's previous tile free to me moved on
    public void SetObjectOnTileQueued(int x, int y, MovingObject obj)
    {
        // Friendly objects can make multiple movements, so we have to null the previous tile
        if (obj is FriendlyObject)
        {
            var friendlyObj = (FriendlyObject)obj;
            GetHexTile(friendlyObj.ShadowX, friendlyObj.ShadowY).ObjectOnTileQueued = null;
        }

        GetHexTile(obj.X, obj.Y).ObjectOnTile = null;
        GetHexTile(x, y).ObjectOnTileQueued = obj;
    }

    // If there is a queued object on the tile, make it ACTUALLY on the tile
    public void SetObjectOnTile(int x, int y, MovingObject obj)
    {
        var tile = GetHexTile(x, y);
        if (tile.ObjectOnTileQueued != null)
        {
            tile.ObjectOnTile = tile.ObjectOnTileQueued;
            tile.ObjectOnTileQueued = null;
        }
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
                GameBoard[x, y] = new HexTile(x, y, hexLocation, tileType);
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

    // Friendly objects include the main hero and companions
    void InstantiateFriendlyObjects()
    {
        // TODO: Somehow determine the starting positions of the player
        var tile = GetHexTile(PlayerInitX, PlayerInitY);

        // Instantiate the player
        var toInstantiate = PlayerManager.Instance.PlayerCharacterPrefab;
        var playerInstance = Instantiate(toInstantiate, tile.Position, Quaternion.identity) as GameObject;
        var playerScript = playerInstance.GetComponent<PlayerObject>();

        // Setup some data in related scripts
        playerScript.X = PlayerInitX;
        playerScript.Y = PlayerInitY;
        playerScript.InitializeData();
        tile.ObjectOnTile = playerScript;
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
            var tile = GetHexTile(EnemyInitX, EnemyInitY);
            var toInstantiate = enemyData.Prefab;
            var enemyInstance = Instantiate(toInstantiate, tile.Position, Quaternion.identity) as GameObject;
            var enemyScript = enemyInstance.GetComponent<EnemyObject>();

            // Setup some data in related scripts
            enemyScript.X = EnemyInitX;
            enemyScript.Y = EnemyInitY;
            tile.ObjectOnTile = enemyScript;
            enemyScript.SetData(enemyData);
            CombatManager.Instance.AddEnemyObject(enemyInstance);
            
            // Get position of next enemy
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

