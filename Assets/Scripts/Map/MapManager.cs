using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    // change this value to fit the tile dimension
    public int multiplicationUnit = 16;

    public GameObject[] mapRuleBooks;
    public GameObject[] mapTiles;
    public GameObject[,] mapTileGameObjects;
    public GameObject blankTile;
    public GameObject line;

    Transform mapHolder;
    List<Vector2> toExpand;

    // categorical tile storage, currently no use. Keep it collapsed
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

    public void MapSetup()
    {
        InitializeGameBoard();
    }

    // main procedural generation process
    void InitializeGameBoard()
    {
        mapHolder = new GameObject("Map").transform; // parent
        toExpand = new List<Vector2>(); // list of tile locations which needs "expansion"

        // pick a random ruleBook
        GameObject mapChoice = mapRuleBooks[Random.Range(0, mapRuleBooks.Length)];

        // read mapData
        MapDataTemplate mapData = mapChoice.GetComponent<MapDataTemplate>();

        // initialize mapTileGameObjects array according to mapData columns and rows
        mapTileGameObjects = new GameObject[mapData.columns, mapData.rows];

        // lay down all premade GameObjects according to mapData, record them in mapTileGameObjects, add to toExpand list
        PlacePremadeTiles(mapData);

        while(toExpand.Count != 0)
        {
            // read first tile location in toExpand list
            Vector2 currentItem = toExpand[0];

            // X and Y coordinates of currentItem
            int x = (int)currentItem.x;
            int y = (int)currentItem.y;

            // look for empty space surrounding currentItem

            // check if tile NORTH of this tile exists, if it doesnt, take that tile location and execute the check surrounding protocol
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

            // check tile surrounding, note which exits MUST must match N1 W2 S3 E4

            // reset tile requirement data
            bool hasNorthExit = false; bool hasWestExit = false; bool hasSouthExit = false; bool hasEastExit = false;
            bool noExit = true;

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
            // if there are no tiles against this exit, add an exit randomly or using MATH
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
            // if there are no tiles against this exit, add an exit randomly or using MATH
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
            // if there are no tiles against this exit, add an exit randomly or using MATH
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
            // if there are no tiles against this exit, add an exit randomly or using MATH
            if (!(y == 0) && !mapTileGameObjects[x, y - 1])
            {
                bool randomBool = (Random.value < 0.5);
                hasEastExit = randomBool;
            }

            // place a blank tile against dead ends
            if (noExit == true)
            {
                // must move onto checking the other sides, or in the case of this fix, lay down a "blank tile" to fill the space
                PlaceGameObject(blankTile, x, y);
                continue;
            }

            // read tiles zoning property
            EnumTileProperty property = ZoningProperty(mapData, x, y);

            // construct a list of valid tiles with correct "exit" and "zone" pool
            List<GameObject> validMapTiles = ConstructValidMapTilesList(property, hasNorthExit, hasWestExit, hasSouthExit, hasEastExit);

            // select a random tile from the pool
            GameObject toInstantiate = validMapTiles[Random.Range(0, validMapTiles.Count)];

            // place tile
            PlaceGameObjectAddToList(toInstantiate, x, y);
        }
        // register all nodes correctly
        ConnectExitNodes(mapData.columns, mapData.rows);

        // draw visual lines between nodes
        //InitializeNodes(mapData.columns, mapData.rows);
    }

    // place object in location provided with GameObject provided
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

    // place object in location provided with GameObject provided, and add instantiated tile into toExpand list
    void PlaceGameObjectAddToList(GameObject toInstantiate, int xCoordinate, int yCoordinate)
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

        // add to List coordinates which need expansion
        toExpand.Add(new Vector2(xCoordinate, yCoordinate));
    }

    // given the info surrounding a tile, return a list of acceptable tiles
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

    // given the MapDataTemplate, lay down all premade GameObjects
    void PlacePremadeTiles(MapDataTemplate mapData)
    {
        // Place premade 3x3
        int xOffSet = mapData.premade3x3OriginX;
        int yOffSet = mapData.premade3x3OriginY;

        PlaceGameObjectAddToList(mapData.tile3x3_0_0, 0 + xOffSet, 0 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_0_1, 0 + xOffSet, 1 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_0_2, 0 + xOffSet, 2 + yOffSet);

        PlaceGameObjectAddToList(mapData.tile3x3_1_0, 1 + xOffSet, 0 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_1_1, 1 + xOffSet, 1 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_1_2, 1 + xOffSet, 2 + yOffSet);

        PlaceGameObjectAddToList(mapData.tile3x3_2_0, 2 + xOffSet, 0 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_2_1, 2 + xOffSet, 1 + yOffSet);
        PlaceGameObjectAddToList(mapData.tile3x3_2_2, 2 + xOffSet, 2 + yOffSet);
    }

    // read tiles zoning property given current location
    EnumTileProperty ZoningProperty(MapDataTemplate mapData, int x, int y)
    {
        if (mapData.building_xFrom <= x && x <= mapData.building_xTo && mapData.building_yFrom <= y && y <= mapData.building_yTo) // if x and y is within zoning property of garden
        {
            // this tile should be building
            return EnumTileProperty.Building;
        }
        if (mapData.garden_xFrom <= x && x <= mapData.garden_xTo && mapData.garden_yFrom <= y && y <= mapData.garden_yTo) // if x and y is within zoning property of garden
        {
            // this tile should be garden
            return EnumTileProperty.Garden;
        }
        Debug.Log("Error may have occurred - check ZoningProperty under MapManager");
        return EnumTileProperty.Default;
    }

    // connect all exit nodes together
    void ConnectExitNodes(int columns, int rows)
    {
        // loop through all tiles, for each tile, check and add South and East only
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // check 1. if out of bound, 2. current tile exists, 3. current tile South exit exists, 4. corresponding tile exists, 5. corresponding tile North exit exists
                if (x - 1 >= 0 &&
                    mapTileGameObjects[x, y] && mapTileGameObjects[x, y].GetComponent<MapTile>().exit3A &&
                    mapTileGameObjects[x - 1, y] && mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit3A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A);
                    mapTileGameObjects[x - 1, y].GetComponent<MapTile>().exit1A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x, y].GetComponent<MapTile>().exit3A);
                }
                // check 1. if out of bound, 2. current tile exists, 3. current tile East exit exists, 4. corresponding tile exists, 5. corresponding tile West exit exists
                if (y - 1 >= 0 &&
                    mapTileGameObjects[x, y] && mapTileGameObjects[x, y].GetComponent<MapTile>().exit4A &&
                    mapTileGameObjects[x, y - 1] && mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A)
                {
                    mapTileGameObjects[x, y].GetComponent<MapTile>().exit4A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A);
                    mapTileGameObjects[x, y - 1].GetComponent<MapTile>().exit2A.GetComponent<MapNode>().nodesConnected.Add(mapTileGameObjects[x, y].GetComponent<MapTile>().exit4A);
                }
            }
        }
    }
    
    // connect all nodes visually
    void InitializeNodes(int columns, int rows)
    {
        List<GameObject> childNodeList = new List<GameObject>();
        Transform lineHolder = new GameObject("Lines").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // skip if this tile doesnt exist
                if (!mapTileGameObjects[x, y])
                {
                    continue;
                }

                childNodeList.Clear();

                // construct a list of all nodes inside tile
                foreach (Transform t in mapTileGameObjects[x, y].transform)
                {
                    childNodeList.Add(t.gameObject);
                }

                // for all nodes inside tile
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    // for all nodesConnected inside node
                    for (int u = 0; u < childNodeList[i].GetComponent<MapNode>().nodesConnected.Count; u++)
                    {
                        if (childNodeList[i] && childNodeList[i].GetComponent<MapNode>().nodesConnected[u]) // i forgot why this part was necessary ha ha ha
                        {
                            //Debug.DrawLine(childNodeList[i].transform.position, childNodeList[i].GetComponent<MapNode>().nodesConnected[u].transform.position, Color.green, 1000, false);

                            // dotted line using line renderer
                            GameObject lineInstance = Instantiate(line) as GameObject;
                            lineInstance.GetComponent<Line>().DrawLine(childNodeList[i].transform, childNodeList[i].GetComponent<MapNode>().nodesConnected[u].transform);

                            // editing GameObject
                            lineInstance.transform.SetParent(lineHolder);
                        }
                    }
                }
            }
        }
    }

    // connect surrounding nodes visually
    public void ConnectNodes(GameObject node)
    {
        Destroy(GameObject.Find("Lines"));
        Transform lineHolder = new GameObject("Lines").transform;

        for (int u = 0; u < node.GetComponent<MapNode>().nodesConnected.Count; u++)
        {
            // dotted line using line renderer
            GameObject lineInstance = Instantiate(line) as GameObject;
            lineInstance.GetComponent<Line>().DrawLine(node.transform, node.GetComponent<MapNode>().nodesConnected[u].transform);

            // editing GameObject
            lineInstance.transform.SetParent(lineHolder);
        }
    }
}
