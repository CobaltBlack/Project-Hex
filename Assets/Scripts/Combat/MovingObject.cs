using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

    public int currentHealth;
    public int maxHealth;

    public int positionX;
    public int positionY;

    public int speed;

    // Move object to position
    public void moveToPosition(HexTile target)
    {
        gameObject.transform.position = target.position;
    }
}
