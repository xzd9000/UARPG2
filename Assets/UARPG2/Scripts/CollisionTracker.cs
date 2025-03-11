using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionTracker : MonoBehaviour
{
    [SerializeField] List<Collider> _entered;

    public int length => _entered.Count;

    public Collider this[int i] => _entered[i];

    public Collider Find(Predicate<Collider> match) => _entered.Find(match);
    public int FindIndex(Predicate<Collider> match) => _entered.FindIndex(match);

    private void OnTriggerEnter(Collider other) => _entered.Add(other);
    private void OnTriggerExit(Collider other) => _entered.Remove(other);
}
