using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCSimplePatrol : MonoCache
{
    [SerializeField] bool _patrolWaiting;
    [SerializeField] private float _totalWaitTime = 3f;
    [SerializeField] private float _switchProbability = 0.2f;
    [SerializeField] private List<Waypoint> _patrolPoints;
    [SerializeField] private Transform waypointParent;

    private NavMeshAgent _navMeshAgent;
    private int _currentPatrolIndex;
    private bool _travelling;
    private bool _waiting;
    private bool _patrolForward;
    private float _waitTimer;

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        

        
        if (!_navMeshAgent)
        {
            Debug.LogError("There's no NavMeshAgent component attached to " + name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                try
                {
                    if (waypointParent)
                    {
                        _patrolPoints = new List<Waypoint>(waypointParent.childCount);
                        foreach (Transform child in waypointParent)
                        {
                            _patrolPoints.Add(child.GetComponent<Waypoint>());
                        }
                    }
                    else
                    {
                        _patrolPoints = new List<Waypoint>();
                        _patrolPoints.AddRange(FindObjectsOfType<Waypoint>());
                    }
                    _currentPatrolIndex = 0;
                    SetDestination();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
        }
    }

    public void Update()
    {
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;

            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }



    private void SetDestination()
    {
        if (_patrolPoints != null)
        {
            _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].Transform.position);
            _travelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if (Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if (--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}
