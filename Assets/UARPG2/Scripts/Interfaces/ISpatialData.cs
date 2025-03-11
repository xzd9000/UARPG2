using UnityEngine;

public interface ISpatialData
{
    public bool inAir { get; }
    public bool onSlope { get; }

    public Vector3 groundDir { get; }
}
