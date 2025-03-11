using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : InstantiatableHitObjectBase
{
    [SerializeField] protected Vector3 _raycastDir = Vector3.forward;
    [SerializeField] protected Space _dirSpace = Space.Self;
    [SerializeField] protected float _raycastLength = 100f;
    [SerializeField] protected LayerMask _layers = Physics.DefaultRaycastLayers;

    protected RaycastHit _rayHit;

    protected virtual void Update()
    {
        if (Physics.Raycast(
            origin: transform.position,
            direction: Direction(),
            maxDistance: _raycastLength,
            layerMask: _layers,
            hitInfo: out _rayHit)) Hit(_rayHit.collider);
    }

    protected Vector3 Direction() => _dirSpace == Space.Self ? transform.TransformDirection(_raycastDir) : _raycastDir;
}
