using UnityEngine;
using System.Collections;

public class MapGameManager : MonoBehaviour
{
    MapManager mapManagerScript;
    InstanceManager instanceManagerScript;
    PlayerManager playerManagerScript;

    Mask maskScript;
    Shadow shadowScript;
    FadeAlphaCutoff fadeScript;

    MapMovingObject mapMovingObjectScript;

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

        maskScript = GetComponent<Mask>();
        shadowScript = GetComponent<Shadow>();
        fadeScript = GetComponent<FadeAlphaCutoff>();

        mapMovingObjectScript = GetComponent<MapMovingObject>();
    }

    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        // fade in map
        fadeScript.FadeMapIn();

        // wait
        yield return new WaitForSeconds(3);

        // set up board
        Debug.Log("Initialize gameboard");
        mapManagerScript.MapSetup();

        // set up player
        Debug.Log("Initialize player");
        InitializePlayer();

        // start shadow flicker
        Debug.Log("Initialize shadow");
        shadowScript.StartFlickerShadow();

        // initialization complete
        Debug.Log("Initialization complete!");
    }

    void InitializePlayer()
    {
        // initialize player GameObject
        playerInstance = Instantiate(player) as GameObject;

        // save starting location
        GameObject startingNode = mapManagerScript.mapTileGameObjects[10, 10].GetComponent<MapTile>().startingNode;

        // set starting location as visited
        startingNode.GetComponent<MapNode>().isVisited = true; 

        // set player at starting location
        playerInstance.transform.position = startingNode.transform.position;

        // unmask map
        maskScript.SpawnUnmaskCluster(startingNode.transform.position);

        // connect surrounding nodes visually, disconnect previous connections
        mapManagerScript.ConnectNodes(startingNode);

        // activate newNode's nodesConnected as clickable
        for (int i = 0; i < startingNode.GetComponent<MapNode>().nodesConnected.Count; i++)
        {
            startingNode.GetComponent<MapNode>().nodesConnected[i].GetComponent<MapNode>().isClickable = true;
        }

        // prep for MovePlayer
        newNode = startingNode;
    }

    public void MovePlayer(GameObject currentNode)
    {
        oldNode = newNode;
        newNode = currentNode;

        // smooth move player
        mapMovingObjectScript.MoveObject(playerInstance.transform, newNode.transform.position);

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

            if (!newNode.GetComponent<MapNode>().isVisited)
            {
                maskScript.SpawnUnmaskSingle(newNode.GetComponent<MapNode>().nodesConnected[i].transform.position);
            }
        }

        // calculate new MP SP
        playerManagerScript.ProcessFlux();
    }

    public void UnmaskArea(GameObject targetNode)
    {
        maskScript.SpawnUnmaskCluster(targetNode.transform.position);
    }

    public void PlayInstance(Instance currentInstance)
    {
        instanceManagerScript.StartInstance(currentInstance);
    }

    public bool PlayerIsMoving()
    {
        return mapMovingObjectScript.IsMoving();
    }
}
