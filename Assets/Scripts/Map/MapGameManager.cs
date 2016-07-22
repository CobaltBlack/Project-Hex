using UnityEngine;
using System.Collections;

public class MapGameManager : MonoBehaviour
{
    MapManager mapScript;
    InstanceManager instanceScript;

    public GameObject player;
    public GameObject playerInstance;

    public static MapGameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // gameObject refers to game object this component is attached to // NOTE IS THERE A BUG? NO RETURN?
        }

        DontDestroyOnLoad(gameObject); // To preserve game data such as score between stages

        mapScript = GetComponent<MapManager>();
        instanceScript = GetComponent<InstanceManager>();
        InitializeGame();
    }

    public void InitializeGame()
    {
        // set up board
        //Debug.Log("Initialize gameboard");
        mapScript.MapSetup();

        // initialize player
        //Debug.Log("Instantiate player");
        //playerInstance = Instantiate(player, mapScript.GetHexPosition(playerInitialX, playerInitialY), Quaternion.identity) as GameObject;

        //Debug.Log("Initialize game complete!");
    }
}
