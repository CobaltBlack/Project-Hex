using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NodeTooltip : MonoBehaviour
{
    private string descriptionText;
    public GameObject hoverTooltip; // assign "HoverUI" in inspector. Also, remember to tick off "HoverUI" before running game.

    void Update()
    {
        if (hoverTooltip.activeSelf)
        {
            hoverTooltip.transform.position = Input.mousePosition;
        }

        if (EventSystem.current.IsPointerOverGameObject()) // UI elements getting the hit/hover
        {
            DeactivateHoverTooltip();
        }

    }

    public void ActivateHoverTooltip()
    {
        ConstructTooltipDataString();

        hoverTooltip.SetActive(true);
    }

    public void DeactivateHoverTooltip()
    {
        hoverTooltip.SetActive(false);
    }

    public void ConstructTooltipDataString()
    {
        // WHITE:   #FFFFFF
        // BLACK:   #000000
        // GREY:    #808080
        // RED:     #FF0000
        // YELLOW:  #FFFF00
        // LIME:    #00FF00
        // BLUE:    #0000FF
        descriptionText = "<color=#FFFFFF><b>" + "PLACE HOLDER MESSAGE" + "</b></color>" + "\n\n" + "<color=#FFFFFF>" + "MORE PLACE HOLDER MESSAGE" + "</color>";
        hoverTooltip.transform.GetChild(0).GetComponent<Text>().text = descriptionText;
    }
}
