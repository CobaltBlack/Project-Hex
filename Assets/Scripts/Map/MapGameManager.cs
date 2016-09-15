using UnityEngine;
using System.Collections;

public class MapGameManager : MonoBehaviour
{
    MapManager mapScript;
    InstanceManager instanceScript;

    public GameObject player;
    GameObject playerInstance;
    GameObject oldNode;
    GameObject newNode;

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

        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player) as GameObject;

        // set player
        newNode = mapScript.mapTileGameObjects[10, 10].GetComponent<MapTile>().startingNode; //temporary place holder starting location
        newNode.GetComponent<MapNode>().isVisited = true; // set start as visited
        MovePlayer(newNode);

        // initialization complete
        Debug.Log("Initialization complete!");
    }

    public void MovePlayer(GameObject currentNode)
    {
        oldNode = newNode;
        newNode = currentNode;

        // send playerInstance to this position
        playerInstance.transform.position = currentNode.transform.position;

        // set the node to visited // this functionality is moved to MapNode
        //currentNode.GetComponent<MapNode>().isVisited = true;

        // connect surrounding nodes visually, disconnect previous connections
        mapScript.ConnectNodes(newNode);

        // deactivate oldNode's nodesConnected
        for (int i = 0; i < oldNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            oldNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = false;
        }
        // activate newNode's nodesConnected
        for (int i = 0; i < newNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            newNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = true;
        }
    }

    public void PlayInstance(Instance currentInstance)
    {
        instanceScript.StartInstance(currentInstance);
    }
}
