using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class MapNode : MonoBehaviour
{
    SpriteRenderer nodeSprite;

    // node property
    public Layout LayoutTag;

    // basic node attributes
    public bool isClickable = false;
    public bool isVisited = false;
    public bool player = false;

    // i dont remember why i added this probably delete it? HA HA HA
    public Canvas dialogueUI;

    // list of neighbouring nodes
    public List<GameObject> nodesConnected = new List<GameObject>();

    public Instance assignedInstance;

    // reference to nodeTooltip script
    NodeTooltip NodeTooltipScript;

    void Awake()
    {
        nodeSprite = gameObject.GetComponent<SpriteRenderer>();
        NodeTooltipScript = GameObject.Find("MapManager").GetComponent<NodeTooltip>();
    }

    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (isClickable)
            {
                // turn node green if clickable
                nodeSprite.material.color = Color.green;

                // display node information
                NodeTooltipScript.ActivateHoverTooltip(assignedInstance);
            }
        }
    }

    void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // turn node back to original color
            nodeSprite.material.color = Color.white;

            // hide node information
            NodeTooltipScript.DeactivateHoverTooltip();
        }
    }

    void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (isClickable && !MapGameManager.instance.PlayerIsMoving()) // node is clickable & character is not moving
            {
                MapGameManager.instance.MovePlayer(gameObject);

                if (!isVisited)
                {
                    //MapGameManager.instance.UnmaskArea(gameObject);
                    //MapGameManager.instance.PlayInstance(assignedInstance);

                    StartCoroutine(NotVistedCase());
                }

                isVisited = true;

                // turn node back to original color
                nodeSprite.material.color = Color.white;

                // hide node information
                NodeTooltipScript.DeactivateHoverTooltip();
            }
        }
    }

    IEnumerator NotVistedCase()
    {
        MapGameManager.instance.UnmaskArea(gameObject);
        yield return new WaitForSeconds(1.5f);
        MapGameManager.instance.PlayInstance(assignedInstance);
    }
}
