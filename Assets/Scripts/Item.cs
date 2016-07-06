using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{

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
