using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class Projectile : InstantiatableHitObjectBase
{
    [SerializeField] Collider _collider;

    private IDisposable _collisionSubscription = null;

    protected override void InitializeCustom() => HitObjectFunctions.Default.CheckCollider(this, ref _collisionSubscription, ref _collider);
}