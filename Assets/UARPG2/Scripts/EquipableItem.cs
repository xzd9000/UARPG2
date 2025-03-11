using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] ItemSlots _slot;
    [SerializeField] Mesh _mesh;
    [SerializeField] CharacterStats _stats;

    public int id => _id;
    public ItemSlots slot => _slot;
    public Mesh mesh => _mesh;
    public CharacterStats stats => _stats;

    [ContextMenu("SetID")]
    private void SetID() => Global.GenerateID();

    public static bool operator==(EquipableItem first, EquipableItem second) => first._id == second._id;
    public static bool operator!=(EquipableItem first, EquipableItem second) => first._id != second._id;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;      

        return _id == ((EquipableItem)obj)._id;
    }
    public override int GetHashCode() => _id;
}
