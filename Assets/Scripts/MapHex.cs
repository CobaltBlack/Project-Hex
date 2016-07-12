using UnityEngine;
using System.Collections;

public enum HexType
{
    NORMAL,
    WALL,
    STORE,
    EXIT,
};

public class MapHex
{
    public Vector3 position;
    public HexType property;
    public bool visited;

    public MapHex(Vector3 position, HexType property) // Constructor
    {
        this.position = position;
        this.property = property;
        this.visited = false;
    }
}