using UnityEngine;
using System.Collections;

public class MapGameManager : MonoBehaviour
{
    MapManager mapScript;
    InstanceManager instanceScript;

    public GameObject player;
    GameObject playerInstance;

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
        Debug.Log("Initialize gameboard");
        mapScript.MapSetup();

        // save newNode
        newNode = mapScript.mapTileObject[0, 0].GetComponent<MapTile>().startingNode;

        // SMART METHOD
        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        GameObject origin = mapScript.mapTileObject[0, 0].GetComponent<MapTile>().startingNode;
        origin.GetComponent<MapNode>().isVisited = true; // this is in place for preperation for addition of instances
        MovePlayer(origin);

        Debug.Log("Initialize game complete!");

        /* MANUAL METHOD
        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player, mapScript.mapTileObject[0,0].GetComponent<MapTile>().startingNode.transform.position , Quaternion.identity) as GameObject;

        // set the tile player starts on as visited
        mapScript.mapTileObject[0, 0].GetComponent<MapTile>().startingNode.GetComponent<MapNode>().isVisited = true;

        // make nodes around player clickable
        for (int i = 0; i < mapScript.mapTileObject[0, 0].GetComponent<MapTile>().startingNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            mapScript.mapTileObject[0, 0].GetComponent<MapTile>().startingNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = true;
        }

        Debug.Log("Initialize game complete!");
        */
    }

    GameObject oldNode;
    GameObject newNode;

    public void MovePlayer(GameObject currentNode)
    {
        oldNode = newNode;
        newNode = currentNode;
        playerInstance.transform.position = currentNode.transform.position; // send player instance to this position

        currentNode.GetComponent<MapNode>().isVisited = true; // set the tile to visited

        // deactivate oldNode's nodesConnected, activate newNode's nodesConnected
        for (int i = 0; i < oldNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            oldNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = false;
        }

        for (int i = 0; i < newNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            newNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = true;
        }
    }
}
