using UnityEngine;
using System.Collections.Generic;
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

    int xWorldCenter;
    int yWorldCenter;

    public GameObject line;

    public GameObject[] mapTiles;
    public GameObject[,] mapTileObject;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter; // probably not needed?

    public void MapSetup()
    {
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;

        InitializeGameBoard();
        //Debug.Log("-InitializeGameBoard");
        InitializeNodes();
        //Debug.Log("-InitializeNodes");
    }

    // not a very nice version of map generation. CURRENTLY NOT USED
    void InitializeGameBoardV2()
    {
        mapTileObject = new GameObject[columns, rows];

        Transform mapHolder = new GameObject("Map").transform; // parent
        GameObject toInstantiate;
        float xTemp, yTemp;

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

    // Set up an empty game board
    void InitializeGameBoard()
    {
        mapTileObject = new GameObject[columns, rows];

        Transform mapHolder = new GameObject("Map").transform; // parent
        GameObject toInstantiate;
        float xTemp, yTemp;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 tileLocation;

                // find place
                xTemp = (float)(x * multiplicationUnit / 2 - y * multiplicationUnit / 2);
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

                // take a TILE, check its BOTTOM (y-1) EXIT and LEFT (x-1) EXIT and if there is a touching TILE, access the NODES, ADD to the "nodesConnected" 
                if (mapTileObject[x, y].GetComponent<MapTile>().exit3A && x > 0)
                {
                    mapTileObject[x, y].GetComponent<MapTile>().exit3A.GetComponent<MapNode>().nodesConnected.Add(mapTileObject[x - 1, y].GetComponent<MapTile>().exit1A);
                }
                if (mapTileObject[x, y].GetComponent<MapTile>().exit4A && y > 0)
                {
                    mapTileObject[x, y].GetComponent<MapTile>().exit4A.GetComponent<MapNode>().nodesConnected.Add(mapTileObject[x, y - 1].GetComponent<MapTile>().exit2A);
                }
            }
        }
        // note this might create a "double line" glitch, which may or may not be visible. to fix, run initialize node function BEFORE this component
        // if this does eventually cause visual problems with added graphics, add this segment below InitializeNodes
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // take a TILE, check its TOP (y+1) EXIT and RIGHT (x+1) EXIT and if there is a touching TILE, access the NODES, ADD to the "nodesConnected" 
                if (y < rows - 1 && mapTileObject[x, y].GetComponent<MapTile>().exit2A && mapTileObject[x, y + 1].GetComponent<MapTile>().exit4A)
                {
                    mapTileObject[x, y].GetComponent<MapTile>().exit2A.GetComponent<MapNode>().nodesConnected.Add(mapTileObject[x, y + 1].GetComponent<MapTile>().exit4A);
                }
                if (x < columns - 1 && mapTileObject[x, y].GetComponent<MapTile>().exit1A && mapTileObject[x + 1, y].GetComponent<MapTile>().exit3A)
                {
                    mapTileObject[x, y].GetComponent<MapTile>().exit1A.GetComponent<MapNode>().nodesConnected.Add(mapTileObject[x + 1, y].GetComponent<MapTile>().exit3A);
                }
            }
        }
    }
    
    void InitializeNodes()
    {
        List<GameObject> childNodeList = new List<GameObject>();
        GameObject instance;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                childNodeList.Clear();
                foreach (Transform t in mapTileObject[x, y].transform)
                {
                    childNodeList.Add(t.gameObject);
                }

                for (int i = 0; i < childNodeList.Count; i++)
                {
                    for (int u = 0; u < childNodeList[i].GetComponent<MapNode>().nodesConnected.Count; u++)
                    {
                        // MAKE THIS INTO A REAL LINE EVENTUALLY
                        //if (childNodeList[i] && childNodeList[i].GetComponent<MapNode>().nodesConnected[u]) // WOWOAOWOAWOOAWOWO DOUBLE CHECK THIS
                        {
                            Debug.DrawLine(childNodeList[i].transform.position, childNodeList[i].GetComponent<MapNode>().nodesConnected[u].transform.position, Color.green, 1000, false);

                            instance = Instantiate(line, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
                            instance.GetComponent<DrawLine>().start = childNodeList[i].transform.position;
                            instance.GetComponent<DrawLine>().stop = childNodeList[i].GetComponent<MapNode>().nodesConnected[u].transform.position;
                        }
                    }
                }
            }

            // look at all initialized MapTiles, scan their all registered node array, go through each node and connect them.
            // send player to the starting node, set the node around plaer to "clickable"
        }
    }
}
