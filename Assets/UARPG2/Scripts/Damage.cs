//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface IDamageDataProcessor
//{
//    public float Sum(Damage damage);
//    public Damage Add(Damage first, Damage second);
//    public Damage Subtract(Damage first, Damage second);
//    public Damage Multiply(Damage first, Damage second);
//    public Damage Multiply(Damage first, float second);

//    public Damage Negate(Damage damage);

//    public Damage CutNegative(Damage damage);
//}

//[System.Serializable] public struct TypedDamage
//{
//    public DamageTypes type;
//    public float value;
//    public bool percentage;

//    public TypedDamage(DamageTypes type, float value, bool percentage)
//    {
//        this.type = type;
//        this.value = value;
//        this.percentage = percentage;
//    }
//}

//[System.Serializable] public struct Damage
//{
//    public TypedDamage[] damage;

//    public int typesLength => damage.Length;

//    public TypedDamage this[int i]
//    {
//        get => damage[i];
//        set => damage[i] = value;      
//    }

//    public float GetValue(int typeID) => damage.Find((dmg) => (int)dmg.type == typeID).value;
//    public void  SetValue(int typeID, float value) => damage[damage.FindIndex((dmg) => (int)dmg.type == typeID)].value = value;
//    public float GetValueAt(int index) => damage[index].value;
//    public void  SetValueAt(int index, float value) => damage[index].value = value;

//    public int FindIndex(int typeID, bool percentage) => damage.FindIndex((dmg) => (int)dmg.type == typeID && dmg.percentage == percentage);

//    public Damage(params TypedDamage[] dmg)
//    {
//        damage = new TypedDamage[dmg.Length];
//        for (int i = 0; i < dmg.Length; i++) damage[i] = dmg[i];
//    }
//    public Damage(Damage other)
//    {
//        damage = new TypedDamage[other.damage.Length];
//        for (int i = 0; i < damage.Length; i++)
//        {
//            damage[i] = new TypedDamage
//               (other.damage[i].type, 
//                other.damage[i].value, 
//                other.damage[i].percentage);
//        }
//    }

//    public override string ToString()
//    {
//        string ret = "Damage:\n";
//        for (int i = 0; i < damage.Length; i++) ret += damage[i].type + ": " + damage[i].value + "\n";
//        return ret;
//    }
//}
