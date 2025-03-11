using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Melee : HittingObject
{
    [SerializeField] Collider _collider;
    [SerializeField] bool _autoDisable = true;
    [SerializeField] float _autoDisableTime = 0.4f;

    protected IDisposable _autoDisableObservable;
    protected IDisposable _collisionSubscription;

    private void Awake() => HitObjectFunctions.Default.CheckCollider(this, ref _collisionSubscription, ref _collider);

    public override void Activate() => Activate(Vector3.negativeInfinity, new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
    public override void Activate(Vector3 position) => Activate(position, new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
    public override void Activate(Vector3 position, Quaternion rotation)
    {
        if (!float.IsNegativeInfinity(position.x)) transform.position = position;
        if (!float.IsNegativeInfinity(rotation.x)) transform.rotation = rotation;
        Enable();
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        if (_autoDisable)
        {
            _autoDisableObservable = Observable.Timer(TimeSpan.FromSeconds(_autoDisableTime)).Subscribe(
                _ =>
                {
                    _autoDisableObservable = null;
                    Disable();
                });
        }
    }
    public override void Disable()
    {
        if (_autoDisableObservable != null)
        {
            _autoDisableObservable.Dispose();
            _autoDisableObservable = null;
        }
        gameObject.SetActive(false);
    }
}
