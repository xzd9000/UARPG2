using UnityEngine;

public interface IBaseInput
{
    public Vector2 movement { get; }
    public Vector2 camera   { get; }
    public float scroll     { get; }
}
