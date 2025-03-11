using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public abstract bool TryAddItem(EquipableItem item);
    public abstract int TryAddItems(EquipableItem item, int quantity);

    public abstract bool TryTakeItem(EquipableItem item);
    public abstract int TryTakeItems(EquipableItem item, int quantity);
    public abstract bool TryTakeItemAt(int index, out EquipableItem item);
    public abstract int TryTakeItemsAt(int index, out EquipableItem item, int quantity);

    public abstract (EquipableItem, bool)[] TransferItems(Inventory destination);
    public abstract (EquipableItem, bool)[] CopyItems(Inventory destination);
}
