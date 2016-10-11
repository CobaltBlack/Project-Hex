using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/*
 * ItemData 
 * 
 * Attach this script to UI "Item" GameObject.
 * Item stacking and click and drags are handled here.  
 * 
 */

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slotIndex;
    public SlotType slotType;

    //private Transform originalParent;
    private Vector2 offset;

    PlayerManager playerManagerScript;

    InventoryManager inventoryManagerScript;
    ShopManager shopManagerScript;
    EquipmentManager equipmentManagerScript;

    ItemBehaviours itemBehavioursScript;
    InventoryTooltip tooltipScript;

    void Start()
    {
        playerManagerScript = GameObject.Find("MapManager").GetComponent<PlayerManager>();

        inventoryManagerScript = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        shopManagerScript = inventoryManagerScript.GetComponent<ShopManager>();
        equipmentManagerScript = inventoryManagerScript.GetComponent<EquipmentManager>();

        itemBehavioursScript = inventoryManagerScript.GetComponent<ItemBehaviours>();
        tooltipScript = inventoryManagerScript.GetComponent<InventoryTooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // to avoid visual conflict, move to the slot panel while moving
            //originalParent = this.transform.parent; // never used
            this.transform.SetParent(this.transform.parent.parent.parent.parent); // dw

            // to avoid mouse from grabbing the center of the item, instead grab where it is clicked
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.position = eventData.position - offset;

            // while item is being dragged, turn off blockRaycasts so raycast can hit slots
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.slotType == SlotType.PlayerInv)
        {
            this.transform.SetParent(inventoryManagerScript.invSlots[slotIndex].transform);
            this.transform.position = inventoryManagerScript.invSlots[slotIndex].transform.position;
        }

        if (this.slotType == SlotType.ShopInv)
        {
            this.transform.SetParent(shopManagerScript.shopSlots[slotIndex].transform);
            this.transform.position = shopManagerScript.shopSlots[slotIndex].transform.position;
        }

        if (this.slotType == SlotType.EquipmentInv)
        {
            this.transform.SetParent(equipmentManagerScript.equipSlots[slotIndex].transform);
            this.transform.position = equipmentManagerScript.equipSlots[slotIndex].transform.position;
        }

        // update stats in case equipment is added or removed
        playerManagerScript.RefreshPlayerStats();

        GetComponent<CanvasGroup>().blocksRaycasts = true; // turn blockRaycasts back on
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipScript.ActivateTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipScript.DeactivateTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (shopManagerScript.shopPanel.activeSelf)
            {
                tooltipScript.DeactivateTooltip(); // close tool tip

                // BUY
                if (this.slotType == SlotType.ShopInv)
                {
                    // if sufficient gold, remove gold and execute following
                    if (inventoryManagerScript.RemoveGold(this.item.Value))
                    {
                        inventoryManagerScript.AddItem(this.item.ID);
                        Destroy(this.gameObject);

                        // update list
                        shopManagerScript.shopItems[this.slotIndex] = new Item();

                        return;
                    }
                }

                // SELL
                else if (this.slotType == SlotType.PlayerInv)
                {
                    inventoryManagerScript.AddGold(this.item.Value);
                    Destroy(this.gameObject);

                    // update list
                    inventoryManagerScript.invItems[this.slotIndex] = new Item();

                    return;
                }
            }

            else if (this.item.Type == Item.ItemType.Consumable)
            {
                Item.Consumable consumable = (Item.Consumable)item;
                itemBehavioursScript.ExecuteByName(consumable.FunctionName, consumable.FunctionParameter);

                Destroy(gameObject);

                inventoryManagerScript.invItems[this.slotIndex] = new Item();

                return;
            }

            else if (inventoryManagerScript.equipmentPanel.activeSelf)
            {
                if (this.item.Type == Item.ItemType.Equipment)
                {
                    tooltipScript.DeactivateTooltip(); // close tool tip

                    // DEQUIP
                    if (this.slotType == SlotType.EquipmentInv)
                    {
                        inventoryManagerScript.AddItem(this.item.ID);
                        Destroy(this.gameObject);

                        equipmentManagerScript.equipItems[this.slotIndex] = new Item();

                        // update stats according to dequip
                        playerManagerScript.RefreshPlayerStats();

                        return;
                    }

                    // EQUIP
                    else if (this.slotType == SlotType.PlayerInv)
                    {
                        equipmentManagerScript.AddItem(this.item.ID);
                        Destroy(this.gameObject);

                        inventoryManagerScript.invItems[this.slotIndex] = new Item();

                        // update stats according to equip
                        playerManagerScript.RefreshPlayerStats();

                        return;
                    }
                }
            }
        }
    }
}