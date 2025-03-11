using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ManualMovementControl : MonoBehaviour
{
    [SerializeField][EditorReadOnly] protected Vector3 _movement;
    [SerializeField] protected float _mass = 1f;
    [SerializeField] protected float _fallSpeedLimit = -10f;
    [SerializeField] protected float _groundedGravity = -2f;
    [SerializeField] protected bool _infiniteJumps;
    [SerializeField][HideUnless("slopeInput", true)] protected float _slipping = 0.1f;

    protected IMovement _moving;
    protected float _tVertSpeed;
    protected int _jumpIndex = 1;
    protected bool _jumpAllowed = true;
    protected bool _jump;

    public bool gravityInput = true;
    public bool slopeInput;
    public Vector3 movementInput;
    public float moveSpeedInput;
    public bool jumpInput;
    public bool jumpHeldInput;
    public float jumpHeightInput;
    public int jumpsAmountInput;
    public float laterJumpsModInput = 1f;
    public float mouseXInput;
    public float mouseYInput;
    public float mouseWheelInput;
    public float massModifierInput = 1f;

    public float massInput
    {
        get => _mass;
        set => _mass = value > 0f ? value : 0f;
    }

    public ISpatialData spatial { get; protected set; }

    public Vector3 movement => _movement;

    [Inject] void Initialize(ISpatialData spatial, IMovement movement)
    {
        this.spatial = spatial;
        _moving = movement;
    }

    protected virtual void Update() => DefaultMovement();

    protected void DefaultMovement() => DefaultMovement(ResetMovement, DefaultHandleInput, JumpAndSlope, Move);
    protected void DefaultMovement(Action resetMovement, Action handleInput, Action jumpAndSlope, Action<Vector3, Space> move)
    {
        resetMovement();

        handleInput();

        _movement.y = _tVertSpeed;

        jumpAndSlope();

        _movement.y += movementInput.y * Time.deltaTime;

        move(_movement, Space.Self);
    }

    protected void DefaultHandleInput()
    {       
        _movement.x = movementInput.x;
        _movement.z = movementInput.z;
        _movement *= moveSpeedInput;
        _movement = Vector3.ClampMagnitude(_movement, moveSpeedInput);

        _jump = jumpInput && _jumpAllowed;      
    }

    protected void ResetMovement()
    {
        _tVertSpeed = _movement.y;
        _movement.y = 0f;
        _movement.x = 0f;
        _movement.z = 0f;
        _jump = false;
    }

    protected void JumpAndSlope()
    {
        if (spatial.inAir)
        {
            if (_movement.y > _fallSpeedLimit && gravityInput) _movement.y += Physics.gravity.y * _mass * massModifierInput * Time.deltaTime;

            if (_jump)
            {
                if (_jumpIndex < jumpsAmountInput || _infiniteJumps)
                {
                    _movement.y += jumpHeightInput * laterJumpsModInput;
                    _movement.y = Mathf.Clamp(_movement.y, 0f, jumpHeightInput * laterJumpsModInput);
                    _jumpIndex++;
                }
            }
        }
        else
        {
            if (gravityInput && _movement.y < -_groundedGravity) _movement.y = _groundedGravity;
            _jumpIndex = 1;

            if (slopeInput && gravityInput && spatial.onSlope) _movement -= spatial.groundDir * massInput * _slipping * Mathf.Acos(Vector3.Dot(spatial.groundDir, Vector3.down)) * Mathf.Rad2Deg;
            
            if (_jump) _movement.y = jumpHeightInput;
        }
    }

    protected void Move(Vector3 movement, Space space = Space.Self) => _moving.Move(transform, movement * Time.deltaTime, space);
}