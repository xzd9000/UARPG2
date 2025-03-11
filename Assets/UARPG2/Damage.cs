using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct TypedDamage : IEnumValue<DamageType>
{
    [SerializeField] DamageType _type;
    [SerializeField] float _value;
    [SerializeField] bool _percentage;

    public DamageType type { get => _type; set => _type = value; }
    public float value { get => _value; set => _value = value; }
    public bool percentage { get => _percentage; set => _percentage = value; }

    public TypedDamage(DamageType type, float value, bool percentage)
    {
        _type = type;
        _value = value;
        _percentage = percentage;
    }
}

[System.Serializable] public class Damage : CharacterData<Damage, DamageType, TypedDamage>
{
    [SerializeField] List<TypedDamage> _values = new List<TypedDamage>();

    public override List<TypedDamage> values { get => _values; set => _values = value; }

    public Damage() { }
    public Damage(params (DamageType, bool)[] stats)
    {
        _values = new List<TypedDamage>(stats.Length);
        for (int i = 0; i < stats.Length; i++)
        {
            TypedDamage value = new TypedDamage();
            value.type = stats[i].Item1;
            value.percentage = stats[i].Item2;
            value.value = 0;
        }
    }

    public float Sum() 
    {
        float ret = 0;
        for (int i = 0; i < values.Count; i++) ret += values[i].value;
        return ret;
    }
}
