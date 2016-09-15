using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }

    public int Power { get; set; }
    public int Defense { get; set; }
    public int Vitality { get; set; }

    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }

    public Sprite Sprite { get; set; }

    public Item(int id, string title, int value, int power, int defense, int vitality, string description, bool stackable, int rarity, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;

        this.Power = power;
        this.Defense = defense;
        this.Vitality = vitality;

        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Slug = slug;

        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    // no item in slot case
    public Item()
    {
        this.ID = -1;
    }
}

    /*
    public int itemId;
    public string itemName;
    public ItemType itemType;
    public string itemDesc;
    public int itemCost;
    public Texture2D itemIcon;

    public enum ItemType
    {
        MAIN,
        CONSUMABLE,
        OTHER,
    }
}

[System.Serializable]
public class Consumable : Item
{

}
*/