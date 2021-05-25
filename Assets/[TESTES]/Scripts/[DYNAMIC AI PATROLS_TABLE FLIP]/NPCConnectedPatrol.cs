using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCConnectedPatrol : MonoCache
{
    [SerializeField] bool _patrolWaiting;
    [SerializeField] private float _totalWaitTime = 3f;
    [SerializeField] private float _switchProbability = 0.2f;
    // [SerializeField] private List<Waypoint> _patrolPoints;
    // [SerializeField] private Transform waypointParent;

    //Variáveis relacionada diretamente ao comportamento base
    private NavMeshAgent _navMeshAgent;
    private ConnectedWaypoint _currentWaypoint;
    private ConnectedWaypoint _previousWaypoint;
    
    private bool _travelling;
    private bool _waiting;
    // private bool _patrolForward;
    private float _waitTimer;
    private int _waypointsVisited;

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        

        
        if (!_navMeshAgent)
        {
            Debug.LogError("There's no NavMeshAgent component attached to " + name);
        }
        else
        {
            if (!_currentWaypoint)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (_currentWaypoint == null)
                    {
                        int random = Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        if (startingWaypoint)
                        {
                            _currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to find any waypoints for use in the scene");
                }
            }
        }
        SetDestination();
    }

    public void Update()
    {
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;
            _waypointsVisited++;

            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                SetDestination();
            }
        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                SetDestination();
            }
        }
    }



    private void SetDestination()
    {
        if (_waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = _currentWaypoint.Transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _travelling = true;
    }
    
}
