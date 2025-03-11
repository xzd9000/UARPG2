using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSControl : Control
{
    [SerializeField] GameObject direction;

    private void Update()
    {
        if (_input.movement.x != 0f || _input.movement.y != 0f)
        {
            Vector3 rotation = direction.transform.eulerAngles;
            Vector3 dir = new Vector3(transform.eulerAngles.x, direction.transform.eulerAngles.y + Mathf.Atan2(_input.movement.x, _input.movement.y) * Mathf.Rad2Deg, transform.eulerAngles.z);
            transform.eulerAngles = dir;
            direction.transform.eulerAngles = rotation;
        }
        _movement.movementInput.z = Mathf.Clamp01(Mathf.Abs(_input.movement.x) + Mathf.Abs(_input.movement.y));

        _movement.jumpInput = _input.jumpPressed;
    }
}
