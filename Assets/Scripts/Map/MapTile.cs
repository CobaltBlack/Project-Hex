using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour
{
    // tile size
    public int size = 0; // 0 = small, 1 = big

    // property
    public EnumTileProperty property;

    // EXIT information
    public GameObject exit1A;
    public GameObject exit1B;

    public GameObject exit2A;
    public GameObject exit2B;

    public GameObject exit3A;
    public GameObject exit3B;

    public GameObject exit4A;
    public GameObject exit4B;

    // default player starting node
    public GameObject startingNode;
}
