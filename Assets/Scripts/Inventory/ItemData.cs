using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/*
 * ItemData 
 * 
 * Attach this script to UI "Item" GameObject.
 * Item stacking and click and drags are handled here.  
 * 
 */

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int amount;
    public int slotIndex;

    private Transform originalParent;
    private Vector2 offset;

    private Inventory inventoryScript;
    private InventoryTooltip tooltipScript;

    void Start()
    {
        inventoryScript = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        tooltipScript = inventoryScript.GetComponent<InventoryTooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // to avoid visual conflict, move to the slot panel while moving
            originalParent = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            // to avoid mouse from grabbing the center of the item, instead grab where it is clicked
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.position = eventData.position - offset;

            // while item is being dragged, turn off blockRaycasts so raycast can hit slots
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryScript.slots[slotIndex].transform);
        this.transform.position = inventoryScript.slots[slotIndex].transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true; // turn blockRaycasts back on
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipScript.ActivateTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipScript.DeactivateTooltip();
    }
}
