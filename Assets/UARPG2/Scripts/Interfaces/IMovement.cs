using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public void Move(Transform transform, Vector3 movement, Space space);
    public void MoveTo(Transform transform, Vector3 position, Vector3 movement, Space space, bool _3dMovement = false, bool _3dRotation = false);

    public void StartMovement(MonoBehaviour behaviour, Vector3 movement, Space space);
    public void EndMovement();

    public void RotateToTarget(Transform transform, Vector3 target, float rotationSpeed, float targetingAngle, bool direction = false, bool _3dTargeting = false);
}
