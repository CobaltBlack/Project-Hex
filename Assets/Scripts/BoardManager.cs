using UnityEngine;
using System; // enables [serializable] attribute... allows us to modify how variables appear in the inspector in the editor
using System.Collections.Generic; // Adding Generic enables lists
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    //[Serializable] // (allows us to embed a class with sub properties in the inspector)
    public class MapHex
    {
        public float x; // TRUE x
        public float y; // TRUE y
        public int property;

        //public bool player;
        //public bool traversable;
        public bool visited = false;

        public MapHex(float xCoordinate, float yCoordinate, int hexProperty) // Constructor
        {
            x = xCoordinate;
            y = yCoordinate;
            property = hexProperty;

            //player = isPlayer; // can be kept track of by GameManager
            //traversable = isTraversable; // can be kept track of by using property
            //visited = isVisited;
        }
    }



    static int columns = 19; // Gameboard dimension - columns
    static int rows = 19; // Gameboard dimension - rows


    public int xWorldCenter;
    public int yWorldCenter;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter;

    void Awake()
    {
        // holds WORLD coordinate
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;
    }

    public GameObject[] hexRooms;

    public MapHex[,] gameBoard = new MapHex[columns, rows]; // removing static status from columns and rows will break this

    public void BoardSetup()
    {
        Transform hexHolder = new GameObject("Board").transform; // parent
        GameObject instance;
        float xTemp, yTemp;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // randomly choose room toInstantiate
                int hexChoice = Random.Range(0, hexRooms.Length);
                GameObject toInstantiate = hexRooms[hexChoice]; // Choose blank tile

                // create walls, probably make HexRooms[0] TO THE INTRAVERSABLE WALL

                // create stores, exit, quests, etc


                // hex protocol
                Vector3 newLocation;
                if (x % 2 == 0) // if even
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = y;
                    newLocation = new Vector3(xTemp, yTemp, 0f);
                }
                else // if odd
                {
                    xTemp = (float)(x * 1.5);
                    yTemp = (float)(y + 0.5);
                    newLocation = new Vector3(xTemp, yTemp, 0f);
                }

                // save center vector3
                if (x == xWorldCenter && y == yWorldCenter)
                {
                    trueCenter = newLocation;
                }

                // instantiation
                instance = Instantiate(toInstantiate, newLocation, Quaternion.identity) as GameObject;

                // editing GameObject
                instance.transform.SetParent(hexHolder);  // parent under hexHolder "Map"
                instance.name = "hex_" + x + "_" + y; // name the hexes by coordinates

                // logging
                gameBoard[x, y] = new MapHex(xTemp, yTemp, hexChoice); // log info to gameBoard
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {
	
	}
}
