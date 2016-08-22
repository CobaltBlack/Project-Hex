using UnityEngine;
using System;
using System.Collections;

public class MapDataTemplate : MonoBehaviour
{
    // class categorization is not used at the moment
    [Serializable]
    public class Zone
    {
        public int xFrom;
        public int xTo;

        public int yFrom;
        public int yTo;

        public Zone(int xFrom, int xTo, int yFrom, int yTo)
        {
            this.xFrom = xFrom;
            this.xTo = xTo;

            this.yFrom = yFrom;
            this.yTo = yTo;
        }
    }

    // Map size
    public int columns;
    public int rows;

    // Zoning Garden
    public Zone garden;

    public int garden_xFrom;
    public int garden_xTo;
    public int garden_yFrom;
    public int garden_yTo;

    // Zoning Building
    public Zone building;

    public int building_xFrom;
    public int building_xTo;
    public int building_yFrom;
    public int building_yTo;

    // Premade location 3x3 origin
    public int premade3x3OriginX;
    public int premade3x3OriginY;

    // Premade location 3x3
    public GameObject[,] premade3x3;

    // Premade location 3x3 tiles gameobject
    public GameObject tile_0_0;
    public GameObject tile_0_1;
    public GameObject tile_0_2;

    public GameObject tile_1_0;
    public GameObject tile_1_1;
    public GameObject tile_1_2;

    public GameObject tile_2_0;
    public GameObject tile_2_1;
    public GameObject tile_2_2;

    void Awake()
    {
        // Zoning // this part is redundant at the moment
        garden = new Zone(0, columns, 0, 1);
        building = new Zone(building_xFrom, building_xTo, building_yFrom, building_yTo);

        // Premade
        premade3x3 = new GameObject[3, 3];

        premade3x3[0, 0] = tile_0_0;
        premade3x3[0, 1] = tile_0_1;
        premade3x3[0, 2] = tile_0_2;

        premade3x3[1, 0] = tile_1_0;
        premade3x3[1, 1] = tile_1_1;
        premade3x3[1, 2] = tile_1_2;

        premade3x3[2, 0] = tile_2_0;
        premade3x3[2, 1] = tile_2_1;
        premade3x3[2, 2] = tile_2_2;
    }
}
