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

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slotIndex;
    public InventoryType invType;

    private Transform originalParent;
    private Vector2 offset;

    private InventoryManager inventoryManagerScript;
    private ShopManager shopManagerScript;
    private InventoryTooltip tooltipScript;

    void Start()
    {
        inventoryManagerScript = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        shopManagerScript = GameObject.Find("InventoryManager").GetComponent<ShopManager>();
        tooltipScript = inventoryManagerScript.GetComponent<InventoryTooltip>();
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
        if (this.invType == InventoryType.PlayerInv)
        {
            this.transform.SetParent(inventoryManagerScript.invSlots[slotIndex].transform);
            this.transform.position = inventoryManagerScript.invSlots[slotIndex].transform.position;
        }

        if (this.invType == InventoryType.ShopInv)
        {
            this.transform.SetParent(shopManagerScript.shopSlots[slotIndex].transform);
            this.transform.position = shopManagerScript.shopSlots[slotIndex].transform.position;
        }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shopManagerScript.shopPanel.activeSelf && eventData.button == PointerEventData.InputButton.Right)
        {
            ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>(); // eventData.pointerDrag is the GameObject being dragged

            GameObject droppedItem = this.gameObject;

            // BUY
            if (this.invType == InventoryType.ShopInv)
            {
                // if enough money
                if (inventoryManagerScript.RemoveGold(this.item.Value))
                {
                    inventoryManagerScript.AddItem(this.item.ID);
                    Destroy(droppedItem);

                    if (droppedItemData.invType == InventoryType.PlayerInv)
                    {
                        inventoryManagerScript.invItems[droppedItemData.slotIndex] = new Item();
                    }
                    else if (droppedItemData.invType == InventoryType.ShopInv)
                    {
                        shopManagerScript.shopItems[droppedItemData.slotIndex] = new Item();
                    }

                    return;
                }
            }

            // SELL
            else if (this.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.AddGold(this.item.Value);
                Destroy(droppedItem);

                if (droppedItemData.invType == InventoryType.PlayerInv)
                {
                    inventoryManagerScript.invItems[droppedItemData.slotIndex] = new Item();
                }
                else if (droppedItemData.invType == InventoryType.ShopInv)
                {
                    shopManagerScript.shopItems[droppedItemData.slotIndex] = new Item();
                }

                return;
            }
        }
    }
}