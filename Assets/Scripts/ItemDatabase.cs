using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * ItemDatabase
 * 
 * This script file manages the item database
 * 
 */

public class ItemDatabase : MonoBehaviour
{
    // List of items defined in the Unity Inspector
    public List<Item> items = new List<Item>();
    public List<Consumable> consumables = new List<Consumable>();

    public Item getItemById(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemId == id)
            {
                return items[i];
            }
        }

        return null;
    }
}
