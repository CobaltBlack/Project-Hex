using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Default,
        Equipment,
        Consumable,
    }

    public ItemType Type { get; set; }
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public int Rarity { get; set; }
    public bool Stackable { get; set; }
    public string Slug { get; set; }

    public Sprite Sprite { get; set; }

    public Item(ItemType type, int id, string title,string description, int value, int rarity, bool stackable, string slug)
    {
        this.Type = type;
        this.ID = id;
        this.Title = title;
        this.Description = description;
        this.Value = value;
        this.Rarity = rarity;
        this.Stackable = stackable;
        this.Slug = slug;

        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    // no item in slot case
    public Item()
    {
        this.ID = -1;
    }

    public class Equipment : Item
    {
        public int MaxHp { get; set; }
        public int ActionPoints { get; set; }
        public int MoralityFlux { get; set; }
        public int SanityFlux { get; set; }

        public int Attack { get; set; }
        public int Crit { get; set; }

        public int Defense { get; set; }
        public int Dodge { get; set; }

        public Equipment(ItemType type, int id, string title, string description, int value, int rarity, bool stackable, string slug 
            ,int maxHp, int actionPoint, int moralityFlux, int sanityFlux, int attack, int crit, int defense, int dodge)
        {
            this.Type = type;
            this.ID = id;
            this.Title = title;
            this.Description = description;
            this.Value = value;
            this.Rarity = rarity;
            this.Stackable = stackable;
            this.Slug = slug;

            this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);

            this.MaxHp = maxHp;
            this.ActionPoints = actionPoint;
            this.MoralityFlux = moralityFlux;
            this.SanityFlux = sanityFlux;
            this.Attack = attack;
            this.Crit = crit;
            this.Defense = defense;
            this.Dodge = dodge;
        }
    }

    public class Consumable : Item
    {
        public string FunctionName { get; set; }
        public int FunctionParameter { get; set; }

        public Consumable(ItemType type, int id, string title, string description, int value, int rarity, bool stackable, string slug, string functionName, int functionParameter)
        {
            this.Type = type;
            this.ID = id;
            this.Title = title;
            this.Description = description;
            this.Value = value;
            this.Rarity = rarity;
            this.Stackable = stackable;
            this.Slug = slug;

            this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);

            this.FunctionName = functionName;
            this.FunctionParameter = functionParameter;
        }
    }

    public class Token : Item
    {

    }
}