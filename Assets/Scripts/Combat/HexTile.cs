using UnityEngine;
using System.Collections;

public enum TileType
{
    NORMAL,
    ROCK,
    LAVA,
};

public class HexTile {

    public int x;
    public int y;
    public Vector3 position;
    public TileType type;

    // Constructor
    public HexTile()
    {

    }

    
}
