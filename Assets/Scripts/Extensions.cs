using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public static class Extensions
    {
        
        public static bool HasReachedDestination(this NavMeshAgent _navMeshAgent)
        {
            return !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                    (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
        }
        
        public static void RotateTowards(this Transform transform, Vector3 targetPos, float speed)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetPos - transform.position,
                speed * Time.deltaTime, 0.0f));
        }

        public static void UncannyVector (this Vector3 vector)
        {
            vector.x = 9999;
            vector.y = 9999;
            vector.z = 9999;
        }
    }
}