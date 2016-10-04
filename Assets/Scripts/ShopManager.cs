using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    ItemDatabase itemDatabaseScript;
    InventoryManager inventoryManagerScript;

    // the following items must be assigned in the inspector
    public GameObject shopPanel;
    public GameObject slotPanel;
    public GameObject blankSlot;
    public GameObject blankItem;

    public int slotAmount;

    [HideInInspector]
    public List<Item> shopItems = new List<Item>();
    [HideInInspector]
    public List<GameObject> shopSlots = new List<GameObject>();

    void Start()
    {
        Debug.Log("shop start");
        itemDatabaseScript = GetComponent<ItemDatabase>();
        inventoryManagerScript = GetComponent<InventoryManager>();

        // initialize all slots according to slotAmount
        InitializeInventory();

        // test
        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(0);
        AddItem(0);
        AddItem(0);
        AddItem(0);
        AddItem(0);
    }

    // initialize all slots according to slotAmount
    void InitializeInventory()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            shopItems.Add(new Item());
            shopSlots.Add(Instantiate(blankSlot)); // instantiate a slot
            shopSlots[i].GetComponent<ItemSlot>().slotIndex = i; // record slotIndex in ItemSlot script
            shopSlots[i].GetComponent<ItemSlot>().invType = InventoryType.ShopInv;
            shopSlots[i].transform.SetParent(slotPanel.transform); // child to slotPanel
        }
    }

    public void ToggleShop()
    {
        if (shopPanel.activeSelf)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            inventoryManagerScript.equipmentPanel.SetActive(false);
            shopPanel.SetActive(true);
        }
    }

    public void AddItem(int id)
    {
        Item itemToAdd = itemDatabaseScript.getItemByID(id);

        /*
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
        */

        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].ID == -1) // if no item in slot case (null item)
            {
                shopItems[i] = itemToAdd;                                   // add to items list
                GameObject itemObject = Instantiate(blankItem);         // instantiate itemObject GameObject

                itemObject.GetComponent<ItemData>().item = itemToAdd;       // register into itemData script attached to item
                itemObject.GetComponent<ItemData>().amount = 1;             // tick up the item amount
                itemObject.GetComponent<ItemData>().slotIndex = i;          // set slotLocation to current index
                itemObject.GetComponent<ItemData>().invType = InventoryType.ShopInv;

                itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // set sprite

                itemObject.transform.SetParent(shopSlots[i].transform);     // child to the corresponding inventorySlot in slots list
                itemObject.transform.position = Vector2.zero;               // set position
                itemObject.name = itemToAdd.Title;                          // rename items for inspector

                break;
            }
        }
    }

    // not yet implemented
    public bool RemoveItem(int itemId)
    {
        return true;
    }

    bool CheckIfInInventory(Item item)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void PrintShopItems()
    {
        Debug.Log("=============================PrintShopItems=============================");
        for (int i = 0; i < shopItems.Count; i++)
        {
            Debug.Log(shopItems[i].Title);
        }
        Debug.Log("=============================PrintShopItems=============================");
    }
}