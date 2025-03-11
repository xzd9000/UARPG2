using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [Flags] public enum ReplaceFlags
    {
        failed = 0,
        newEquiped  = 0b01,
        oldUneqiped = 0b10,
    }

    [Serializable] public struct EquipmentSlot
    {
        public ItemSlots slot;
        public EquipableItem item;
        public bool changeMesh;
        [HideUnless("changeMesh", true)] public MeshFilter meshFilter;
        [HideUnless("changeMesh", true)] public Mesh defaultMesh;
        public bool addGameObjects;
        [HideUnless("addGameObjects", true)] public bool newParent;
        [HideUnless("addGameObjects", true)] public GameObject parent;
        [HideUnless("addGameObjects", true)][EditorReadOnly] public GameObject[] addedObjects;
    }

    [SerializeField] EquipmentSlot[] _slots;
    [SerializeField] bool _useEquipmentStats = true;

    public int length => _slots.Length;
    public EquipmentSlot this[int i] => _slots[i];

    public bool useEquipmentStats => _useEquipmentStats;

    public bool TryEquip(EquipableItem item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (TryEquipAt(item, i)) return true;
        }

        return false;
    }
    public bool TryEquipAt(EquipableItem item, int index)
    {
        if (_slots[index].slot == item.slot && _slots[index].item == null)
        {
            EquipAt(item, index);
            return true;
        }
        return false;
    }
    public bool TryUnequip(ItemSlots slot, out EquipableItem item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].slot == slot && _slots[i].item != null)
            {
                UnequipAt(i, out item);
                return true;
            }
        }

        item = null;
        return false;
    }
    public bool TryUnequipAt(int index, out EquipableItem item)
    {
        if (_slots[index].item != null)
        {
            UnequipAt(index, out item);
            return true;
        }

        item = null;
        return false;
    }
    public ReplaceFlags TryReplace(EquipableItem newItem, out EquipableItem oldItem)
    {
        ReplaceFlags result;

        for (int i = 0; i < _slots.Length; i++)
        {
            result = TryReplaceAt(newItem, out oldItem, i);
            if (result != ReplaceFlags.failed) return result;
        }

        oldItem = null;
        result = ReplaceFlags.failed;
        return result;
    }
    public ReplaceFlags TryReplaceAt(EquipableItem newItem, out EquipableItem oldItem, int index)
    {
        int result = 0;
        oldItem = null;

        if (_slots[index].slot == newItem.slot)
        {                                           
            if (_slots[index].item != null)
            {
                UnequipAt(index, out oldItem);
                result |= (int)ReplaceFlags.oldUneqiped;
            }

            EquipAt(newItem, index);
            result |= (int)ReplaceFlags.newEquiped;
        }

        return (ReplaceFlags)result;
    }

    private void EquipAt(EquipableItem item, int index)
    {
        _slots[index].item = item;

        ItemEquiped?.Invoke(_slots[index], item);
        EquipmentChanged?.Invoke();
    }
    private void UnequipAt(int index, out EquipableItem item)
    {
        item = _slots[index].item;
        _slots[index].item = null;

        ItemUnequiped?.Invoke(_slots[index], item);
        EquipmentChanged?.Invoke();
    }

    public event Action<EquipmentSlot, EquipableItem> ItemEquiped;
    public event Action<EquipmentSlot, EquipableItem> ItemUnequiped;
    public event Action EquipmentChanged;
}
