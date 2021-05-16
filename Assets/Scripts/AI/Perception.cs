using System;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace DefaultNamespace.AI
{
    public class Perception : MonoBehaviour
    {
        [SerializeField] private float outerRadius;
        [SerializeField] private float innerRadius;
        [SerializeField] private LayerMask LM_target;
        [SerializeField] private float waitTime;
        [SerializeField] private float staticTurnSpeed;
        [Space(11)] 
        [SerializeField] private Weapon gun;
        [SerializeField] private Transform head;


        private NavMeshAgent _navMeshAgent;
        
        private Transform _target;
        private Collider[] _possibleTargets;
        private Vector3 _targetPosition;
        private Vector3 _initialPosition;
        private Vector3 _gazeDirection;
        private float _guardTimer;

        private void Awake()
        {
            _initialPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _possibleTargets = new Collider[1];
            _guardTimer = 0.0f;
        }

        private void Update()
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, outerRadius, _possibleTargets, LM_target) > 0)
            {
                _targetPosition = _possibleTargets[0].transform.position;
                
                //O inimigo rotaciona em direção ao alvo caso ele esteja dentro do raio maior
                transform.RotateTowards(_targetPosition, staticTurnSpeed);
                
                if (Physics.OverlapSphereNonAlloc(transform.position, innerRadius, _possibleTargets, LM_target) > 0)
                {
                    gun.Attack(head, true);
                }
                return;
            }

            if (_possibleTargets[0] && !_navMeshAgent.hasPath)
            {
                _navMeshAgent.SetDestination(_targetPosition);
                _possibleTargets[0] = null;
                _guardTimer = 0f;
            }
            
            if (_navMeshAgent.HasReachedDestination())
            {
                FireTimer(ref _guardTimer);
            
                if (HasWaitedEnough(_guardTimer, waitTime))
                {
                    _guardTimer = 0f;
                    _navMeshAgent.SetDestination(_initialPosition);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, innerRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, outerRadius);
        }


        private bool HasWaitedEnough(float timer, float timeLimit)
        {
            return timer >= timeLimit;
        }

        private void FireTimer(ref float timer)
        {
            timer += Time.deltaTime;
        }
    }
}