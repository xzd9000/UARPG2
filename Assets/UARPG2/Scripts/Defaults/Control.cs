using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Control : MonoBehaviour
{
    protected IBasePlayerInput _input;
    protected ManualMovementControl _movement;

    [Inject] void Initialize(IBasePlayerInput input, ManualMovementControl movement)
    {
        _input = input;
        _movement = movement;
    }
}
