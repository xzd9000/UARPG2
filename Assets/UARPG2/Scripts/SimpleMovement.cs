using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] Vector3 _initMovement;
    [SerializeField] Vector3 _initScale;
    [SerializeField] Vector3 _initRotation;

    [SerializeField] Vector3 _moveAcceleration;
    [SerializeField] Vector3 _scaleAcceleration;
    [SerializeField] Vector3 _rotationAcceleration;

    [SerializeField] Space _space;

    private Vector3 _movement;
    private Vector3 _scale;
    private Vector3 _rotation;

    private void Awake()
    {
        _movement = _initMovement;
        _scale = _initScale;
        _rotation = _initRotation;
    }

    private void Update()
    {
        transform.Translate(_movement * Time.deltaTime, _space);
        transform.localScale += _scale * Time.deltaTime;
        transform.Rotate(_rotation * Time.deltaTime, _space);
        

        _movement += _moveAcceleration * Time.deltaTime;
        _scale += _scaleAcceleration * Time.deltaTime;
        _rotation += _rotationAcceleration * Time.deltaTime;
    }
}