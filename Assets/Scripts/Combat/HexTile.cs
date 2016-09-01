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
    public MovingObject ObjectOnTile = null;
    public MovingObject ObjectOnTileQueued = null;

    // Constructor
    public HexTile(int x, int y, Vector3 position, HexTileType type)
    {
        this.X = x;
        this.Y = y;
        this.Position = position;
        this.TileType = type;
    }

    List<HexTile> _neighbors = null;
    List<HexTile> GetNeighbors()
    {
        // The neighbors of a tile don't change, so we can cache them
        if (_neighbors != null) return _neighbors;

        _neighbors = new List<HexTile>();
        HexTile tempTile;

        // Above
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y - 1);
        if (tempTile != null) _neighbors.Add(tempTile);

        // Below
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y + 1);
        if (tempTile != null) _neighbors.Add(tempTile);

        // (&) is the bitwise AND operator
        // x is even
        if ((X & 1) == 0)
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y - 1);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y - 1);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null) _neighbors.Add(tempTile);
        }
        // x is odd
        else
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y + 1);
            if (tempTile != null) _neighbors.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y + 1);
            if (tempTile != null) _neighbors.Add(tempTile);
        }

        return _neighbors;
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
        // If the tile type is None or Wall
        if (TileType == HexTileType.None || TileType == HexTileType.Wall)
        {
            return false;
        }

        // If there is something on the tile, or WILL be on the tile
        if (ObjectOnTile != null || ObjectOnTileQueued != null)
        {
            return false;
        }

        return true;
    }
}
