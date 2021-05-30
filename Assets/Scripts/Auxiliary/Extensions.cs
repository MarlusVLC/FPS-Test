using System.Collections;
using System.Collections.Generic;
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
        
        public static bool HasReachedDestination(this NavMeshAgent _navMeshAgent, Vector3 position, Vector3 destination)
        {
            return !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                   (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f) &&
                   // (Vector3.Distance(position,destination) < 2f);
                   Vector3.SqrMagnitude(destination - position) < 2f * 2f;
        }
        
        public static void RotateTowards(this Transform transform, Vector3 targetPos, float speed)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetPos - transform.position,
                speed * Time.deltaTime, 0.0f));
        }
        
        
        public static IEnumerator tempModText(this TextMesh textMesh, Color color, string text, float timer)
        {
            var defaultColor = textMesh.color;
            var defaultText = textMesh.text;

            textMesh.color = color;
            textMesh.text = text;

            yield return new WaitForSeconds(timer);

            textMesh.color = defaultColor;
            textMesh.text = defaultText;
        }

        public static short KeepNatural(this short num)
        {
            if (num < 0)
            {
                num = 0;
            }

            return num;
        }
    }
}