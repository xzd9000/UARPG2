using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum AnimationStateEvents
{
    cantBeStarted = -1,
    waiting = 0,
    started = 1,
    nextPhase = 2,
    animationEnded = 3,
    phaseEnded = 4,
    finished
}

public class AnimationStateControl
{
    private Animator _anim;

    public virtual bool attackState { get; private set; }
    public virtual bool movementState 
    {
        get => _anim.GetCurrentAnimatorStateInfo(moveLayer).IsTag(moveTag);
        private set { } 
    }
    public bool waiting { get; private set; }

    public virtual bool readyForAttack { get => true; private set { } }

    public string attackTag { get; }
    public string moveTag { get; }
    public string attackTrigger { get; }
    public int attackLayer;
    public int moveLayer;

    public AnimationStateControl(Animator anim) => _anim = anim;

    public virtual void SetAnimatorParam(string param, bool value = true, bool boolParam = false)
    {
        if (boolParam) _anim.SetBool(param, value);
        else
        {
            if (value == true) _anim.SetTrigger(param);
            else               _anim.ResetTrigger(param);
        }
    }

    public virtual IObservable<AnimationStateEvents> AnimationState(string tag, int layer)
    {
        return Observable.FromCoroutine<AnimationStateEvents>((observer) => ObserveAnimation(observer, tag, layer));
    }
    public virtual IEnumerator ObserveAnimation(IObserver<AnimationStateEvents> observer, string tag, int layer)
    {
        yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(layer).IsTag(tag));
        observer.OnNext(AnimationStateEvents.started);
        observer.OnNext(AnimationStateEvents.nextPhase);
        while (true)
        {
            var stateInfo = _anim.GetCurrentAnimatorStateInfo(layer);
            var currentState = stateInfo.shortNameHash;

            yield return new WaitForSeconds(stateInfo.length);
            observer.OnNext(AnimationStateEvents.animationEnded);

            yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(layer).shortNameHash != currentState);
            observer.OnNext(AnimationStateEvents.phaseEnded);

            if (_anim.GetCurrentAnimatorStateInfo(layer).IsTag("")) observer.OnNext(AnimationStateEvents.nextPhase);
            else break;
        }
        observer.OnNext(AnimationStateEvents.finished);
        observer.OnCompleted();
    }
}
