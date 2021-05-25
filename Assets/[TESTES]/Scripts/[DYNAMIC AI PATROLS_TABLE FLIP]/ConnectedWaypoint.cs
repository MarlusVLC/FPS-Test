using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Waypoint
{
    [SerializeField] protected float _ccnnectivityRadius = 50f;

    private List<ConnectedWaypoint> _connections;

    public void Start()
    {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        _connections = new List<ConnectedWaypoint>();

        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

            if (nextWaypoint)
            {
                if (Vector3.Distance(Transform.position, nextWaypoint.Transform.position) <= _ccnnectivityRadius && nextWaypoint !=  this)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Transform.position, _ccnnectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if (_connections.Count == 0)
        {
            Debug.LogError("Insufficient waypoint count.");
            return null;
        }
        
        if (_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }

        ConnectedWaypoint nextWaypoint;
        int nextIndex;

        do
        {
            nextIndex = Random.Range(0, _connections.Count);
            nextWaypoint = _connections[nextIndex];
        } while (nextWaypoint == previousWaypoint);

        return nextWaypoint;
    }
    
    
}
