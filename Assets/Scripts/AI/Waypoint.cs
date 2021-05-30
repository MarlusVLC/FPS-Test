using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Waypoint : MonoCache
{
    [field: SerializeField] public Color DefaultColor { get; private set; }
    [field: SerializeField] public Color NormalRouteColor { get; private set; }
    [field: SerializeField] public Color AlertRouteColor { get; private set; }
    [SerializeField] protected float debugDrawRadius = 1.0f;

    private Color _currentColor;
    void Start()
    {
        base.OnStart();
        _currentColor = DefaultColor;
    }
    
    

    protected virtual void OnDrawGizmos()
    {
        if (Application.IsPlaying(this))
        {
            Gizmos.color =_currentColor;
        }
        else
        {
            Gizmos.color = DefaultColor;
        }
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }


    public Color CurrentColor
    {
        get => _currentColor;
        set => _currentColor = value;
    }
}
