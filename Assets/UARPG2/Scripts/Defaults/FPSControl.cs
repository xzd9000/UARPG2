using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FPSControl : Control
{   
    protected virtual void Update()
    {
        _movement.movementInput = new Vector3(_input.movement.x, 0f, _input.movement.y);
        _movement.jumpInput = _input.jumpPressed;
        _movement.jumpHeldInput = _input.jumpHeld;
    }
}
