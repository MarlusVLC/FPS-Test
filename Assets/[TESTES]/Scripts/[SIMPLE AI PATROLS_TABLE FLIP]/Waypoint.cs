using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Waypoint : MonoCache
{
    [SerializeField] protected float debugDrawRadius = 1.0f;
    void Start()
    {
        base.OnStart();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
