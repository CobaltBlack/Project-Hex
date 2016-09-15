using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * InventoryManager
 * 
 * This script provides functions to manage the player inventory and gold.
 * 
 */

public class InventoryManager : MonoBehaviour
{
    public int maxNumOfItems;
    public int startingGold;
    public List<Item> playerItems; // Current items possessed by player

    ItemDatabase database;

    int playerGoldAmount;

    public void InventorySetup()
    {
        database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();

        playerItems = new List<Item>();
        playerGoldAmount = startingGold;

        // Give player test item
        AddItem(1);
    }

    public bool doesPlayerHaveItem(Item item)
    {
        return true;
    }

    public void AddItem(int itemId)
    {
        if (playerItems.Count >= maxNumOfItems)
        {
            return;
        }
        playerItems.Add(database.getItemByID(itemId));
    }

    public bool RemoveItem(int itemId)
    {
        return true;
    }

    public int getPlayerGold()
    {
        return playerGoldAmount;
    }

    public void AddGold(int amount)
    {
        playerGoldAmount += amount;
    }

    public bool RemoveGold(int amount)
    {
        if (amount > getPlayerGold())
        {
            return false;
        }

        playerGoldAmount -= amount;
        return true;
    }
}
