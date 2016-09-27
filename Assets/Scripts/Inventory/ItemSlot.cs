using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    private Inventory inventoryScript;

    void Start()
    {
        inventoryScript = GameObject.Find("InventoryManager").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>(); // eventData.pointerDrag is the GameObject being dragged

        if (inventoryScript.items[slotIndex].ID == -1) // if no item there
        {
            inventoryScript.items[droppedItem.slotIndex] = new Item(); // null out the current item slot
            inventoryScript.items[this.slotIndex] = droppedItem.item;
            droppedItem.slotIndex = this.slotIndex; // set new slot location
        }
        else if(droppedItem.slotIndex != this.slotIndex)
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slotIndex = droppedItem.slotIndex;
            item.transform.SetParent(inventoryScript.slots[droppedItem.slotIndex].transform);
            item.transform.position = inventoryScript.slots[droppedItem.slotIndex].transform.position;

            droppedItem.slotIndex = this.slotIndex;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            inventoryScript.items[droppedItem.slotIndex] = item.GetComponent<ItemData>().item;
            inventoryScript.items[slotIndex] = droppedItem.item;
        }
    }
}
