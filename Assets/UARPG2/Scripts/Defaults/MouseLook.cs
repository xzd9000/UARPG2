using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

#pragma warning disable 0649


public class MouseLook : MonoBehaviour
{
    [Flags] public enum Control
    {
        X = 0b10,
        Y = 0b01,
        XY = 0b11,
        distance = 0b100,
    }

    public Control control;

    [SerializeField] bool _lockCursorAtStart = true;
    [SerializeField] float _sens = 6;
    [SerializeField] float _vertMin = -90;
    [SerializeField] float _vertMax = 90;
    [SerializeField][HideUnless("control", typeof(Enum), true, (int)Control.distance)] float _maxDistance = -30;
    [SerializeField][HideUnless("control", typeof(Enum), true, (int)Control.distance)] float _defaultCameraDistance = -5;
    [SerializeField][HideUnless("control", typeof(Enum), true, (int)Control.distance)] float _minDistance = 0;
    [SerializeField][HideUnless("control", typeof(Enum), true, (int)Control.distance)] float _distanceChangeSpeed = 1;
    [SerializeField][HideUnless("control", typeof(Enum), true, (int)Control.distance)] bool _cameraWallCollision;
    [SerializeField][HideUnless("_cameraWallCollision", true)] float _wallDistance = 0.2f;
    [SerializeField][HideUnless("_cameraWallCollision", true)] LayerMask _includeLayers = Physics.DefaultRaycastLayers;
    [SerializeField][EditorReadOnly] float _distance;

    private bool _mouseLocked;
    private float _rawDistance;
    private RaycastHit _hit;

    private IBaseInput _input;

    public float angleY;
    public float angleX;

    public bool targetLocked { get; private set; }
    public Transform target { get; private set; }

    [Inject] void Initialize(IBaseInput input) => _input = input;

    // Use this for initialization
    void Start () 
    {
        if (_lockCursorAtStart) SetCursorLock(true);
        _rawDistance = transform.localPosition.z;
        _distance = _rawDistance;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (_mouseLocked == true)
        {                  
            if (!targetLocked)
            {
                if (control.HasFlag(Control.Y))
                {
                    float mouseY = _input.camera.y * _sens;
                    angleY -= mouseY;
                    angleY = Mathf.Clamp(angleY, _vertMin, _vertMax);
                }
                else angleY = transform.eulerAngles.x;

                if (control.HasFlag(Control.X))
                {
                    float mouseX = _input.camera.x * _sens;
                    angleX = transform.eulerAngles.y + mouseX;
                }
                else angleX = transform.eulerAngles.y;
            }
            else
            {
                Vector3 angle = (Quaternion.LookRotation(transform.DirectionToTarget(target.position, true))).eulerAngles;
                if (control.HasFlag(Control.X)) angleX = angle.y;
                else angleX = transform.eulerAngles.y;
                if (control.HasFlag(Control.Y)) angleY = angle.x;
                else angleY = transform.eulerAngles.x;
            }

            SetAngles();

            if (control.HasFlag(Control.distance))
            {
                if (_input.scroll > 0f) _rawDistance += _distanceChangeSpeed;
                else if (_input.scroll < 0f) _rawDistance -= _distanceChangeSpeed;
                if (_rawDistance > _minDistance) _rawDistance = _minDistance;
                if (_rawDistance < _maxDistance) _rawDistance = _maxDistance;                

                if (_cameraWallCollision)
                {
                    if (Physics.Raycast(transform.position, transform.DirectionToTarget(transform.position, true), out _hit, Mathf.Abs(_rawDistance), _includeLayers, QueryTriggerInteraction.Ignore))
                    {
                        _distance = -_hit.distance + _wallDistance;
                        _distance = Mathf.Clamp(_distance, _distance, _minDistance);
                    }
                    else _distance = _rawDistance;
                }
                else _distance = _rawDistance;

                transform.localPosition = new Vector3(0, 0, _distance);
            }
            
        }
    }

    public void ResetAxis(Control axis)
    {
        if (axis.HasFlag(Control.X)) angleY = 0f;
        if (axis.HasFlag(Control.Y)) angleX = 0f;
    }
    public void SetAngles() => transform.eulerAngles = new Vector3(angleY, angleX, transform.eulerAngles.z);

    public void SetCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
        _mouseLocked = locked;
    }

    public void ResetCameraDistance() => transform.localPosition = new Vector3(0, 0, _defaultCameraDistance);

    public void LockTarget(Transform target)
    {
        targetLocked = true;
        this.target = target;
    }
    public void UnlockTarget()
    {
        targetLocked = false;
        target = null;
    }
}
