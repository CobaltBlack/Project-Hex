using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    ItemDatabase itemDatabaseScript;
    ShopManager shopManagerScript;

    // the following items must be assigned in the inspector
    public GameObject inventoryPanel;
    public GameObject invSlotPanel;
    public GameObject equipmentPanel;
    public GameObject equipSlotPanel;
    public GameObject blankSlot;
    public GameObject blankItem;
    public Text GoldText;

    public int startingGold;
    int playerGold;

    public int invSlotAmount;
    public int equipSlotAmount;

    [HideInInspector]
    public List<Item> invItems = new List<Item>();
    [HideInInspector]
    public List<GameObject> invSlots = new List<GameObject>();

    void Start()
    {
        Debug.Log("inv start");
        itemDatabaseScript = GetComponent<ItemDatabase>();
        shopManagerScript = GetComponent<ShopManager>();

        // initialize all slots according to slotAmount
        InitializeInventory();
        //InitializeEquipment();

        // initialize gold
        playerGold = startingGold;
        RefreshGoldText();

        // test
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(0);
        AddItem(0);
        AddItem(2);
        AddItem(3);

        //AddEquipment(2);
        //AddEquipment(3);
    }

    // initialize all slots according to slotAmount
    void InitializeInventory()
    {
        for (int i = 0; i < invSlotAmount; i++)
        {
            // add to Items list
            invItems.Add(new Item());                                               // NULL case (ID = -1), add to items list

            // add to Slots list, record slot information
            invSlots.Add(Instantiate(blankSlot));                                   // instantiate a slot, add to slots list
            invSlots[i].GetComponent<ItemSlot>().slotIndex = i;                     // record slotIndex in ItemSlot script
            invSlots[i].GetComponent<ItemSlot>().slotType = SlotType.PlayerInv; // record invType in ItemSlot script
            invSlots[i].transform.SetParent(invSlotPanel.transform);                // child to slotPanel
        }
    }

    void InitializeEquipment()
    {
        for (int i = 0 + invSlotAmount; i < equipSlotAmount + invSlotAmount; i++)
        {
            // add to Items list
            invItems.Add(new Item());                                               // NULL case (ID = -1), add to items list

            // add to Slots list, record slot information
            invSlots.Add(Instantiate(blankSlot));                                   // instantiate a slot, add to slots list
            invSlots[i].GetComponent<ItemSlot>().slotIndex = i;                     // record slotIndex in ItemSlot script
            invSlots[i].GetComponent<ItemSlot>().slotType = SlotType.PlayerInv; // record invType in ItemSlot script
            invSlots[i].transform.SetParent(equipSlotPanel.transform);              // child to slotPanel
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
        }
    }

    public void ToggleEquipment()
    {
        if (equipmentPanel.activeSelf)
        {
            equipmentPanel.SetActive(false);
        }
        else
        {
            shopManagerScript.shopPanel.SetActive(false);
            equipmentPanel.SetActive(true);
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

        for (int i = 0; i < invSlotAmount; i++)
        {
            if (invItems[i].ID == -1) // if no item in slot case (null item)
            {
                invItems[i] = itemToAdd;                                        // add to items list
                GameObject itemObject = Instantiate(blankItem);                 // instantiate itemObject GameObject

                itemObject.GetComponent<ItemData>().item = itemToAdd;           // register into itemData script attached to item
                itemObject.GetComponent<ItemData>().amount = 1;                 // tick up the item amount
                itemObject.GetComponent<ItemData>().slotIndex = i;              // set slotIndex to current index in ItemData
                itemObject.GetComponent<ItemData>().slotType = SlotType.PlayerInv;

                itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;     // set sprite

                itemObject.transform.SetParent(invSlots[i].transform);          // child to the corresponding inventorySlot in slots list
                //itemObject.transform.position = Vector2.zero;                 // set position // why did this line break?
                itemObject.transform.position = invSlots[i].transform.position; // set position // replacement
                itemObject.name = itemToAdd.Title;                              // rename items for inspector

                break;
            }
        }
    }

    public void AddEquipment(int id)
    {
        Item itemToAdd = itemDatabaseScript.getItemByID(id);

        for (int i = 0 + invSlotAmount; i < equipSlotAmount + invSlotAmount; i++)
        {
            if (invItems[i].ID == -1) // if no item in slot case (null item)
            {
                invItems[i] = itemToAdd;                                        // add to items list
                GameObject itemObject = Instantiate(blankItem);                 // instantiate itemObject GameObject

                itemObject.GetComponent<ItemData>().item = itemToAdd;           // register into itemData script attached to item
                itemObject.GetComponent<ItemData>().amount = 1;                 // tick up the item amount
                itemObject.GetComponent<ItemData>().slotIndex = i;              // set slotIndex to current index in ItemData
                itemObject.GetComponent<ItemData>().slotType = SlotType.PlayerInv;

                itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;     // set sprite

                itemObject.transform.SetParent(invSlots[i].transform);          // child to the corresponding inventorySlot in slots list
                //itemObject.transform.position = Vector2.zero;                 // set position // why doesnt this work anymore?
                itemObject.transform.position = invSlots[i].transform.position; // set position
                itemObject.name = itemToAdd.Title;                              // rename items for inspector

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
        for (int i = 0; i < invItems.Count; i++)
        {
            if (invItems[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }

    // gold manipulation
    public int getPlayerGold()
    {
        return playerGold;
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        RefreshGoldText();
    }

    public bool RemoveGold(int amount)
    {
        if (amount > playerGold)
        {
            return false;
        }

        playerGold -= amount;
        RefreshGoldText();
        return true;
    }

    void RefreshGoldText()
    {
        GoldText.text = playerGold + " USD";
    }

    // Debug
    public void PrintInvItems()
    {
        Debug.Log("=============================PrintInvItems==============================");
        for (int i = 0; i < invItems.Count; i++)
        {
            Debug.Log(invItems[i].Title);
        }
        Debug.Log("=============================PrintInvItems==============================");
    }
}