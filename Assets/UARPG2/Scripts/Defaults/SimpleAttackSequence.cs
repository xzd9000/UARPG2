using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimpleAttackSequence : AttackSequence
{
    [SerializeField] bool _animated;
    [SerializeField][HideUnless("animated", true)] string _attackTag = "attack";
    [SerializeField][HideUnless("animated", true)] string _attackTrigger = "Attack";
    [SerializeField][HideUnless("animated", true)] int _attackLayer = 0;

    [SerializeField] AttackAction _activation;

    private IDisposable animation = null;

    public override void Play(ICharacter character, Attack.Data[] data, Links[] linkedData)
    {
        if (_animated)
        {
            if (animation == null)
            {
                character.animatedState.SetAnimatorParam(_attackTrigger);
                animation = character.animatedState.AnimationState(_attackTag, _attackLayer)
                    .First((e) => e == AnimationStateEvents.finished)
                    .Subscribe(_ =>
                    {
                        AttackSequenceFunctions.Default.CommitAttack(_activation, character, data, linkedData);
                        animation.Dispose();
                        animation = null;
                    });
            }
        }
        else AttackSequenceFunctions.Default.CommitAttack(_activation, character, data, linkedData);
    }
    public override void Abort(ICharacter character)
    {
        character.animatedState.SetAnimatorParam(_attackTrigger, false, false);
        if (animation != null)
        {
            animation.Dispose();
            animation = null;
        }
    }  
}