using UnityEngine;
using System.Collections.Generic;

public class MapNode : MonoBehaviour
{
    SpriteRenderer nodeSprite;

    // basic node attributes
    public bool isClickable = false;
    public bool isVisited = false;
    public bool player = false;

    // list of neighbouring nodes
    public List<GameObject> nodesConnected = new List<GameObject>();

    void Awake()
    {
        nodeSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        if (isClickable)
        {
            nodeSprite.material.color = Color.green;
        }
    }

    void OnMouseExit()
    {
        nodeSprite.material.color = Color.white;
    }

    void OnMouseUp()
    {
        if (isClickable)
        {
            MapGameManager.instance.MovePlayer(gameObject);
        }
    }
}
