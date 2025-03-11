using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public static class HitObjectFunctions
{
    public static class Default
    {
        public static void CheckCollider(HittingObject hitObj, ref IDisposable collisionSubscription, ref Collider collider)
        {
            if (collider == null)
            {
                if (!hitObj.TryGetComponent(out collider))
                {
                    Debug.LogError("Collider not found");
                    return;
                }
            }

            if (collisionSubscription == null && collider != null)
            {
                collisionSubscription = collider.isTrigger ?
                collider.OnTriggerEnterAsObservable().Subscribe(c => hitObj.Hit(c)) :
                collider.OnCollisionEnterAsObservable().Subscribe(c => hitObj.Hit(c.collider));
            }
        }
    }
    public static partial class Custom { }
}
