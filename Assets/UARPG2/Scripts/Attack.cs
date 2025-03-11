using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public struct Attack
{

    [Serializable] public struct Data
    {
        [Flags] public enum Types
        {
            none = 0,
            float_ = 0b1,
            vector3 = 0b10,
            object_ = 0b100,

            several = 1 << 31
        }

        public string name;
        public Types type;
        public float floatValue;
        public Vector3 vector3Value;
        public UnityEngine.Object objectValue;

        public object value
        {
            get
            {
                if (!type.HasFlag(Types.several))
                {
                    if (type == Types.float_) return floatValue;
                    if (type == Types.vector3) return vector3Value;
                    if (type == Types.object_) return objectValue;
                    else return null;
                }
                else
                {
                    List<object> ret = new List<object>(3);
                    if (type.HasFlag(Types.float_)) ret.Add(floatValue);
                    if (type.HasFlag(Types.vector3)) ret.Add(vector3Value);
                    if (type.HasFlag(Types.object_)) ret.Add(objectValue);

                    ret.Capacity = ret.Count;
                    return ret.ToArray();
                }
            }
        }
    }

    [Serializable] public struct Area
    {
        public enum Type
        {
            circle = 0,
            sphere = 1,
            box = 2,
            cylinder = 3
        }

        public Type type;
        public Vector3 offset;
        public float angle;
        [HideUnless("type", typeof(Enum), 0, 1, 3)] public float radius;
        [HideUnless("type", typeof(Enum), 2)] public Vector3 halfDimensions;
        [HideUnless("type", typeof(Enum), 2)] public Vector3 rotation;
        [HideUnless("type", typeof(Enum), 2)] public bool globalRotation;
        [HideUnless("type", typeof(Enum), 3)] public float height;
    }

    public AttackSequence sequence;
    public Data[]         sequenceData;
    public Links[]        linkedData;

    public bool useDetectors;
    public Vector2Int[] detectorsIndexes;

    public bool useAreas;
    public Area[] areas;
}