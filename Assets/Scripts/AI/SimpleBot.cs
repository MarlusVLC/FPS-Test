using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.States;
using Auxiliary;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(MyStateMachine))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(State_BotGlobalState))]
    [RequireComponent(typeof(State_Patrol))]
    [RequireComponent(typeof(State_Gaze))]
    [RequireComponent(typeof(State_Pursuit))]
    [RequireComponent(typeof(State_Attack))]
    [RequireComponent(typeof(State_Idle))]
    public class SimpleBot : MonoCache
    {
        [Header("Detection")]
        [SerializeField] private Transform head;
        [SerializeField] private FieldOfView outerFow;
        [SerializeField] private FieldOfView innerFow;
        [Space(11)]
        [Header("Idle")]
        // [SerializeField] private float waitTime;
        [SerializeField] private float staticTurnSpeed;
        [Space(11)] 
        [Header("Attack")]
        [SerializeField] private Weapon gun;
        [SerializeField] private float timeLimitBetweenBursts;
        [SerializeField] private int minBurst;
        [SerializeField] private int maxBurst;
        [Space(11)]
        [Header("Patrol")]
        // [SerializeField] private float minWaitingTime;
        // [SerializeField] private float maxWaitingTime;
        [SerializeField] private float alertRadius = 27f;
        [SerializeField] private LayerMask alarmMask;
        [SerializeField] private List<Waypoint> waypoints;
        [SerializeField] public List<Waypoint> normalRoute;
        [SerializeField] public List<Waypoint> alertRoute;



        #region External Tools
        private MyStateMachine _stateMachine;
        public NavMeshAgent _navMeshAgent { get; private set;}
        #endregion

        
        private Vector3? _targetPosition;
        private Vector3 _initialPosition;
        private Material _material;
        private short _alertLevel;
        private short _previousAlertLevel;
        private float _guardTimer;
        private int _attacksExecuted;
        private int _burstRange;
        private float _interTimer;

        private Collider[] nearbyWaypoints;


        protected override void Awake()
        {

            base.Awake();
            _stateMachine = GetComponent<MyStateMachine>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            _material = GetComponent<Renderer>().material;
            _material.color = Color.green;
            
            _targetPosition = null;
            _initialPosition = Transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _guardTimer = 0.0f;
            _attacksExecuted = 0;
            _interTimer = 0;
            _burstRange = Random.Range(minBurst, maxBurst);
            gun.AttackExecuted += AttacksIncrement;
            
            normalRoute = new List<Waypoint>(waypoints.Capacity);
            alertRoute = new List<Waypoint>(10);
            nearbyWaypoints = new Collider[5];

            _alertLevel = _previousAlertLevel = 0;
        }

        protected virtual void Start()
        {
            _stateMachine.ChangeState(GetComponent<State_Patrol>()) ;
            _stateMachine.GlobalState = GetComponent<State_BotGlobalState>();
        }

        protected void Update()
        {
            ChangeColorBasedOnAlertLevel();
        }

        #region IDLE
        public bool HasWaitedEnough(float timer, float timeLimit)
        {
            return timer >= timeLimit;
        }
        public void FireTimer(ref float timer)
        {
            timer += Time.deltaTime;
        }
        #endregion
        
        #region PURSUIT
        public void StartPositionPursuit(ref Vector3? target)
        {
            _navMeshAgent.SetDestination(target.Value);
            target = null;
            _guardTimer = 0f;
        }
        public bool HasTargetPositionInMemory(Vector3? target, List<Transform> visibleTargets)
        {
            return visibleTargets.Count <= 0  && target.HasValue;
        }
        #endregion

        #region ATTACK
        public void AttacksIncrement()
        {
            _attacksExecuted++;
        }
        public void AttackInBurstPattern()
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
                gun.Attack(head, true, true);
            }
        }
        #endregion

        #region PATROL
        public void BuildNewRoute(List<Waypoint> knownWaypoints, List<Waypoint> route)
        {
            //TODO estudar:
            //TODO Queue
            //TODO Dictionary
            //TODO HashSet
            //TODO Value types e reference types
            //TODO LINQ 
            //TODO Regras de Negocio e Regras de Enterprise 
            
            
            HashSet<int> indexes =
                MathTools.GenerateDistinctNumbersWithinRange(0, knownWaypoints.Count,  Random.Range(1, knownWaypoints.Count));
            normalRoute = knownWaypoints.Where(waypoint => indexes.Contains(knownWaypoints.IndexOf(waypoint))
                                                            && !IsEqualToFirstWaypoint(normalRoute,waypoint)).ToList();
            print(normalRoute.Count);
        }
        public bool HasARoute()
        {
            return normalRoute.Count > 0;
        }
        public bool IsEqualToFirstWaypoint(List<Waypoint> waypoints, Waypoint a)
        {
            if (waypoints.Count <= 0)
            {
                return false;
            }
            return waypoints[0] == a;
        }
        public IEnumerator TraverseRoute(List<Waypoint> route, float minWaitTime, float maxWaitTime)
        {
            
            print("RouteCount: " + route.Count );
            if (route.Count <= 0)
            {
                throw new InvalidOperationException("There must waypoints in this route");
            }
            while (route.Count > 0)
            {
                // while (currState != State.PATROL){
                //      yield return null;
                //}
                _navMeshAgent.SetDestination(route[0].Transform.position);
                yield return new WaitUntil(() =>
                    _navMeshAgent.HasReachedDestination(Transform.position, route[0].Transform.position));
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                route.RemoveAt(0);
            }
        }
        #endregion



        private bool HasAlertLevelChanged()
        {
            return _alertLevel != _previousAlertLevel;
        }

        private void SetPreviousAmmo()
        {
            _previousAlertLevel = _alertLevel;
        }

        public void ChangeColorBasedOnAlertLevel()
        {
            if (!HasAlertLevelChanged())
                return;
            
            SetPreviousAmmo();
            
            switch (_alertLevel)
            {
                case 0:
                    _material.color = Color.green;
                    break;
                case 1:
                    _material.color = Color.yellow;
                    break;
                case 2:
                    _material.color = Color.red;
                    break;
            }
        }

        public List<Waypoint> GetAllNearbyWaypoints()
        {
            List<Waypoint> nWaypoints = new List<Waypoint>(5);
            Waypoint waypoint;
            print(Physics.OverlapSphereNonAlloc(Transform.position, alertRadius, nearbyWaypoints, alarmMask));
            print(this.nearbyWaypoints[0]);
            
            foreach (var coll in this.nearbyWaypoints)
            {
                waypoint = null;
                print(" got it ");
                if (coll.TryGetComponent(out waypoint))
                {
                    nWaypoints.Add(waypoint);
                }
            }

            return nWaypoints;
        }


        public NavMeshAgent NavMeshAgent
        {
            get => _navMeshAgent;
            set => _navMeshAgent = value;
        }
        
        public MyStateMachine StateMachine
        {
            get => _stateMachine;
            set => _stateMachine = value;
        }
        
        
        public List<Waypoint> Waypoints
        {
            get => waypoints;
            set => waypoints = value;
        }

        public List<Waypoint> NormalRoute
        {
            get => normalRoute;
            set => normalRoute = value;
        }
        
        public int AttacksExecuted
        {
            get => _attacksExecuted;
            set => _attacksExecuted = value;
        }

        public short AlertLevel
        {
            get => _alertLevel;
            set => _alertLevel = value;
        }
        
        
        public List<Waypoint> AlertRoute
        {
            get => alertRoute;
            set => alertRoute = value;
        }

        
        public Material Material
        {
            get => _material;
            set => _material = value;
        }


        public FieldOfView OuterFow => outerFow;

        public FieldOfView InnerFow => innerFow;

        public float StaticTurnSpeed => staticTurnSpeed;
        
        
        public Transform Head => head;

        public Weapon Gun => gun;

        public float TimeLimitBetweenBursts => timeLimitBetweenBursts;

        public int MINBurst => minBurst;

        public int MAXBurst => maxBurst;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
        }
    }
}