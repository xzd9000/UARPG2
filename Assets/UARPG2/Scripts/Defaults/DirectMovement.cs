using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DirectMovement : IMovement
{
    private CharacterController _controller;

    [Inject] void Initialize(CharacterController controller) => _controller = controller;

    public void Move(Transform transform, Vector3 movement, Space space)
    {
        if (space == Space.Self) movement = transform.transform.TransformDirection(movement);
        _controller.Move(movement);
    }

    public void MoveTo(Transform transform, Vector3 position, Vector3 movement, Space space, bool _3dMovement, bool _3dRotation)
    {      
        if (_3dMovement && !_3dRotation)
        {
            float magnitude = movement.magnitude;
            movement.y += transform.DirectionToTarget(position, true).y;
            movement = Vector3.ClampMagnitude(movement, magnitude);
        }
        Move(transform, movement, space);
    }

    private MonoBehaviour routineHolder;
    private IEnumerator movementRoutine;

    public void StartMovement(MonoBehaviour behaviour, Vector3 movement, Space space)
    {
        routineHolder = behaviour;
        movementRoutine = IEMove(behaviour.transform, movement, space);
        behaviour.StartCoroutine(movementRoutine);
    }
    public void EndMovement()
    {
        routineHolder.StopCoroutine(movementRoutine);
        routineHolder = null;
        movementRoutine = null;
    }
    private IEnumerator IEMove(Transform transform, Vector3 movement, Space space)
    {
        Move(transform, movement, space);
        yield return new WaitForEndOfFrame();
    }

    public void RotateToTarget(Transform transform, Vector3 target, float rotationSpeed, float targetingAngle, bool direction = false, bool _3dTargeting = false)
    {
        if (transform.AngleToTarget(target, _3dTargeting, direction) > targetingAngle) transform.RotateToTarget(target, rotationSpeed, direction, !_3dTargeting);
    } 
}
