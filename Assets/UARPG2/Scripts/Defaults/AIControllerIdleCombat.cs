using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AIControllerIdleCombat : AIController
{
    [SerializeField] protected bool _combat;
    [SerializeField] protected AI _idleAI;
    [SerializeField] protected AI _combatAI;
    [SerializeField] protected bool _combatAtStart;
    [SerializeField] protected bool _generateDetector;
    [SerializeField][HideUnless("_generateDetector", true)] protected int _detectorLayer;
    [SerializeField][HideUnless("_generateDetector", true)] protected float _detectorRadius;

    public virtual bool combat
    {
        get => _combat;
        set
        {
            _combat = value;
            if (_combat) ai = _combatAI;
            else         ai = _idleAI;
        }
    }

    [SerializeField] protected Collider _enemyDetector;

    protected virtual void Awake()
    {
        AwakeBase();
        AwakeCustom();
    }
    protected virtual void AwakeBase()
    {
        if (_combatAtStart) combat = true;
        else combat = false;

        if (_generateDetector) GenerateDetector();

        if (_enemyDetector != null) _enemyDetector.OnTriggerEnterAsObservable().Subscribe((c) => EnemyDetect(c));
    }
    protected virtual void AwakeCustom() { }

    [ContextMenu("Set Combat")]
    private void SetCombat() => combat = true;

    [ContextMenu("Set Idle")]
    private void SetIdle() => combat = false;

    protected virtual void EnemyDetect(Collider collider) 
    {
        if (!combat)
        {
            if (collider.TryGetComponent(out ICharacter other))
            {
                if (!targetExists && owner.faction.Conflict(other))
                {
                    SetTarget((MonoBehaviour)other);
                    combat = true;
                }
            }
        }
    }

    protected void GenerateDetector()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(obj.GetComponent<MeshRenderer>());
        Destroy(obj.GetComponent<MeshFilter>());
        SphereCollider sphere = obj.GetComponent<SphereCollider>();
        sphere.radius = _detectorRadius;
        sphere.gameObject.layer = _detectorLayer;
        sphere.isTrigger = true;
        _enemyDetector = sphere;
        obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        obj.transform.SetParent(transform);
    }
}
