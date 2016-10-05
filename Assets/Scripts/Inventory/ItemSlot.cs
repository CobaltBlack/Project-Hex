using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/*
 * ItemSlot 
 * 
 * Attach this script to UI "Slot" GameObject.
 * Item stacking and click and drags are handled here.  
 * 
 */

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    public InventoryType invType;
    private InventoryManager inventoryManagerScript;
    private ShopManager shopManagerScript;

    private ItemData droppedItemData;

    void Start()
    {
        inventoryManagerScript = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        shopManagerScript = GameObject.Find("InventoryManager").GetComponent<ShopManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        droppedItemData = eventData.pointerDrag.GetComponent<ItemData>(); // eventData.pointerDrag is GameObject being dragged

        if (droppedItemData.invType != this.invType)
        {
            GameObject droppedItem = eventData.pointerDrag;

            // BUY
            if (this.invType == InventoryType.PlayerInv)
            {
                // if sufficient gold, remove gold and execute following
                if (inventoryManagerScript.RemoveGold(droppedItemData.item.Value))
                {
                    Destroy(droppedItem);
                    inventoryManagerScript.AddItem(droppedItemData.item.ID);

                    // update list
                    ListUpdatePreviousIndex(new Item());

                    return;
                }
            }

            // SELL
            else if (this.invType == InventoryType.ShopInv)
            {
                Destroy(droppedItem);
                inventoryManagerScript.AddGold(droppedItemData.item.Value);

                // update list
                ListUpdatePreviousIndex(new Item());

                return;
            }

            return;
        }

        // makes you unable to rearrange shop items
        if (this.invType == InventoryType.ShopInv)
        {
            return;
        }

        //==================== remove two above if components above to make two independent item windows ====================//

        // drop on an empty slot
        if (this.invType == InventoryType.PlayerInv && inventoryManagerScript.invItems[slotIndex].ID == -1)
        {
            // update list - previous index
            ListUpdatePreviousIndex(new Item());

            // update list - current index
            inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item;

            // update ItemData according to data in ItemSlot
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.invType = this.invType;

            // transform is handled in ItemData
        }

        // drop on an empty slot
        else if (this.invType == InventoryType.ShopInv && shopManagerScript.shopItems[slotIndex].ID == -1)
        {
            // update list - previous index
            ListUpdatePreviousIndex(new Item());

            // update list - current index
            shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item;

            // update ItemData according to data in ItemSlot
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.invType = this.invType;

            // transform is handled in ItemData
        }

        // drop on a slot with item, and the item isnt being dropped on the same slot
        else if (!(droppedItemData.invType == this.invType && droppedItemData.slotIndex == this.slotIndex))
        {
            Transform currentItemInSlot = this.transform.GetChild(0);

            // update ItemData - current item in slot
            currentItemInSlot.GetComponent<ItemData>().slotIndex = droppedItemData.slotIndex;
            currentItemInSlot.GetComponent<ItemData>().invType = droppedItemData.invType;

            // update transform - current item in slot
            if (droppedItemData.invType == InventoryType.PlayerInv)
            {
                currentItemInSlot.transform.SetParent(inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform);
                currentItemInSlot.transform.position = inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform.position;
            }
            else if (droppedItemData.invType == InventoryType.ShopInv)
            {
                currentItemInSlot.transform.SetParent(shopManagerScript.shopSlots[droppedItemData.slotIndex].transform);
                currentItemInSlot.transform.position = shopManagerScript.shopSlots[droppedItemData.slotIndex].transform.position;
            }

            // update list - current index
            if (this.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }
            else if (this.invType == InventoryType.ShopInv)
            {
                shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }

            // update list - previous index
            ListUpdatePreviousIndex(currentItemInSlot.GetComponent<ItemData>().item);

            // update ItemData - droppedItemData
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.invType = this.invType;

            // update transform - droppedItemData
            droppedItemData.transform.SetParent(this.transform);
            droppedItemData.transform.position = this.transform.position;
        }
    }

    // update list according to invType and slotIndex of where the droppedItem came from, with the Item item of choice
    public void ListUpdatePreviousIndex(Item item)
    {
        if (droppedItemData.invType == InventoryType.PlayerInv)
        {
            inventoryManagerScript.invItems[droppedItemData.slotIndex] = item;
        }
        else if (droppedItemData.invType == InventoryType.ShopInv)
        {
            shopManagerScript.shopItems[droppedItemData.slotIndex] = item;
        }
    }
}