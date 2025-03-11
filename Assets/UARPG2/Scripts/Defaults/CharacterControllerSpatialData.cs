using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerSpatialData : MonoBehaviour, ISpatialData
{
    [SerializeField][EditorReadOnly] bool _inAir;
    [SerializeField][EditorReadOnly] bool _onSlope;
    [SerializeField][EditorReadOnly] Vector3 _groundDir;
    [SerializeField] float _groundContactMinDistance = 0.4f;

    public bool inAir => _inAir;
    public bool onSlope => _onSlope;
    public Vector3 groundDir => _groundDir;

    private CharacterController _controller;
    private ControllerColliderHit _controllerColliderHit = new ControllerColliderHit();

    void Start() => _controller = GetComponent<CharacterController>();

    void Update()
    {
        _inAir = false;
        _onSlope = false;

        if (!Physics.Raycast(new Vector3(transform.position.x + _controller.center.x * transform.localScale.x,
                                         transform.position.y + _controller.center.y * transform.localScale.y - _controller.height * 0.5f * transform.localScale.y + 0.3f * transform.localScale.y,
                                         transform.position.z + _controller.center.z * transform.localScale.z),
                                        -transform.up, _groundContactMinDistance, Physics.DefaultRaycastLayers & ~(1 << gameObject.layer), QueryTriggerInteraction.Ignore))
        {
            if (_controller.isGrounded) _onSlope = true;
            else                        _inAir = true;
        }

        _groundDir = transform.InverseTransformDirection(Vector3.Normalize(_controllerColliderHit.point - new Vector3
                (
                    transform.position.x + _controller.center.x * transform.localScale.x,
                    transform.position.y + _controller.center.y * transform.localScale.y - _controller.height * transform.localScale.y,
                    transform.position.z + _controller.center.z * transform.localScale.z
                )));
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) => _controllerColliderHit = hit;
}
