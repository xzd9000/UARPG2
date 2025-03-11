using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickLazer : Lazer
{
    [SerializeField] float _sphereCastRadius = 0.1f;

    protected override void Update()
    {
        if (Physics.SphereCast(
            origin: transform.position,
            direction: Direction(),
            radius: _sphereCastRadius,
            layerMask: _layers,
            maxDistance: _raycastLength,
            hitInfo: out _rayHit)) Hit(_rayHit.collider);
    }
}
