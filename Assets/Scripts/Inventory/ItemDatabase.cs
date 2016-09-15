using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

/*
 * ItemDatabase
 * 
 * This script file manages the item database
 * 
 */

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Awake()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    public Item getItemByID(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            {
                return database[i];
            }
        }

        return null;
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"]
                , (string)itemData[i]["title"]
                , (int)itemData[i]["value"]

                , (int)itemData[i]["stats"]["power"]
                , (int)itemData[i]["stats"]["defense"]
                , (int)itemData[i]["stats"]["vitality"]

                , (string)itemData[i]["description"]
                , (bool)itemData[i]["stackable"]
                , (int)itemData[i]["rarity"]
                , (string)itemData[i]["slug"] ));
        }
    }
}

    /*
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
*/