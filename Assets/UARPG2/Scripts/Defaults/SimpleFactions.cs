using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class SimpleFactions : IFactionInfo
{
    [SerializeField] bool enemy;

    public int faction
    {
        get => enemy ? 1 : 0;
        set { if (value > 0) enemy = true; else enemy = false; }
    }

    public bool Conflict(ICharacter other) => other.faction.faction != faction;
}
