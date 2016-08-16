using UnityEngine;
using System.Collections;

public enum HexTileType
{
    Normal,
    Rock,
    Water,
    Lava,
    Wall,
};

public class HexTile {

    public int X, Y;
    public int CubeX, CubeY;
    public Vector3 Position;
    public HexTileType TileType;

    // Constructor
    public HexTile(int x, int y, Vector3 position, HexTileType type)
    {
        this.X = x;
        this.Y = y;
        this.Position = position;
        this.TileType = type;
    }
}
