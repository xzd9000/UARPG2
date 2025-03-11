using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class HitObjPool
{
    private HittingObject _owner;
    private bool _usePool;
    private List<HittingObject> _hitObjs;

    public HitObjPool(HittingObject owner, bool reuse, int capacity = 0)
    {
        _owner = owner;
        _usePool = reuse;
        if (_usePool) _hitObjs = new List<HittingObject>(capacity);
    }

    public HittingObject Spawn() => Spawn(_owner.transform.position, _owner.transform.rotation);
    public HittingObject Spawn(Vector3 position, Quaternion rotation)
    {
        if (!_usePool) return instantiate();
        else
        {
            if (_hitObjs.Count > 0)
            {
                HittingObject ret = _hitObjs[_hitObjs.Count - 1];
                _hitObjs.RemoveAt(_hitObjs.Count - 1);
                ret.Reset(position, rotation);
                return ret;
            }
            else return instantiate();
        }
;

        HittingObject instantiate()
        {
            HittingObject ret = HittingObject.Instantiate(_owner);
            ret.Reset(position, rotation);
            return ret;
        }
    }

    public void Return(HittingObject instance)
    {
        if (_usePool)
        {
            _hitObjs.Add(instance);
            instance.Disable();
        }
        else HittingObject.Destroy(instance.gameObject);
    }
}

public abstract class InstantiatableHitObjectBase : HittingObject
{
    [SerializeField] bool _reusable = true;
    [SerializeField] [HideUnless("_reusable", true)] int _poolCapacity = 100;
    [SerializeField] bool _noInstall = true;

    [SerializeField] bool _autoDestroy = true;
    [SerializeField] [HideUnless("_autoDestroy", true)] float _autoDestroyTime = 4f;

    public HitObjPool pool { get; private set; }

    private void Awake() => InitializeIfNeeded();

    protected void InitializeIfNeeded()
    {
        InitializeBase();
        InitializeCustom();  
    }
    protected virtual void InitializeBase() { if (pool == null) pool = new HitObjPool(this, _reusable, _poolCapacity); }
    protected virtual void InitializeCustom() { }

    public override void Activate() => Activate(new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity), new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
    public override void Activate(Vector3 position) => Activate(position, new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity)); 
    public override void Activate(Vector3 position, Quaternion rotation)
    {
        InitializeIfNeeded();
        HittingObject proj = pool.Spawn(position, rotation);
        proj.Enable();
        if (_autoDestroy) Observable.Timer(TimeSpan.FromSeconds(_autoDestroyTime)).Subscribe(_ => pool.Return(proj));
    }
}
