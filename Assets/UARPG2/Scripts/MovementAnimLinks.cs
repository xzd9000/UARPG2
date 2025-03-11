using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementLink
{
    horMagnitude,
    fullMagnitude,
    X,
    Y,
    Z,
    jump,
    slope,
    inAir
}

public class MovementAnimLinks : AnimatorLinks
{
    [System.Serializable] protected struct LinkPair
    {
        public string param;
        public MovementLink link;
    }

    [SerializeField] protected ManualMovementControl _movement;
    [SerializeField] protected LinkPair[] _links;

    private void Update()
    {
        for (int i = 0; i < _links.Length; i++)
        {
            if (_links[i].link == MovementLink.fullMagnitude) _anim.SetFloat(_links[i].param, _movement.movement.sqrMagnitude);
            else if (_links[i].link == MovementLink.horMagnitude) _anim.SetFloat(_links[i].param, new Vector3(_movement.movement.x, 0f, _movement.movement.z).sqrMagnitude);

            else if (_links[i].link == MovementLink.X) _anim.SetFloat(_links[i].param, _movement.movement.x);
            else if (_links[i].link == MovementLink.Y) _anim.SetFloat(_links[i].param, _movement.movement.y);
            else if (_links[i].link == MovementLink.Z) _anim.SetFloat(_links[i].param, _movement.movement.z);

            else if (_links[i].link == MovementLink.jump) _anim.SetBool(_links[i].param, _movement.jumpInput);

            else if (_links[i].link == MovementLink.slope) _anim.SetBool(_links[i].param, _movement.spatial.onSlope);
            else if (_links[i].link == MovementLink.inAir) _anim.SetBool(_links[i].param, _movement.spatial.inAir);
            else throw new System.NotImplementedException();
        }
    }
}
