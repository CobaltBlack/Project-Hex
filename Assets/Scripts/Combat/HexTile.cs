using UnityEngine;
using System.Collections;

public enum HexTileType
{
    NORMAL,
    ROCK,
    LAVA,
    WALL,
};

public class HexTile {

    public int x;
    public int y;
    public Vector3 position;
    public HexTileType tileType;

    // Constructor
    public HexTile(int x, int y, Vector3 position, HexTileType type)
    {
        this.x = x;
        this.y = y;
        this.position = position;
        this.tileType = type;
    }
}
