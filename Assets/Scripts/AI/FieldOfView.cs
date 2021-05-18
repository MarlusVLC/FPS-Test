using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    
    //Fonte: Sebastian Lague
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private bool showCircumference;
        [SerializeField] private int pretendedTargets;
        [SerializeField] private float checkInterval;
        [Range(0,360)] [SerializeField] private float viewAngle;
        [SerializeField] float viewRadius; 
        [SerializeField] LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;
    
        private Collider[] _targetsInViewRadius;
        private List<Transform> visibleTargets = new List<Transform>();


        void Start()
        {
            _targetsInViewRadius = new Collider[pretendedTargets];
            StartCoroutine(FindTargetsWithDelay(checkInterval));
        }
        
        void OnValidate()
        {
            if (Application.isPlaying)
            {
                _targetsInViewRadius = new Collider[pretendedTargets];
                StartCoroutine(FindTargetsWithDelay(checkInterval));
            }
        }
        
        
        
        public IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets()
        {
            visibleTargets.Clear();

            if (Physics.OverlapSphereNonAlloc(transform.position, viewRadius, _targetsInViewRadius, targetMask) <=
                0) return;
            
            for (int i = 0; i < _targetsInViewRadius.Length; i++)
            {
                Transform target = _targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
        

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
        
        
        
        public float ViewAngle => viewAngle;
        public float ViewRadius => viewRadius;
        public List<Transform> VisibleTargets => visibleTargets;

        public bool ShowCircumference => showCircumference;
    }  
}

