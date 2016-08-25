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

    public GameObject[] mapRuleBooks;
    public GameObject[] mapTiles;
    public GameObject[,] mapTileGameObjects;

    // holds TRUE coordinate
    [HideInInspector]
    public Vector3 trueCenter; // probably not needed?

    public void MapSetup()
    {
        xWorldCenter = columns / 2;
        yWorldCenter = rows / 2;

        InitializeGameBoardV4();
        //Debug.Log("-InitializeGameBoard");
        InitializeNodes();
        //Debug.Log("-InitializeNodes");
    }

    // not a very nice version of map generation. CURRENTLY NOT USED
    void InitializeGameBoardV2()
    {
        mapTileGameObjects = new GameObject[columns, rows];

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
                    xTemp = (float)(x * multiplicationUnit / 2); // multiplicationUnit = 16
                    yTemp = (float)(y * multiplicationUnit);
                    tileLocation = new Vector3(xTemp, yTemp, 0f);
                }
                else // if odd
                {
                    xTemp = (float)(x * multiplicationUnit / 2); // multiplicationUnit = 16
                    yTemp = (float)(y * multiplicationUnit + multiplicationUnit / 2); // multiplicationUnit = 16
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
                mapTileGameObjects[x, y] = Instantiate(toInstantiate, tileLocation, Quaternion.identity) as GameObject;

                // editing GameObject
                mapTileGameObjects[x, y].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
                mapTileGameObjects[x, y].name = "map_" + x + "_" + y; // name the hexes by coordinates
            }
        }
    }

    /*
    public List<GameObject> building_4Exit;
    public List<GameObject> building_0Exit;

    public List<GameObject> building_1Exit_N;
    public List<GameObject> building_1Exit_W;
    public List<GameObject> building_1Exit_S;
    public List<GameObject> building_1Exit_E;

    public List<GameObject> building_2Exit_NW;
    public List<GameObject> building_2Exit_WS;
    public List<GameObject> building_2Exit_SE;
    public List<GameObject> building_2Exit_EN;

    public List<GameObject> building_2Exit_NS;
    public List<GameObject> building_2Exit_WE;

    public List<GameObject> building_3Exit_NWS;
    public List<GameObject> building_3Exit_WSE;
    public List<GameObject> building_3Exit_SEN;
    public List<GameObject> building_3Exit_ENW;


    public List<GameObject> garden_4Exit;
    public List<GameObject> garden_0Exit;

    public List<GameObject> garden_1Exit_N;
    public List<GameObject> garden_1Exit_W;
    public List<GameObject> garden_1Exit_S;
    public List<GameObject> garden_1Exit_E;

    public List<GameObject> garden_2Exit_NW;
    public List<GameObject> garden_2Exit_WS;
    public List<GameObject> garden_2Exit_SE;
    public List<GameObject> garden_2Exit_EN;

    public List<GameObject> garden_2Exit_NS;
    public List<GameObject> garden_2Exit_WE;

    public List<GameObject> garden_3Exit_NWS;
    public List<GameObject> garden_3Exit_WSE;
    public List<GameObject> garden_3Exit_SEN;
    public List<GameObject> garden_3Exit_ENW;
    */

    Transform mapHolder; // parent

    void InitializeGameBoardV3()
    {
        mapHolder = new GameObject("Map").transform; // parent

        // Pick a random ruleBook
        GameObject mapChoice = Instantiate(mapRuleBooks[Random.Range(0, mapRuleBooks.Length)]) as GameObject;

        // Read mapData
        MapDataTemplate mapData = mapChoice.GetComponent<MapDataTemplate>();

        // Initialize map size
        mapTileGameObjects = new GameObject[mapData.columns, mapData.rows];

        // lay down the fixed coordinate properties in the MapData, record them in mapTileGameObjects
        int xOffSet = mapData.premade3x3OriginX;
        int yOffSet = mapData.premade3x3OriginY;

        PlaceGameObject(mapData.premade3x3[0, 0], 0 + xOffSet, 0 + yOffSet);
        PlaceGameObject(mapData.premade3x3[0, 1], 0 + xOffSet, 1 + yOffSet);
        PlaceGameObject(mapData.premade3x3[0, 2], 0 + xOffSet, 2 + yOffSet);

        PlaceGameObject(mapData.premade3x3[1, 0], 1 + xOffSet, 0 + yOffSet);
        PlaceGameObject(mapData.premade3x3[1, 1], 1 + xOffSet, 1 + yOffSet);
        PlaceGameObject(mapData.premade3x3[1, 2], 1 + xOffSet, 2 + yOffSet);

        PlaceGameObject(mapData.premade3x3[2, 0], 2 + xOffSet, 0 + yOffSet);
        PlaceGameObject(mapData.premade3x3[2, 1], 2 + xOffSet, 1 + yOffSet);
        PlaceGameObject(mapData.premade3x3[2, 2], 2 + xOffSet, 2 + yOffSet);

        for (int x = 0; x < mapData.columns; x++)
        {
            for (int y = 0; y < mapData.rows; y++)
            {
                // check if a tile is alraedy placed. if so, do nothing this loop.
                if (mapTileGameObjects[x, y])
                {
                    continue;
                }

                // reset tile requirements
                bool hasNorthExit = false;
                bool hasWestExit = false;
                bool hasSouthExit = false;
                bool hasEastExit = false;
                EnumTileProperty property = EnumTileProperty.Default;

                // check tile surrounding. note which exits MUST must match N1 W2 S3 E4

                // N (1) exit check
                if (x != mapData.columns - 1 && mapTileGameObjects[x + 1, y])
                {
                    // check if it has a S (3) entrance
                    if(mapTileGameObjects[x + 1, y].GetComponent<MapTile>().exit3A)
                    {
                        // since there is S (3) entrance, we must have a N (1) exit on our tile to match
                        hasNorthExit = true;
                    }
                }
                // if there are no tiles against this exit, add an exit randomly
                if(x != mapData.columns - 1 && !mapTileGameObjects[x + 1, y])
                {
                    bool randomBool = (Random.value < 0.5);
                    hasNorthExit = randomBool;
                }

                // W (2) check
                if (y != mapData.rows - 1 && mapTileGameObjects[x, y + 1])
                {
                    // check if it has a E (4) entrance
                    if (mapTileGameObjects[x, y + 1].GetComponent<MapTile>().exit4A)
                    {
                        // since there is E (4) entrance, we must have a W (2) exit on our tile to match
                        hasWestExit = true;
                    }
                }
                if (y != mapData.rows - 1 && !mapTileGameObjects[x, y + 1])
                {
                    bool randomBool = (Random.value < 0.5);
                    hasWestExit = randomBool;
                }

                // S (3) check
                if (!(x == 0) && mapTileGameObjects[x - 1, y])
                {
                    // check if it has a N (1) entrance
                    if (mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A)
                    {
                        // since there is N (1) entrance, we must have a S (3) exit on our tile to match
                        hasSouthExit = true;
                    }
                }
                if (!(x == 0) && !mapTileGameObjects[x - 1, y])
                {
                    bool randomBool = (Random.value < 0.5);
                    hasSouthExit = randomBool;
                }

                // E (4) check
                if (!(y == 0) && mapTileGameObjects[x, y - 1])
                {
                    // check if it has a W (2) entrance
                    if (mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A)
                    {
                        // since there is W (2) entrance, we must have a E (4) exit on our tile to match
                        hasEastExit = true;
                    }
                }
                if (!(y == 0) && !mapTileGameObjects[x, y - 1])
                {
                    bool randomBool = (Random.value < 0.5);
                    hasEastExit = randomBool;
                }

                // read this tiles zoning property
                if (mapData.building_xFrom <= x && x <= mapData.building_xTo && mapData.building_yFrom <= y && y <= mapData.building_yTo) // if x and y is within zoning property of garden
                {
                    // this tile should be building
                    property = EnumTileProperty.Building;
                }
                if (mapData.garden_xFrom <= x && x <= mapData.garden_xTo && mapData.garden_yFrom <= y && y <= mapData.garden_yTo) // if x and y is within zoning property of garden
                {
                    // this tile should be garden
                    property = EnumTileProperty.Garden;
                }

                // construct a list of valid tiles

                List<GameObject> validMapTiles = new List<GameObject>();

                validMapTiles.Clear();

                for (int i = 0; i < mapTiles.Length; i++)
                {
                    MapTile currentTile = mapTiles[i].GetComponent<MapTile>();

                    if (property == currentTile.property) // check property
                    {
                        if (hasNorthExit == currentTile.exit1A && hasWestExit == currentTile.exit2A && hasSouthExit == currentTile.exit3A && hasEastExit == currentTile.exit4A) // check exit
                        {
                            validMapTiles.Add(mapTiles[i]);
                        }
                    }
                }

                // select a random tile from the correct "exit" and "zone" pool.
                GameObject toInstantiate = validMapTiles[Random.Range(0, validMapTiles.Count)];

                // place tile
                PlaceGameObject(toInstantiate, x, y);

                // register all nodes correctly
                // currently not implemented

            }
        }
    }

    void PlaceGameObject(GameObject toInstantiate, int xCoordinate, int yCoordinate)
    {
        // find place
        float xTemp = (float)(xCoordinate * multiplicationUnit / 2 - yCoordinate * multiplicationUnit / 2);
        float yTemp = (float)(yCoordinate * multiplicationUnit / 2 + xCoordinate * multiplicationUnit / 2);
        Vector3 tileLocation = new Vector3(xTemp, yTemp, 0f);

        // instantiation
        mapTileGameObjects[xCoordinate, yCoordinate] = Instantiate(toInstantiate, tileLocation, toInstantiate.transform.rotation) as GameObject;

        // editing GameObject
        mapTileGameObjects[xCoordinate, yCoordinate].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
        mapTileGameObjects[xCoordinate, yCoordinate].name = "map_" + xCoordinate + "_" + yCoordinate; // name the hexes by coordinates
    }

    // Used to bundle GameObject and coordinates which accompany them for the toEpand LIST
    struct ToExpandItem
    {
        public GameObject toExpandItem;
        public int x;
        public int y;

        public ToExpandItem(GameObject toExpandItem, int x, int y)
        {
            this.toExpandItem = toExpandItem;
            this.x = x;
            this.y = y;
        }
    }

    public GameObject blankTile;

    // List of items which needs "expansion"
    List<ToExpandItem> toExpand = new List<ToExpandItem>();

    void InitializeGameBoardV4()
    {
        mapHolder = new GameObject("Map").transform; // hierarchy parent

        // Pick a random ruleBook
        GameObject mapChoice = Instantiate(mapRuleBooks[Random.Range(0, mapRuleBooks.Length)]) as GameObject;

        // Read mapData
        MapDataTemplate mapData = mapChoice.GetComponent<MapDataTemplate>();

        // Initialize map size
        mapTileGameObjects = new GameObject[mapData.columns, mapData.rows];

        // lay down the fixed coordinate properties in the MapData, record them in mapTileGameObjects
        PlacePremadeTiles(mapData);

        while(toExpand.Count != 0)
        {
            ToExpandItem currentItem = toExpand[0];

            // access current X and Y coordinates
            int x = currentItem.x;
            int y = currentItem.y;

            // check if tile NORTH of this tile exists, if so, take that tile and execute the check surrounding protocol
            if (x != mapData.columns - 1 && !mapTileGameObjects[x + 1, y])
            {
                x += 1;
            }
            // check if tile WEST
            else if (y != mapData.rows - 1 && !mapTileGameObjects[x, y + 1])
            {
                y += 1;
            }
            // check if tile SOURTH
            else if (!(x == 0) && !mapTileGameObjects[x - 1, y])
            {
                x -= 1;
            }
            // check if tile EAST
            else if (!(y == 0) && !mapTileGameObjects[x, y - 1])
            {
                y -= 1;
            }
            // if there are no available tiles surrounding thist aile, end loop, remove item - continue;
            else
            {
                toExpand.Remove(toExpand[0]);
                continue;
            }

            // check tile surrounding. note which exits MUST must match N1 W2 S3 E4

            // reset tile requirement data
            bool hasNorthExit = false; bool hasWestExit = false; bool hasSouthExit = false; bool hasEastExit = false;
            bool noExit = true;
            EnumTileProperty property = EnumTileProperty.Default;

            // N (1) exit check
            if (x != mapData.columns - 1 && mapTileGameObjects[x + 1, y])
            {
                // check if it has a S (3) entrance
                if (mapTileGameObjects[x + 1, y].GetComponent<MapTile>().exit3A)
                {
                    // since there is S (3) entrance, we must have a N (1) exit on our tile to match
                    hasNorthExit = true;
                    noExit = false;
                }
            }
            // if there are no tiles against this exit, add an exit randomly
            if (x != mapData.columns - 1 && !mapTileGameObjects[x + 1, y])
            {
                bool randomBool = (Random.value < 0.5);
                hasNorthExit = randomBool;
            }

            // W (2) check
            if (y != mapData.rows - 1 && mapTileGameObjects[x, y + 1])
            {
                // check if it has a E (4) entrance
                if (mapTileGameObjects[x, y + 1].GetComponent<MapTile>().exit4A)
                {
                    // since there is E (4) entrance, we must have a W (2) exit on our tile to match
                    hasWestExit = true;
                    noExit = false;
                }
            }
            if (y != mapData.rows - 1 && !mapTileGameObjects[x, y + 1])
            {
                bool randomBool = (Random.value < 0.5);
                hasWestExit = randomBool;
            }

            // S (3) check
            if (!(x == 0) && mapTileGameObjects[x - 1, y])
            {
                // check if it has a N (1) entrance
                if (mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A)
                {
                    // since there is N (1) entrance, we must have a S (3) exit on our tile to match
                    hasSouthExit = true;
                    noExit = false;
                }
            }
            if (!(x == 0) && !mapTileGameObjects[x - 1, y])
            {
                bool randomBool = (Random.value < 0.5);
                hasSouthExit = randomBool;
            }

            // E (4) check
            if (!(y == 0) && mapTileGameObjects[x, y - 1])
            {
                // check if it has a W (2) entrance
                if (mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A)
                {
                    // since there is W (2) entrance, we must have a E (4) exit on our tile to match
                    hasEastExit = true;
                    noExit = false;
                }
            }
            if (!(y == 0) && !mapTileGameObjects[x, y - 1])
            {
                bool randomBool = (Random.value < 0.5);
                hasEastExit = randomBool;
            }

            // place a blank tile against dead ends
            if (noExit == true)
            {
                // needs to move onto checking the other sides, or else will be stuck in infinite loop
                PlaceGameObject(blankTile, x, y);

                continue;
            }

            // read tiles zoning property
            if (mapData.building_xFrom <= x && x <= mapData.building_xTo && mapData.building_yFrom <= y && y <= mapData.building_yTo) // if x and y is within zoning property of garden
            {
                // this tile should be building
                property = EnumTileProperty.Building;
            }
            if (mapData.garden_xFrom <= x && x <= mapData.garden_xTo && mapData.garden_yFrom <= y && y <= mapData.garden_yTo) // if x and y is within zoning property of garden
            {
                // this tile should be garden
                property = EnumTileProperty.Garden;
            }

            // construct a list of valid tiles with correct "exit" and "zone" pool
            List<GameObject> validMapTiles = ConstructValidMapTilesList(property, hasNorthExit, hasWestExit, hasSouthExit, hasEastExit);

            // select a random tile from the pool
            GameObject toInstantiate = validMapTiles[Random.Range(0, validMapTiles.Count)];

            // place tile
            PlaceGameObjectV2(toInstantiate, x, y);

            // register all nodes correctly
            // currently not implemented
        }
    }

    void PlaceGameObjectV2(GameObject toInstantiate, int xCoordinate, int yCoordinate)
    {
        // V2 has the additional task of ADDING instantiated tiles into toExpand LIST

        // find place
        float xTemp = (float)(xCoordinate * multiplicationUnit / 2 - yCoordinate * multiplicationUnit / 2);
        float yTemp = (float)(yCoordinate * multiplicationUnit / 2 + xCoordinate * multiplicationUnit / 2);
        Vector3 tileLocation = new Vector3(xTemp, yTemp, 0f);

        // instantiation
        mapTileGameObjects[xCoordinate, yCoordinate] = Instantiate(toInstantiate, tileLocation, toInstantiate.transform.rotation) as GameObject;

        // editing GameObject
        mapTileGameObjects[xCoordinate, yCoordinate].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
        mapTileGameObjects[xCoordinate, yCoordinate].name = "map_" + xCoordinate + "_" + yCoordinate; // name the hexes by coordinates

        // add to List
        ToExpandItem item = new ToExpandItem(mapTileGameObjects[xCoordinate, yCoordinate], xCoordinate, yCoordinate);
        toExpand.Add(item);
    }

    List<GameObject> ConstructValidMapTilesList(EnumTileProperty property, bool hasNorthExit, bool hasWestExit, bool hasSouthExit, bool hasEastExit)
    {
        List<GameObject> validMapTiles = new List<GameObject>();
        validMapTiles.Clear();

        for (int i = 0; i < mapTiles.Length; i++)
        {
            MapTile currentTile = mapTiles[i].GetComponent<MapTile>();

            if (property == currentTile.property) // check property
            {
                if (hasNorthExit == currentTile.exit1A && hasWestExit == currentTile.exit2A && hasSouthExit == currentTile.exit3A && hasEastExit == currentTile.exit4A) // check exit
                {
                    validMapTiles.Add(mapTiles[i]);
                }
            }
        }

        return validMapTiles;
    }

    void PlacePremadeTiles(MapDataTemplate mapData)
    {
        // Place premade 3x3
        int xOffSet = mapData.premade3x3OriginX;
        int yOffSet = mapData.premade3x3OriginY;

        PlaceGameObjectV2(mapData.premade3x3[0, 0], 0 + xOffSet, 0 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[0, 1], 0 + xOffSet, 1 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[0, 2], 0 + xOffSet, 2 + yOffSet);

        PlaceGameObjectV2(mapData.premade3x3[1, 0], 1 + xOffSet, 0 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[1, 1], 1 + xOffSet, 1 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[1, 2], 1 + xOffSet, 2 + yOffSet);

        PlaceGameObjectV2(mapData.premade3x3[2, 0], 2 + xOffSet, 0 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[2, 1], 2 + xOffSet, 1 + yOffSet);
        PlaceGameObjectV2(mapData.premade3x3[2, 2], 2 + xOffSet, 2 + yOffSet);
    }

    // Set up an empty game board
    void InitializeGameBoard()
    {
        mapTileGameObjects = new GameObject[columns, rows];

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
                mapTileGameObjects[x, y] = Instantiate(toInstantiate, tileLocation, toInstantiate.transform.rotation) as GameObject;

                // editing GameObject
                mapTileGameObjects[x, y].transform.SetParent(mapHolder);  // parent under hexHolder "Map"
                mapTileGameObjects[x, y].name = "map_" + x + "_" + y; // name the hexes by coordinates

                // take a TILE, check its BOTTOM (y-1) EXIT and LEFT (x-1) EXIT and if there is a touching TILE, access the NODES, ADD to the "nodesConnected" 
                if (mapTileGameObjects[x, y].GetComponent<MapTile>().exit3A && x > 0)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit3A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A);
                }
                if (mapTileGameObjects[x, y].GetComponent<MapTile>().exit4A && y > 0)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit4A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A);
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
                if (y < rows - 1 && mapTileGameObjects[x, y].GetComponent<MapTile>().exit2A && mapTileGameObjects[x, y + 1].GetComponent<MapTile>().exit4A)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit2A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x, y + 1].GetComponent<MapTile>().exit4A);
                }
                if (x < columns - 1 && mapTileGameObjects[x, y].GetComponent<MapTile>().exit1A && mapTileGameObjects[x + 1, y].GetComponent<MapTile>().exit3A)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit1A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x + 1, y].GetComponent<MapTile>().exit3A);
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
                foreach (Transform t in mapTileGameObjects[x, y].transform)
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
