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
    QuestMark QuestMarkScript;

    void Awake()
    {
        nodeSprite = gameObject.GetComponent<SpriteRenderer>();
        NodeTooltipScript = GameObject.Find("MapManager").GetComponent<NodeTooltip>();
        QuestMarkScript = GameObject.Find("QuestMark").GetComponent<QuestMark>();
    }

    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (isClickable)
            {
                // turn node green if clickable
                nodeSprite.material.color = Color.black;

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
            if (isClickable && !MapGameManager.instance.PlayerIsMoving() && !QuestMarkScript.IsActive()) // node is clickable & character is not moving & quest mark is inactive
            {
                MapGameManager.instance.MovePlayerPartA(gameObject);

                if (!isVisited)
                {
                    StartCoroutine(NotVistedCase());

                    isVisited = true;
                }

                else
                {
                    StartCoroutine(VistedCase());
                }

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

        yield return new WaitForSeconds(2f);

        QuestMarkScript.ActivateQuestMark(assignedInstance);
    }

    IEnumerator VistedCase()
    {
        yield return new WaitForSeconds(2f);

        MapGameManager.instance.MovePlayerPartB();
    }
}
