using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    CombatBoardManager boardScript;

    public GameObject playerObject;
    GameObject playerInstance;

    // Game related player stats
    public int positionX;
    public int positionY;

    public int speed = 10;
    public int turnInterval = 100;

    // Instantiates player object at {x, y}
    public void InstantiatePlayer(int playerInitialX, int playerInitialY)
    {
        boardScript = GetComponent<CombatBoardManager>();
        playerInstance = Instantiate(playerObject, boardScript.GetHexPosition(playerInitialX, playerInitialY), Quaternion.identity) as GameObject;
        positionX = playerInitialX;
        positionY = playerInitialY;
    }

    // Moves player object to tile at {x, y}
    public void MovePlayer(int x, int y)
    {
        playerInstance.transform.position = boardScript.GetHexPosition(x, y);
        positionX = x;
        positionY = y;
    }
}
