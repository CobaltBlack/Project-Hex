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
    private List<Item> itemDatabase = new List<Item>();
    private List<Item.Equipment> equipmentDatabase = new List<Item.Equipment>();
    private List<Item.Consumable> consumablesDatabase = new List<Item.Consumable>();

    private JsonData itemData;

    void Awake()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    public Item getItemByID(int id)
    {
        for (int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i].ID == id)
            {
                return itemDatabase[i];
            }
        }

        return null;
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            if ((Item.ItemType)(int)itemData[i]["type"] == Item.ItemType.Default)
            {
                itemDatabase.Add(new Item((Item.ItemType)(int)itemData[i]["type"]
                    , (int)itemData[i]["id"]
                    , (string)itemData[i]["title"]
                    , (string)itemData[i]["description"]
                    , (int)itemData[i]["value"]
                    , (int)itemData[i]["rarity"]
                    , (bool)itemData[i]["stackable"]
                    , (string)itemData[i]["slug"]));
            }

            if ((Item.ItemType)(int)itemData[i]["type"] == Item.ItemType.Equipment)
            {
                itemDatabase.Add(new Item.Equipment((Item.ItemType)(int)itemData[i]["type"]
                    , (int)itemData[i]["id"]
                    , (string)itemData[i]["title"]
                    , (string)itemData[i]["description"]
                    , (int)itemData[i]["value"]
                    , (int)itemData[i]["rarity"]
                    , (bool)itemData[i]["stackable"]
                    , (string)itemData[i]["slug"]

                    , (int)itemData[i]["maxHp"]
                    , (int)itemData[i]["actionPoint"]
                    , (int)itemData[i]["moralityFlux"]
                    , (int)itemData[i]["sanityFlux"]
                    , (int)itemData[i]["attack"]
                    , (int)itemData[i]["crit"]
                    , (int)itemData[i]["defense"]
                    , (int)itemData[i]["dodge"]));
            }

            if ((Item.ItemType)(int)itemData[i]["type"] == Item.ItemType.Consumable)
            {
                itemDatabase.Add(new Item.Consumable((Item.ItemType)(int)itemData[i]["type"]
                    , (int)itemData[i]["id"]
                    , (string)itemData[i]["title"]
                    , (string)itemData[i]["description"]
                    , (int)itemData[i]["value"]
                    , (int)itemData[i]["rarity"]
                    , (bool)itemData[i]["stackable"]
                    , (string)itemData[i]["slug"]

                    , (string)itemData[i]["function name"]
                    , (int)itemData[i]["function parameter"]));
            }
        }
    }

    public void printDatabase()
    {
        Debug.Log("=============================PrintDatabase==============================");
        for (int i = 0; i < itemDatabase.Count; i++)
        {
            Debug.Log(itemDatabase[i].ID + " / " + itemDatabase[i]. Title);
        }
        Debug.Log("=============================PrintDatabase==============================");
    }
}