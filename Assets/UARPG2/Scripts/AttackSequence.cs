using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

[Serializable] public struct ProjectileDataInfo
{
    public bool projFromLinks;
    public Vector2Int projDataIndex;

    public bool usePosition;
    public bool positionFromLinks;
    public Vector2Int positionDataIndex;

    public bool useRotation;
    public bool rotationFromLinks;
    public Vector2Int rotationDataIndex;
}

[Serializable] public struct AttackAction
{
    public bool damageTarget;

    public bool activateProjectiles;
    public ProjectileDataInfo[] projInfo;
}

public class AttackSequence : ScriptableObject
{
    public virtual void Play(ICharacter character, Attack.Data[] data, Links[] linkedData)
    {
        throw new NotImplementedException();        
    }
    public virtual void Abort(ICharacter character)
    {
        throw new NotImplementedException();
    }  
}
