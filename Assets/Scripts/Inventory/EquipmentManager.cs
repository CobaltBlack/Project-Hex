using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    ItemDatabase itemDatabaseScript;
    InventoryManager inventoryManagerScript;

    PlayerManager playerManagerScript;

    // the following items must be assigned in the inspector
    public GameObject shopPanel;
    public GameObject slotPanel;
    public GameObject blankSlot;
    public GameObject blankItem;

    public int slotAmount;

    [HideInInspector]
    public List<Item> equipItems = new List<Item>();
    [HideInInspector]
    public List<GameObject> equipSlots = new List<GameObject>();

    void Start()
    {
        Debug.Log("equip start");
        itemDatabaseScript = GetComponent<ItemDatabase>();
        inventoryManagerScript = GetComponent<InventoryManager>();

        playerManagerScript = GameObject.Find("MapManager").GetComponent<PlayerManager>();

        // initialize all slots according to slotAmount
        InitializeInventory();

        // test
        AddItem(2);
        AddItem(3);
        AddItem(3);
    }

    // initialize all slots according to slotAmount
    void InitializeInventory()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            equipItems.Add(new Item());
            equipSlots.Add(Instantiate(blankSlot));                                  // instantiate a slot
            equipSlots[i].GetComponent<ItemSlot>().slotIndex = i;                    // record slotIndex in ItemSlot script
            equipSlots[i].GetComponent<ItemSlot>().slotType = SlotType.EquipmentInv;  // record invType in ItemSlot script
            equipSlots[i].transform.SetParent(slotPanel.transform);                  // child to slotPanel
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

        for (int i = 0; i < equipItems.Count; i++)
        {
            if (equipItems[i].ID == -1) // if no item in slot case (null item)
            {
                equipItems[i] = itemToAdd;                                   // add to items list
                GameObject itemObject = Instantiate(blankItem);             // instantiate itemObject GameObject

                itemObject.GetComponent<ItemData>().item = itemToAdd;       // register into itemData script attached to item
                itemObject.GetComponent<ItemData>().amount = 1;             // tick up the item amount
                itemObject.GetComponent<ItemData>().slotIndex = i;          // set slotLocation to current index
                itemObject.GetComponent<ItemData>().slotType = SlotType.EquipmentInv;

                itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // set sprite

                itemObject.transform.SetParent(equipSlots[i].transform);     // child to the corresponding inventorySlot in slots list
                //itemObject.transform.position = Vector2.zero;                 // set position // why doesnt this work anymore?
                itemObject.transform.position = equipSlots[i].transform.position; // set position
                itemObject.name = itemToAdd.Title;                          // rename items for inspector

                playerManagerScript.RefreshPlayerStats(); // redundant?

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
        for (int i = 0; i < equipItems.Count; i++)
        {
            if (equipItems[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void PrintEquipItems()
    {
        Debug.Log("=============================PrintEquipItems============================");
        for (int i = 0; i < equipItems.Count; i++)
        {
            Debug.Log(equipItems[i].Title);
        }
        Debug.Log("=============================PrintEquipItems============================");
    }
}