using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Attacks/Phased")]
public class PhasedAttackSequence : AttackSequence
{
    public enum AttackActivation
    {
        no,
        onStart,
        onEndBeforeHold,
        onEndAfterHold
    }
    [Serializable] public struct AnimationPhase
    {
        public bool targeting;

        public bool hold;
        [HideUnless("hold", true)] public float holdDuration;

        public bool attackMovement;
        [HideUnless("attackMovement", true)] public Vector2Int movementDataIndex;
        [HideUnless("attackMovement", true)] public bool movementFromLinks;
        [HideUnless("attackMovement", true)] public bool ignoreGravity;

        public AttackActivation activation;
        public AttackActivation deactivation;

        public AttackAction action;
    }

    [SerializeField] AnimationPhase[] _phases;
    [SerializeField] string _attackTag = "attack";
    [SerializeField] string _attackTrigger = "Attack";
    [SerializeField] string _holdBool = "Hold";
    [SerializeField] int _attackLayer = 0;
    
    private IDisposable animation = null;

    public override void Play(ICharacter character, Attack.Data[] data, Links[] linkedData)
    {
        if (animation == null)
        {
            int phaseIndex = -1;

            character.animatedState.SetAnimatorParam(_attackTrigger);
            character.animatedState.AnimationState(_attackTag, _attackLayer).Do((e) => 
            {
                if (e == AnimationStateEvents.nextPhase)
                {
                    phaseIndex++;
                    var phase = _phases[phaseIndex];
                    if (phaseIndex < _phases.Length)
                    {
                        if (phase.hold) character.animatedState.SetAnimatorParam(_holdBool, true, true);
                        if (phase.attackMovement) character.movement.StartMovement(character.asMono,
                            AttackSequenceFunctions.Default.GetDataItem<Vector3>(phase.movementFromLinks, phase.movementDataIndex, data, linkedData), Space.Self);
                        activateOrDeactivateAttack(phase, AttackActivation.onStart);
                    }
                }
                else if (e == AnimationStateEvents.animationEnded)
                {
                    if (phaseIndex >= 0 && phaseIndex < _phases.Length)
                    {
                        var phase = _phases[phaseIndex];
                        activateOrDeactivateAttack(phase, AttackActivation.onEndBeforeHold);
                    }
                }
                else if (e == AnimationStateEvents.phaseEnded)
                {
                    if (phaseIndex >= 0 && phaseIndex < _phases.Length)
                    {
                        var phase = _phases[phaseIndex];
                        activateOrDeactivateAttack(phase, AttackActivation.onEndAfterHold);
                    }
                }
            });

            character.animatedState.AnimationState(_attackTag, _attackLayer).First((e) => e == AnimationStateEvents.animationEnded).Delay(TimeSpan.FromSeconds(_phases[phaseIndex].holdDuration))
                .Do( (_) => character.animatedState.SetAnimatorParam(_holdBool, false, true) );
        }

        void activateOrDeactivateAttack(AnimationPhase phase, AttackActivation activation)
        {
            if (phase.activation == activation) AttackSequenceFunctions.Default.CommitAttack(phase.action, character, data, linkedData); 
            if (phase.deactivation == activation) AttackSequenceFunctions.Default.DeactivateProjectiles(phase.action.projInfo, data, linkedData); 
        }

    }
}
