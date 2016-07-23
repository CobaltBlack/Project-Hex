using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

    public int currentHealth;
    public int maxHealth;

    public int positionX;
    public int positionY;

    public int speed;

    // Returns an array of coordinates that represents
    // the path to coordinate (x, y)
    public HexTile[] moveToPosition(HexTile target)
    {
        HexTile[] path = { new HexTile() };


        return path;
    }
}
