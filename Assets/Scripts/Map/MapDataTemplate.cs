using UnityEngine;
using System;
using System.Collections;

public class MapDataTemplate : MonoBehaviour
{
    // Map size
    public int columns;
    public int rows;

    // Zoning Garden
    public int garden_xFrom;
    public int garden_xTo;
    public int garden_yFrom;
    public int garden_yTo;

    // Zoning Building
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
    public GameObject tile3x3_0_0;
    public GameObject tile3x3_0_1;
    public GameObject tile3x3_0_2;

    public GameObject tile3x3_1_0;
    public GameObject tile3x3_1_1;
    public GameObject tile3x3_1_2;

    public GameObject tile3x3_2_0;
    public GameObject tile3x3_2_1;
    public GameObject tile3x3_2_2;
}
