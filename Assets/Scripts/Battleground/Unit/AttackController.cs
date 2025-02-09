using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform AttackTarget { get; private set; }

    internal void Attack(Transform target)
    {
        AttackTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && AttackTarget == null)
        {
            AttackTarget = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && AttackTarget != null)
        {
            AttackTarget = null;
        }
    }
}
