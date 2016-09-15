using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryTooltip : MonoBehaviour
{
    private Item item;
    private string descriptionText;

    public GameObject tooltip; // assign "Tooltip" in inspector. Also, remember to tick off "Tooltip" before running game.

    void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }

    public void ActivateTooltip(Item currentItem)
    {
        this.item = currentItem;
        ConstructTooltipDataString();

        tooltip.SetActive(true);
    }

    public void DeactivateTooltip()
    {
        tooltip.SetActive(false);
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
        descriptionText = "<color=#FFFFFF><b>" + item.Title + "</b></color>" + "\n\n" + "<color=#FFFFFF>" + item.Description + "</color>";
        tooltip.transform.GetChild(0).GetComponent<Text>().text = descriptionText;
    }
}
