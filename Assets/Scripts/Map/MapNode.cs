using UnityEngine;
using System.Collections;

public class MapNode : MonoBehaviour
{
    SpriteRenderer nodeSprite;

    public bool clickable = false;
    public bool player = false;
    public bool isVisited = false;

    //public MapNode[] mapNode;

    public GameObject[] nodesConnected;

    public void connectNode()
    {
        for (int i = 0; i < nodesConnected.Length; i++)
        {
            MapNode mapNode = nodesConnected[i].GetComponent<MapNode>();
        }
    }

    void Awake()
    {
        nodeSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        if (clickable)
        {
            nodeSprite.material.color = Color.green; // color wont work because most of the sprite is transparent
            //nodeSprite.enabled = false; // disable sprite for testing purposes
        }
    }

    void OnMouseExit()
    {
        if (clickable)
        {
            nodeSprite.material.color = Color.white;
            //nodeSprite.enabled = true;
        }
    }

    void OnMouseUp()
    {
        if (clickable)
        {
            //send player to this location // have this function in map manager, then access it
        }
    }
}
