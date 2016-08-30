using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum HexTileType
{
    None,
    Normal,
    Rock,
    Water,
    Lava,
    Wall,
};

public class HexTile
{
    public int X, Y;
    public Vector3 Position;
    public HexTileType TileType;
    public List<HexTile> Neighbors { get { return GetNeighbors(); } }
    public List<HexTile> TraversableNeighbors { get { return GetTraversableNeighbors(); } }
    public bool IsTraversable { get { return GetIsTraversable(); } }
    public MovingObject ObjectOnTile;
    public MovingObject ObjectQueuedOnTile;

    // Constructor
    public HexTile(int x, int y, Vector3 position, HexTileType type)
    {
        this.X = x;
        this.Y = y;
        this.Position = position;
        this.TileType = type;
    }

    List<HexTile> GetNeighbors()
    {
        var neighborTiles = new List<HexTile>();
        HexTile tempTile;

        // Above
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y - 1);
        if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

        // Below
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y + 1);
        if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

        // (&) is the bitwise AND operator
        // x is even
        if ((X & 1) == 0)
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y - 1);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y - 1);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);
        }
        // x is odd
        else
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y + 1);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y + 1);
            if (tempTile != null && tempTile.IsTraversable) neighborTiles.Add(tempTile);
        }

        return neighborTiles;
    }

    List<HexTile> GetTraversableNeighbors()
    {
        var traversableNeighborTiles = new List<HexTile>();
        foreach (var neighbor in Neighbors)
        {
            if (neighbor.IsTraversable)
            {
                traversableNeighborTiles.Add(neighbor);
            }
        }

        return traversableNeighborTiles;
    }

    bool GetIsTraversable()
    {
        if (TileType == HexTileType.None || TileType == HexTileType.Wall)
        {
            return false;
        }
        
        return true;
    }
}
