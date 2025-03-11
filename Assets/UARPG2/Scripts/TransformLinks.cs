using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLinks : Links
{
    public enum Link
    {
        position,
        localPosition,

        rotation,
        localRotation,

        eulerRotation,
        localEulerRotation,

        scale,
        localScale
    }

    [SerializeField] Link[] _links;

    private object[] _linked;

    public override IList<object> linked => _linked;

    private void Awake() => _linked = new object[_links.Length];

    private void Update() { for (int i = 0; i < _links.Length; i++) SetLink(i); }

    private void SetLink(int index)
    {
        object value;
        if (_links[index] == Link.position) value = transform.position;
        else if (_links[index] == Link.localPosition) value = transform.localPosition;

        else if (_links[index] == Link.rotation) value = transform.rotation;
        else if (_links[index] == Link.localRotation) value = transform.localRotation;

        else if (_links[index] == Link.eulerRotation) value = transform.eulerAngles;
        else if (_links[index] == Link.localEulerRotation) value = transform.localEulerAngles;

        else if (_links[index] == Link.scale) value = transform.lossyScale;
        else if (_links[index] == Link.localScale) value = transform.localScale;

        else throw new System.ArgumentException("Invalid link");

        _linked[index] = value;
    }
}
