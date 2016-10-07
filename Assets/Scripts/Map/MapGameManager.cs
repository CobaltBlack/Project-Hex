using UnityEngine;
using System.Collections;

public class MapGameManager : MonoBehaviour
{
    MapManager mapManagerScript;
    InstanceManager instanceManagerScript;
    PlayerManager playerManagerScript;

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

        mapManagerScript = GetComponent<MapManager>();

        instanceManagerScript = GetComponent<InstanceManager>();

        playerManagerScript = GetComponent<PlayerManager>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        // set up board
        Debug.Log("Initialize gameboard");
        mapManagerScript.MapSetup();

        // initialize player
        Debug.Log("Instantiate player");
        playerInstance = Instantiate(player) as GameObject;

        // set player
        newNode = mapManagerScript.mapTileGameObjects[10, 10].GetComponent<MapTile>().startingNode; //temporary place holder starting location
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
        mapManagerScript.ConnectNodes(newNode);

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

        // GLITCHED - it will have one extra processing caused by InitializeGame
        // fluctuations in MP SP
        playerManagerScript.ProcessFlux();
    }

    public void PlayInstance(Instance currentInstance)
    {
        instanceManagerScript.StartInstance(currentInstance);
    }
}
