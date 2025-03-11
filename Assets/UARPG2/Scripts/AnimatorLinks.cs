using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimatorLinks : MonoBehaviour
{
    protected Animator _anim;

    private void Awake() => _anim = GetComponent<Animator>();
}
