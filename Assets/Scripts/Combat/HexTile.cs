using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HexTileType
{
    Normal,
    Rock,
    Water,
    Lava,
    Wall,
};

public class HexTile
{
    public int X, Y;
    public int CubeX, CubeY;
    public Vector3 Position;
    public HexTileType TileType;
    public List<HexTile> Neighbors { get { return GetNeighbors(); } }
    public MovingObject ObjectOnTile;
    public MovingObject ObjectQueuedOnTile;

    public bool Traversable;

    // Constructor
    public HexTile(int x, int y, Vector3 position, HexTileType type, bool traversable)
    {
        this.X = x;
        this.Y = y;
        this.Position = position;
        this.TileType = type;
        this.Traversable = traversable;
    }

    List<HexTile> GetNeighbors()
    {
        var neighborTiles = new List<HexTile>();
        HexTile tempTile;

        // Above
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y - 1);
        if (tempTile != null) neighborTiles.Add(tempTile);

        // Below
        tempTile = CombatBoardManager.Instance.GetHexTile(X, Y + 1);
        if (tempTile != null) neighborTiles.Add(tempTile);

        // (&) is the bitwise AND operator
        // x is even
        if ((X & 1) == 0)
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y - 1);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y - 1);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null) neighborTiles.Add(tempTile);
        }
        // x is odd
        else
        {
            // Top Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Top Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Bottom Left
            tempTile = CombatBoardManager.Instance.GetHexTile(X - 1, Y + 1);
            if (tempTile != null) neighborTiles.Add(tempTile);

            // Bottom Right
            tempTile = CombatBoardManager.Instance.GetHexTile(X + 1, Y + 1);
            if (tempTile != null) neighborTiles.Add(tempTile);
        }

        return neighborTiles;
    }
}
