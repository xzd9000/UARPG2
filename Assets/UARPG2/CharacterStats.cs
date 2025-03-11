using System;
using System.Collections.Generic;
using UnityEngine;
using static System.Convert;

[Serializable] public struct StatValue : IEnumValue<CharacterStat>
{
    [SerializeField] CharacterStat _type;
    [SerializeField] float _value;
    [SerializeField] bool _percentage;

    public CharacterStat type { get => _type; set => _type = value; }
    public float value { get => _value; set => _value = value; }
    public bool percentage { get => _percentage; set => _percentage = value; }

    public StatValue(CharacterStat type, float value) : this()
    {
        this.type = type;
        this.value = value;
    }
}

[Serializable] public class CharacterStats : CharacterData<CharacterStats, CharacterStat, StatValue>
{
    [SerializeField] bool _percentage;
    [SerializeField] List<StatValue> _values = new List<StatValue>();

    public override List<StatValue> values { get => _values; set => _values = value; }

    public CharacterStats() { }
    public CharacterStats(params (CharacterStat, bool)[] stats)
    {
        _values = new List<StatValue>(stats.Length);
        for (int i = 0; i < stats.Length; i++)
        {
            StatValue value = new StatValue();
            value.type = stats[i].Item1;
            value.percentage = stats[i].Item2;
            value.value = 0;
        }
    }
}