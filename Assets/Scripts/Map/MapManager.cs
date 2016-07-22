using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

/*
 * MapManager
 * 
 * This script is in charge of initializing the map.
 * Also has functions to get various information about the state of the board.
 *  
 */

public class MapManager : MonoBehaviour
{
    public int multiplicationUnit = 16;

    public int columns; // Gameboard dimension - columns
    public int rows; // Gameboard dimension - rows

    public int xWorldCenter;
    public int yWorldCenter;

    public GameObject[] mapTiles;
    public GameObject[,] mapTileObject;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter; // probably not needed?

    public void MapSetup()
    {
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;

        InitializeGameBoardV2();
        Debug.Log("InitializeGameBoardV2 : complete");
        InitializeNodes();
        Debug.Log("InitializeNodes : complete");

        // testing if line debug works
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(100, 100, 0),Color.green, 100, false);
    }

    // Set up an empty game board
    void InitializeGameBoard()
    {
        float xTemp, yTemp;

        mapTileObject = new GameObject[columns, rows];

        // parent
        Transform mapHolder = new GameObject("Map").transform;
        GameObject toInstantiate;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x % 2 == 1 && y == rows - 1) // makes the map look a little nicer in certain cases
                {
                    break;
                }

                Vector3 tileLocation;

                if (x % 2 == 0) // if even
                {
                    xTemp = (float)(x * multiplicationUnit/2); // multiplicationUnit = 16
                    yTemp = (float)(y * multiplicationUnit);
                    tileLocation = new Vector3(xTemp, yTemp, 0f);
                }
                else // if odd
                {
                    xTemp = (float)(x * multiplicationUnit/2); // multiplicationUnit = 16
                    yTemp = (float)(y * multiplicationUnit + multiplicationUnit/2); // multiplicationUnit = 16
                    tileLocation = new Vector3(xTemp, yTemp, 0f);
                }

                // save center vector3
                if (x == xWorldCenter && y == yWorldCenter)
                {
                    trueCenter = tileLocation;
                }

                // randomly choose tile
                int tileChoice = Random.Range(0, mapTiles.Length);
                toInstantiate = mapTiles[tileChoice]; // Choose blank tile

                // instantiation
                mapTileObject[x, y] = Instantiate(toInstantiate, tileLocation, Quaternion.identity) as GameObject;

                // editing GameObject
                mapTileObject[x, y].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
                mapTileObject[x, y].name = "map_" + x + "_" + y; // name the hexes by coordinates
            }
        }
    }

    // Set up an empty game board V2
    void InitializeGameBoardV2()
    {
        float xTemp, yTemp;

        mapTileObject = new GameObject[columns, rows];

        // parent
        Transform mapHolder = new GameObject("Map").transform;
        GameObject toInstantiate;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 tileLocation;

                // find place
                xTemp = (float)(x * multiplicationUnit / 2 - y * multiplicationUnit / 2); // multiplicationUnit = 16
                yTemp = (float)(y * multiplicationUnit / 2 + x * multiplicationUnit / 2);
                tileLocation = new Vector3(xTemp, yTemp, 0f);

                // save center vector3
                if (x == xWorldCenter && y == yWorldCenter)
                {
                    trueCenter = tileLocation;
                }

                // randomly choose tile
                int tileChoice = Random.Range(0, mapTiles.Length);
                toInstantiate = mapTiles[tileChoice]; // Choose blank tile

                // instantiation
                mapTileObject[x, y] = Instantiate(toInstantiate, tileLocation, Quaternion.identity) as GameObject;

                // editing GameObject
                mapTileObject[x, y].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
                mapTileObject[x, y].name = "map_" + x + "_" + y; // name the hexes by coordinates
            }
        }
    }

    void InitializeNodes()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int i = 0; i < mapTileObject[x, y].GetComponent<MapTile>().nodes.Length; i++)
                {

                    // connect all nodes in mapTileObject[x, y] node array
                    
                    Debug.DrawLine(mapTileObject[x, y].GetComponent<MapTile>().nodes[i].transform.position, mapTileObject[x, y].GetComponent<MapTile>().nodes[+1].transform.position,Color.green, 100, false);
                    Debug.Log("lines have been drawn");

                    // look at mapTileObject[x, y].exit1A, match it with mapTileObject[x + 1, y].exit3A ... so on

                    // if node with player on, make surrounding nodes clickkable

                    mapTileObject[x, y] = mapTileObject[x, y];
                }
            }

            // look at all initialized MapTiles, scan their all registered node array, go through each node and connect them.
            // send player to the starting node, set the node around plaer to "clickable"
        }
    }

    void LookUp(int x, int y)
    {

    }

    void Lookright()
    {

    }
}
