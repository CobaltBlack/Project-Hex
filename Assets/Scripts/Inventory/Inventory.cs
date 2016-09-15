using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    ItemDatabase itemDatabaseScript;

    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        itemDatabaseScript = GetComponent<ItemDatabase>();

        inventoryPanel = GameObject.Find("Inventory Panel"); // assign in inspector later?
        slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject; // assign in inspector later?

        // initialize all slots according to slotAmount
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot)); // instantiate a slot
            slots[i].GetComponent<ItemSlot>().slotIndex = i; // record slotIndex in ItemSlot script
            slots[i].transform.SetParent(slotPanel.transform); // child to slotPanel
        }

        // test
        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(0);
        AddItem(0);
    }

    public void AddItem(int id)
    {
        Item itemToAdd = itemDatabaseScript.getItemByID(id);

        if (itemToAdd.Stackable && CheckIfInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++) // shoot! the exact same loop was done in CheckIfInInventory! maybe it should be compressed!
            {
                if (items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>(); // access the ItemData Script attached to Item
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString(); // set text according to amount

                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1) // if no item in slot case (null item)
                {
                    items[i] = itemToAdd;
                    GameObject itemObject = Instantiate(inventoryItem); // instantiate

                    itemObject.GetComponent<ItemData>().item = itemToAdd; // register into itemData script attached to item
                    itemObject.GetComponent<ItemData>().amount = 1; // tick up the item amount
                    itemObject.GetComponent<ItemData>().slotIndex = i; // set slotLocation to current index

                    itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // set sprite

                    itemObject.transform.SetParent(slots[i].transform); // child to the corresponding inventorySlot in slots list
                    itemObject.transform.position = Vector2.zero; // set position
                    itemObject.name = itemToAdd.Title; // rename items for inspector

                    break;
                }
            }
        }
    }

    bool CheckIfInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }
}