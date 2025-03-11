using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public struct StateActions
    {
        public Action<AIController, ICharacter> stateEnter;
        public Action<AIController, ICharacter> stateAction;
        public Action<AIController, ICharacter> stateExit;

        public StateActions(Action<AIController, ICharacter> stateEnter, Action<AIController, ICharacter> stateAction, Action<AIController, ICharacter> stateExit)
        {
            this.stateEnter = stateEnter;
            this.stateAction = stateAction;
            this.stateExit = stateExit;
        }
    }

    [SerializeField] protected float _targetReachDistance = 0.1f;
    [SerializeField] protected float _targetingAngle = 1f;
    [SerializeField] protected bool __3dTargeting = false;

    protected int _stateIndex;
    protected StateActions[] _states;

    public float targetReachDistance => _targetReachDistance;
    public float targetingAngle => _targetingAngle;
    public int currentState => _stateIndex;
    public bool _3dTargeting => __3dTargeting;

    [HideInInspector] public AIController ctl;
    [HideInInspector] public ICharacter owner;

    public virtual void Initialize() { }

    public void BaseAction(AIController ownerCtl, ICharacter owner) => _states[_stateIndex].stateAction(ownerCtl, owner);

    public void ChangeState(AIController ownerCtl, ICharacter owner, int index)
    {
        _states[_stateIndex].stateExit(ownerCtl, owner);
        _stateIndex = index;
        _states[index].stateEnter(ownerCtl, owner);
    }
}
