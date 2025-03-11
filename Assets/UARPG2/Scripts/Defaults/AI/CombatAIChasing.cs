using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAIChasing : AI
{
    public enum State
    {
        chasing = 0,
        returning = 1
    }

    [SerializeField] State _state; 
    [SerializeField] bool _guarding;

    private Vector3 _startPosition;
    private Vector3 _startDirection;
    

    private void Awake()
    {       
        _states = new StateActions[_guarding ? 2 : 1];
        _states[(int)State.chasing] = new StateActions
            (
                stateEnter: (ctl, c) => { },
                stateExit: (ctl, c) => { },
                stateAction: (ctl, owner) =>
                {
                    if ((owner.stateFlags & (int)ICharacter.DefaultStateFlags.dead) == 0)
                    {
                        if (ctl.targetExists)
                        {
                            AIFunctions.Default.RotateToTarget(this, owner, ctl.target.transform.position, owner.rotationSpeed, targetingAngle, false, __3dTargeting);
                            if (AIFunctions.Default.MoveToTarget(this, owner, ctl.target.transform.position, new Vector3(0f, 0f, owner.moveSpeed * Time.deltaTime), Space.Self))
                                owner.Attack(AIFunctions.Default.RandomAttack(owner));
                        }
                        else if (_guarding) ChangeState(ctl, owner, (int)State.returning);
                    }
                }
            );
        if (_guarding)
        {
            _states[(int)State.returning] = new StateActions
            (
                stateEnter: (ctl, c) => { },
                stateExit: (ctl, c) => { },
                stateAction: (ctl, owner) =>
                {
                    if ((owner.stateFlags & (int)ICharacter.DefaultStateFlags.dead) == 0)
                    {
                        if (ctl.targetExists) ChangeState(ctl, owner, (int)State.chasing);
                        else
                        {
                            if (AIFunctions.Default.MoveToTarget(this, owner, _startPosition, new Vector3(0f, 0f, owner.moveSpeed * Time.deltaTime), Space.Self) &&
                                AIFunctions.Default.RotateToTarget(this, owner, _startDirection, owner.rotationSpeed, targetingAngle, true, __3dTargeting))
                            {
                                ChangeState(ctl, owner, (int)State.chasing);
                                if (ctl is AIControllerIdleCombat combatCtl) combatCtl.combat = false;
                            }
                        }
                    }
                }
            );
            _startPosition = transform.position;
            _startPosition = transform.forward;
        }
    }
}
