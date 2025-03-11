using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AIController : MonoBehaviour
{
    [SerializeField] AI _ai;
    [SerializeField] protected MonoBehaviour _target;

    protected AI ai
    {
        get => _ai;
        set
        {
            _ai = value;
            InitAI();
        }
    }

    public ICharacter owner { get; protected set; }
    public bool targetExists { get; protected set; }

    public ICharacter target => (ICharacter)_target;

    public void SetTarget(MonoBehaviour target)
    {
        if (target is ICharacter)
        {
            _target = target;
            targetExists = true;
        }
        else throw new System.ArgumentException("Target is not ICharacter");
    }
    public void ClearTarget()
    {
        _target = null;
        targetExists = false;
    }

    [Inject] void Initialize(ICharacter character)
    {
        owner = character;
        InitAI();
    }

    private void InitAI()
    {
        _ai.owner = owner;
        _ai.ctl = this;
        _ai.Initialize();
    }

    protected virtual void Update() => _ai.BaseAction(this, owner);
}
