using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultInventory : Inventory
{
    [Serializable] public struct InventoryItem
    {
        public EquipableItem item;
        public int quantity;

        public InventoryItem(EquipableItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
    [SerializeField] List<InventoryItem> _items;

    public InventoryItem this[int i] => _items[i];
    public int length => _items.Count;

    public override bool TryAddItem(EquipableItem item) => TryAddItems(item, 1) > 0;
    public override int TryAddItems(EquipableItem item, int quantity)
    {
        if (item != null)
        {
            if (quantity > 0)
            {
                int index;
                if ((index = _items.FindIndex((iitem) => iitem.item == item)) >= 0)
                    _items[index] = new InventoryItem(_items[index].item, _items[index].quantity + quantity);
                else _items.Add(new InventoryItem(item, quantity));
            }
            return quantity;
        }
        else throw new System.NullReferenceException();
    }

    public override bool TryTakeItem(EquipableItem item) => TryTakeItems(item, 1) > 0;
    public override int TryTakeItems(EquipableItem item, int quantity)
    {
        if (quantity > 0)
        {
            int index;
            if ((index = _items.FindIndex((iitem) => iitem.item == item)) >= 0) return TakeItemsAt(index, quantity);
            else return -1;
        }
        else return 0;
    }
    public override bool TryTakeItemAt(int index, out EquipableItem item) => TryTakeItemsAt(index, out item, 1) > 0;
    public override int TryTakeItemsAt(int index, out EquipableItem item, int quantity)
    {
        item = _items[index].item;
        return TakeItemsAt(index, quantity);
    }

    private int TakeItemsAt(int index, int quantity)
    {
        int ret;
        if (_items[index].quantity > quantity)
        {
            _items[index] = new InventoryItem(_items[index].item, _items[index].quantity - quantity);
            ret = quantity;
        }
        else
        {
            if (_items[index].quantity == quantity) ret = quantity;
            else ret = _items[index].quantity;

            _items.RemoveAt(index);
        }
        return ret;
    }

    public override (EquipableItem, bool)[] TransferItems(Inventory destination)
    {
        throw new System.NotImplementedException();
    }
    public override (EquipableItem, bool)[] CopyItems(Inventory destination)
    {
        throw new System.NotImplementedException();
    }
}
