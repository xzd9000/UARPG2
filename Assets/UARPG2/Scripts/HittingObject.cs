using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public interface IHitFunction { public void Hit(HittingObject owner, Collider contacted); }

public abstract class HittingObject : MonoBehaviour
{
    [Flags] public enum OnContact
    {
        nothing = 0,
        destroyThis      = 0b_00_00_01,
        destroyContacted = 0b_00_00_10,
        disableThis      = 0b_00_01_00,
        disableContacted = 0b_00_10_00,
        deflectThis      = 0b_01_00_00,
        deflectContacted = 0b_10_00_00
    }

    [SerializeField] Damage _baseDamage;
    [SerializeField] OnContact _contactFlags;

    public Damage statusDamage;
    public virtual int contactFlags => (int)_contactFlags;

    public Damage damage
    {
        get
        {
            _finalDamage.Override(0f);
            _finalDamage.Add(_baseDamage);
            _finalDamage.Add(statusDamage);
            _finalDamage.Clamp(0f, false);
            return _finalDamage;
        }
        private set => _baseDamage = value;
    }

    protected Damage _finalDamage;
    protected Action<HittingObject, Collider> _hitFunction;

    [Inject] void InitDependencies(Action<HittingObject, Collider> hitFunction)
    {
        _hitFunction = hitFunction;
    }

    public abstract void Activate();
    public abstract void Activate(Vector3 position);
    public abstract void Activate(Vector3 position, Quaternion rotation);
    public virtual void Deactivate() { }

    public virtual void Hit(Collider contacted) => _hitFunction(this, contacted); 

    public virtual void Reset(Vector3 position, Quaternion rotation)
    {
        if (!float.IsNegativeInfinity(position.x)) transform.position = position;
        if (!float.IsNegativeInfinity(rotation.x)) transform.rotation = rotation;
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }
    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}
