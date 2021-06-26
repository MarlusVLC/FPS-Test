using System;
using Auxiliary;
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
            //Se a quantidade de coisas dentro da esfera for maior que 0
            //e essas coisas fizerem parte da Layer LM_target:
            if (Physics.OverlapSphereNonAlloc(transform.position, outerRadius, _possibleTargets, LM_target) > 0)
            {
                //Pega a posica do jogador
                _targetPosition = _possibleTargets[0].transform.position;
                
                //O inimigo rotaciona em direção ao alvo caso ele esteja dentro do raio maior
                transform.RotateTowards(_targetPosition, staticTurnSpeed);
                
                //Se essa coisa estiver dentro da esfera menor, atira
                if (Physics.OverlapSphereNonAlloc(transform.position, innerRadius, _possibleTargets, LM_target) > 0)
                {
                    gun.Attack(head, true);
                }
                
                //Se houver algo no círculo externo, o Update acaba aqui
                return;
            }
            
            
            //Checa se há algo em _possibleTargets e se a IA não tem um caminho formado.
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