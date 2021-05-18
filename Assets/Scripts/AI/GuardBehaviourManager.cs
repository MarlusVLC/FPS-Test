using AI;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Random = UnityEngine.Random;

namespace DefaultNamespace.AI
{
    public class GuardBehaviourManager : MonoBehaviour
    {
        [SerializeField] private FieldOfView outerFow;
        [SerializeField] private FieldOfView innerFow;
        [SerializeField] private float waitTime;
        [SerializeField] private float staticTurnSpeed;
        [Space(11)] 
        [SerializeField] private Weapon gun;
        [SerializeField] private Transform head;
        [SerializeField] private float timeLimitBetweenBursts;
        [SerializeField] private int minBurst;
        [SerializeField] private int maxBurst;
        
        


        private NavMeshAgent _navMeshAgent;
        
        //PROCURAR UM MEIO MELHOR -> EVITAR USAR NULLABLES E FAZER CASTINGS
        private Vector3? _targetPosition;
        private Vector3 _initialPosition;
        private float _guardTimer;
        private int _attacksExecuted;
        private int _burstRange;
        private float _interTimer;

        private void Awake()
        {
            _targetPosition = null;
            _initialPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _guardTimer = 0.0f;
            _attacksExecuted = 0;
            _interTimer = 0;
            gun.AttackExecuted += AttacksIncrement;
            _burstRange = Random.Range(minBurst, maxBurst);
        }
        

        private void Update()
        {

            if (outerFow.VisibleTargets.Count > 0)
            {
                
                _targetPosition = outerFow.VisibleTargets[0].transform.position;
                transform.RotateTowards((Vector3)_targetPosition, staticTurnSpeed);
                
                // Se essa coisa estiver dentro da esfera menor, atira

                if (innerFow.VisibleTargets.Count > 0)
                {
                    if (_attacksExecuted > _burstRange)
                    {
                        if (_interTimer < timeLimitBetweenBursts)
                        {
                            _interTimer += Time.deltaTime;
                        }
                        else
                        {
                            _burstRange = Random.Range(minBurst, maxBurst);
                            _interTimer = 0;
                            _attacksExecuted = 0;
                        }
                    }
                    else
                    {
                        gun.Attack(head, true);
                    }
                }
                return;
            }

            //Checa se há algo em _possibleTargets e se a IA não tem um caminho formado.
            if (outerFow.VisibleTargets.Count <= 0  && _targetPosition != null)
            {
                _navMeshAgent.SetDestination((Vector3)_targetPosition);
                _targetPosition = null;
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


        private bool HasWaitedEnough(float timer, float timeLimit)
        {
            return timer >= timeLimit;
        }

        private void FireTimer(ref float timer)
        {
            timer += Time.deltaTime;
        }

        private void AttacksIncrement()
        {
            _attacksExecuted++;
        }
    }
}