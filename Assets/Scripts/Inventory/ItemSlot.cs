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

    void Start()
    {
        inventoryManagerScript = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        shopManagerScript = GameObject.Find("InventoryManager").GetComponent<ShopManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>(); // eventData.pointerDrag is the GameObject being dragged

        if (droppedItemData.invType != this.invType)
        {
            GameObject droppedItem = eventData.pointerDrag;

            // BUY
            if (this.invType == InventoryType.PlayerInv)
            {
                // if enough money
                if (inventoryManagerScript.RemoveGold(droppedItemData.item.Value)) // check if enough gold is available, and remove it if true
                {
                    inventoryManagerScript.AddItem(droppedItemData.item.ID);
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
            else if (this.invType == InventoryType.ShopInv)
            {
                inventoryManagerScript.AddGold(droppedItemData.item.Value);
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

            return;
        }

        // drop on an empty slot
        if (this.invType == InventoryType.PlayerInv && inventoryManagerScript.invItems[slotIndex].ID == -1) // if no item there 
        {
            // update list

            // came from list index
            if (droppedItemData.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.invItems[droppedItemData.slotIndex] = new Item(); // null out the current item in the items list
            }
            else if (droppedItemData.invType == InventoryType.ShopInv)
            {
                shopManagerScript.shopItems[droppedItemData.slotIndex] = new Item(); // null out the current item in the items list
            }

            // now in list index
            inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list

            // update dropped items ItemData
            droppedItemData.slotIndex = this.slotIndex; // update slotIndex of the itemData of item dropped to the slotIndex of this new slot
            droppedItemData.invType = this.invType;
        }

        // drop on an empty slot
        else if (this.invType == InventoryType.ShopInv && shopManagerScript.shopItems[slotIndex].ID == -1) // if no item there 
        {
            // update list

            // came from list index
            if (droppedItemData.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.invItems[droppedItemData.slotIndex] = new Item(); // null out the current item in the items list
            }
            else if (droppedItemData.invType == InventoryType.ShopInv)
            {
                shopManagerScript.shopItems[droppedItemData.slotIndex] = new Item(); // null out the current item in the items list
            }

            // now in list index
            shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list

            // update dropped items ItemData
            droppedItemData.slotIndex = this.slotIndex; // update slotIndex of the itemData of item dropped to the slotIndex of this new slot
            droppedItemData.invType = this.invType;
        }

        // drop on a slot with item, and the item isnt being dropped on the same slot
        else if (!(droppedItemData.invType == this.invType && droppedItemData.slotIndex == this.slotIndex))
        {
            Transform originalItem = this.transform.GetChild(0);

            // update original items itemData
            originalItem.GetComponent<ItemData>().slotIndex = droppedItemData.slotIndex;
            originalItem.GetComponent<ItemData>().invType = droppedItemData.invType;

            // handle movement of item currently in slot
            if (droppedItemData.invType == InventoryType.PlayerInv)
            {
                originalItem.transform.SetParent(inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform);
                originalItem.transform.position = inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform.position;
            }
            else if (droppedItemData.invType == InventoryType.ShopInv)
            {
                originalItem.transform.SetParent(shopManagerScript.shopSlots[droppedItemData.slotIndex].transform);
                originalItem.transform.position = shopManagerScript.shopSlots[droppedItemData.slotIndex].transform.position;
            }

            // update list
            //inventoryManagerScript.invItems[droppedItemData.slotIndex] = item.GetComponent<ItemData>().item;
            //inventoryManagerScript.invItems[slotIndex] = droppedItemData.item;

            if (droppedItemData.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.invItems[droppedItemData.slotIndex] = originalItem.GetComponent<ItemData>().item;
            }
            else if (droppedItemData.invType == InventoryType.ShopInv)
            {
                shopManagerScript.shopItems[droppedItemData.slotIndex] = originalItem.GetComponent<ItemData>().item;
            }

            if (this.invType == InventoryType.PlayerInv)
            {
                inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }
            else if (this.invType == InventoryType.ShopInv)
            {
                shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }

            // update dropped items ItemData
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.invType = this.invType;

            // handle movement of item new in slot
            droppedItemData.transform.SetParent(this.transform);
            droppedItemData.transform.position = this.transform.position;
        }
    }
}