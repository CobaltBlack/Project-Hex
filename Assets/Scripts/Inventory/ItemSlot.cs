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
    public SlotType slotType;

    InventoryManager inventoryManagerScript;
    ShopManager shopManagerScript;
    EquipmentManager equipmentManagerScript;

    private ItemData droppedItemData;

    void Start()
    {
        inventoryManagerScript = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        shopManagerScript = inventoryManagerScript.GetComponent<ShopManager>();
        equipmentManagerScript = inventoryManagerScript.GetComponent<EquipmentManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        droppedItemData = eventData.pointerDrag.GetComponent<ItemData>(); // eventData.pointerDrag is GameObject being dragged

        if (shopManagerScript.shopPanel.activeSelf)
        { 
            GameObject droppedItem = eventData.pointerDrag;

            // BUY
            if (this.slotType == SlotType.PlayerInv && droppedItemData.slotType == SlotType.ShopInv)
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
            else if (this.slotType == SlotType.ShopInv && droppedItemData.slotType == SlotType.PlayerInv)
            {
                Destroy(droppedItem);
                inventoryManagerScript.AddGold(droppedItemData.item.Value);

                // update list
                ListUpdatePreviousIndex(new Item());

                return;
            }
        }

        // makes you unable to rearrange shop items
        if (this.slotType == SlotType.ShopInv)
        {
            return;
        }

        //==================== remove two above if components above to make two independent item windows ====================//

        // drop on an empty slot
        if (this.slotType == SlotType.PlayerInv && inventoryManagerScript.invItems[slotIndex].ID == -1)
        {
            // update list - previous index
            ListUpdatePreviousIndex(new Item());

            // update list - current index
            inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item;

            // update ItemData according to data in ItemSlot
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.slotType = this.slotType;

            // transform is handled in ItemData
        }

        // drop on an empty slot
        else if (this.slotType == SlotType.ShopInv && shopManagerScript.shopItems[slotIndex].ID == -1)
        {
            // update list - previous index
            ListUpdatePreviousIndex(new Item());

            // update list - current index
            shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item;

            // update ItemData according to data in ItemSlot
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.slotType = this.slotType;

            // transform is handled in ItemData
        }

        // drop on an empty slot
        else if (this.slotType == SlotType.EquipmentInv && equipmentManagerScript.equipItems[slotIndex].ID == -1)
        {
            if (droppedItemData.item.Type == Item.ItemType.Equipment)
            {
                // update list - previous index
                ListUpdatePreviousIndex(new Item());

                // update list - current index
                equipmentManagerScript.equipItems[this.slotIndex] = droppedItemData.item;

                // update ItemData according to data in ItemSlot
                droppedItemData.slotIndex = this.slotIndex;
                droppedItemData.slotType = this.slotType;

                // transform is handled in ItemData
            }
        }

        // drop on a slot with item, and the item isnt being dropped on the same slot
        else if (!(droppedItemData.slotType == this.slotType && droppedItemData.slotIndex == this.slotIndex))
        {
            Transform currentItemInSlot = this.transform.GetChild(0);
            ItemData currentItemData = currentItemInSlot.GetComponent<ItemData>();

            // prevent swapping between player inventory and equipment if the item type isnt equipment in either droppedItemData or currentItemInSlot
            if ((droppedItemData.slotType == SlotType.PlayerInv && currentItemData.slotType == SlotType.EquipmentInv)
                || (droppedItemData.slotType == SlotType.EquipmentInv && currentItemData.slotType == SlotType.PlayerInv))
            {
                if (droppedItemData.item.Type != Item.ItemType.Equipment || currentItemData.item.Type != Item.ItemType.Equipment)
                {
                    return;
                }
            }

            // update ItemData - current item in slot
            currentItemInSlot.GetComponent<ItemData>().slotIndex = droppedItemData.slotIndex;
            currentItemInSlot.GetComponent<ItemData>().slotType = droppedItemData.slotType;

            // update transform - current item in slot
            if (droppedItemData.slotType == SlotType.PlayerInv)
            {
                currentItemInSlot.transform.SetParent(inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform);
                currentItemInSlot.transform.position = inventoryManagerScript.invSlots[droppedItemData.slotIndex].transform.position;
            }
            else if (droppedItemData.slotType == SlotType.ShopInv)
            {
                currentItemInSlot.transform.SetParent(shopManagerScript.shopSlots[droppedItemData.slotIndex].transform);
                currentItemInSlot.transform.position = shopManagerScript.shopSlots[droppedItemData.slotIndex].transform.position;
            }
            else if (droppedItemData.slotType == SlotType.EquipmentInv)
            {
                currentItemInSlot.transform.SetParent(equipmentManagerScript.equipSlots[droppedItemData.slotIndex].transform);
                currentItemInSlot.transform.position = equipmentManagerScript.equipSlots[droppedItemData.slotIndex].transform.position;
            }

            // update list - current index
            if (this.slotType == SlotType.PlayerInv)
            {
                inventoryManagerScript.invItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }
            else if (this.slotType == SlotType.ShopInv)
            {
                shopManagerScript.shopItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }
            else if (this.slotType == SlotType.EquipmentInv)
            {
                equipmentManagerScript.equipItems[this.slotIndex] = droppedItemData.item; // register the new item in the items list
            }

            // update list - previous index
            ListUpdatePreviousIndex(currentItemInSlot.GetComponent<ItemData>().item);

            // update ItemData - droppedItemData
            droppedItemData.slotIndex = this.slotIndex;
            droppedItemData.slotType = this.slotType;

            // update transform - droppedItemData - handled in itemData
        }
    }

    // update list according to invType and slotIndex of where the droppedItem came from, with the Item item of choice
    public void ListUpdatePreviousIndex(Item item)
    {
        if (droppedItemData.slotType == SlotType.PlayerInv)
        {
            inventoryManagerScript.invItems[droppedItemData.slotIndex] = item;
        }
        else if (droppedItemData.slotType == SlotType.ShopInv)
        {
            shopManagerScript.shopItems[droppedItemData.slotIndex] = item;
        }
        else if (droppedItemData.slotType == SlotType.EquipmentInv)
        {
            equipmentManagerScript.equipItems[droppedItemData.slotIndex] = item;
        }
    }
}