using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePlayerInput : IBaseInput
{
    public bool jumpPressed { get; }
    public bool jumpHeld { get; }
    public bool jumpReleased { get; }
    public bool attackPressed { get; }
    public bool attackHeld { get; }
    public bool attackReleased { get; }
    public bool interactPressed { get; }
    public bool interactHeld { get; }
    public bool interactReleased { get; }
}
