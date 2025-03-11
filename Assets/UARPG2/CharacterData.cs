using System;
using System.Collections.Generic;
using static System.Convert;

public interface IEnumValue<T>
{
    T type { get; set; }
    float value { get; set; }
    bool percentage { get; set; }
}

public abstract class CharacterData<TDataClass, TDataTypesEnum, TEnumValue> where TDataClass : CharacterData<TDataClass, TDataTypesEnum, TEnumValue> where TEnumValue : IEnumValue<TDataTypesEnum>, new() where TDataTypesEnum : Enum
{
    public abstract List<TEnumValue> values { get; set; }

    public virtual float FindValue(TDataTypesEnum type, bool percentage = false)
    {
        int index;
        if ((index = FindIndex(type, percentage)) >= 0) return values[index].value;
                                                        return float.NegativeInfinity;
    }
    public virtual int FindIndex(TDataTypesEnum type, bool percentage = false) => values.FindIndex((v) => v.type.Equals(type) && v.percentage == percentage);

    public virtual float this[TDataTypesEnum type, bool percentage = false]
    {
        get => values[FindIndex(type)].value;
        set => values[FindIndex(type)].value = value;
    }

    public virtual void Add(TEnumValue value, bool addNew = false)
    {
        int index;
        if ((index = FindIndex(value.type, value.percentage)) >= 0) values[index].value += value.value;
        else if (addNew) values.Add(value);
    }

    public virtual void Multiply(float second) => CommitOperation(second, (value, second_) => value * second_);
    public virtual void Add(TDataClass second) => CommitOperation(second, (first, second_) => first + second_);
    public virtual void Subtract(TDataClass second) => CommitOperation(second, (first, second_) => first - second_);
    

    public virtual void Override(TDataClass override_) => CommitOperation(override_, (value, override__) => override__);
    public virtual void Override(float override_) => CommitOperation(override_, (value, override__) => override__);

    public void Clamp(float value, bool max) => Clamp<float>(value, max);
    public void Clamp(TDataClass value, bool max) => Clamp<TDataClass>(value, max);
    public void Clamp(TDataClass min, TDataClass max, bool clampMin = true, bool clampMax = true) => Clamp<TDataClass>(min, max, clampMin, clampMax);
    public void Clamp(float min, float max, bool clampMin = true, bool clampMax = true) => Clamp<float>(min, max, clampMin, clampMax);
    private void Clamp<T>(T value, bool max)
    {
        if (max) Clamp(default, value, false, true);
        else Clamp(value, default, true, false);
    }
    protected virtual void Clamp<T>(T min, T max, bool clampMin, bool clampMax)
    {
        object min_;
        object max_;
        Func<float, float, float>[] ops =
        {
            (value, min___) =>
            {
                if (value < min___) return min___;
                else return value;
            },
            (value, max___) =>
            {
                if (value > max___) return max___;
                else return value;
            }
        };
        if (min is float min__ && max is float max__)
        {
            min_ = min__;
            max_ = max__;
            if (clampMin) CommitOperation(min__, ops[0]);
            if (clampMax) CommitOperation(max__, ops[1]);
        }
        else if (min is TDataClass min___ && max is TDataClass max___)
        {
            min_ = min___;
            max_ = max___;
            if (clampMin) CommitOperation(min___, ops[0]);
            if (clampMax) CommitOperation(max___, ops[1]);
        }
        else return;        
    }

    /// <param name="operation">first param is this value, second is operands value</param>
    protected void CommitOperation(float operand, Func<float, float, float> operation) { for (int i = 0; i < values.Count; i++) values[i].value = operation(values[i].value, operand); }

    /// <param name="operation">first param is this value, second is operands value</param>
    protected void CommitOperation(TDataClass operandValues, Func<float, float, float> operation)
    {
        if (operandValues != null)
        {
            if (operandValues.values != null)
            {
                for (int i = 0; i < operandValues.values.Count; i++)
                {
                    for (int ii = 0; ii < values.Count; ii++)
                    {
                        if (values[ii].type.Equals(operandValues.values[i].type) && values[ii].percentage == operandValues.values[i].percentage)
                        {
                            values[ii].value = operation(values[ii].value, operandValues.values[i].value);
                            break;
                        }
                    }
                }
            }
        }
    }

    public virtual void CopyFrom(TDataClass data)
    {
        if (data != null)
        {
            values = new List<TEnumValue>(data.values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = new TEnumValue();
                values[i].type = data.values[i].type;
                values[i].value = data.values[i].value;
                values[i].percentage = data.values[i].percentage;
            }
        }
    }

    public float this[int i]
    {
        get
        {
            if (values != null) if (i >= 0 && i < values.Count) return values[i].value;
            return float.NegativeInfinity;
        }
        set  { if (values != null) if (i >= 0 && i < values.Count) values[i].value = value; }
    }

    public virtual void CreateEmptyAllTypes()
    {
        Array values = Enum.GetValues(typeof(TDataTypesEnum));
        this.values = new List<TEnumValue>(values.Length * 2);
        TEnumValue value;
        for (int i = 0; i < values.Length; i++)
        {
            for (int ii = 0; ii < 2; ii++)
            {
                value = new TEnumValue();
                value.type = (TDataTypesEnum)values.GetValue(i);
                value.value = 0f;
                value.percentage = ii != 0;
                this.values.Add(value);
            }
        }
    }

    public override string ToString()
    {
        string ret = "";
        for (int i = 0; i < values.Count; i++)
        {
            ret += values[i].value + (values[i].percentage ? "%" : "") + " " + values[i].type;
            if (i < values.Count - 1) ret += "\n";
        }
        return ret;
    }
}