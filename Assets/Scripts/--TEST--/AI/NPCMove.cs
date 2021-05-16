using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField] private Transform _destination;

    private NavMeshAgent _navMeshAgent;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        if (!_navMeshAgent)
            Debug.LogError("The nav mesh agent component is not attached to " + name);
        else
        {
            SetDestination();
        }
    }


    private void SetDestination()
    {
        if (_destination)
        {
            Vector3 targetVector = _destination.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }
}
