using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.States;
using Auxiliary;
using DefaultNamespace;
using Entities;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(MyStateMachine))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(State_BotGlobalState))]
    [RequireComponent(typeof(State_Patrol))]
    [RequireComponent(typeof(State_Gaze))]
    [RequireComponent(typeof(State_Pursuit))]
    [RequireComponent(typeof(State_Attack))]
    [RequireComponent(typeof(State_Idle))]
    public class SimpleBot : SensorialBeing
    {
        [Header("General")] 
        [SerializeField] private float normalSpeed;
        [SerializeField] private float alertSpeed;
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
        [SerializeField] private float minWaitingTime;
        [SerializeField] private float maxWaitingTime;
        [SerializeField] private List<Waypoint> waypoints;
        [SerializeField] public List<Waypoint> normalRoute;
        [SerializeField] public List<Waypoint> alertRoute;
        
        [Space(11)]
        [Header("Alertness")]
        [SerializeField]private TextMesh alertText;
        [SerializeField] private float alertRadius = 27f;
        [SerializeField] private LayerMask alarmMask;
        [SerializeField] private float alarmingSound = 1.2f;

        [Space(11)] [Header("Audio")]
        [SerializeField] private AudioClip alert0SFX;
        [SerializeField] private AudioClip alert1SFX;
        [SerializeField] private AudioClip alert2SFX;
        
        
        

        #region External Tools
        public MyStateMachine MyStateMachine { get; private set;}
        public NavMeshAgent _navMeshAgent { get; private set;}
        private AudioSource _audioSource;
        #endregion
        
        #region States
        private State_Idle _idle;
        private State_Attack _attack;
        private State_Patrol _patrol;
        private State_AlertPatrol _alertPatrol;
        private State_Pursuit _pursuit;
        private State_Gaze _gaze;
        #endregion


        
        private Vector3? _targetPosition;
        private Vector3 _initialPosition;
        private Material _material;
        private Int16 _alertLevel;
        private Int16 _previousAlertLevel;
        private float _waitTime;
        private bool _hasWaitedEnough;
        private int _attacksExecuted;
        private int _burstRange;
        private float _interTimer;


        private Collider[] nearbyWaypoints;


        protected override void Awake()
        {

            base.Awake();
            MyStateMachine = GetComponent<MyStateMachine>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _navMeshAgent.speed = normalSpeed;
            
            _material = GetComponent<Renderer>().material;
            _material.color = Color.green;
            
            _targetPosition = null;
            _initialPosition = Transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _waitTime = Random.Range(minWaitingTime, maxWaitingTime);
            _attacksExecuted = 0;
            _interTimer = 0;
            _burstRange = Random.Range(minBurst, maxBurst);
            gun.AttackExecuted += AttacksIncrement;
            
            normalRoute = new List<Waypoint>(waypoints.Capacity);
            alertRoute = new List<Waypoint>(10);
            nearbyWaypoints = new Collider[10];

            soundBlockingObstacles = new RaycastHit[10];
            
            _alertLevel = _previousAlertLevel = 0;

            _audioSource = GetComponent<AudioSource>();

            #region States_Initialization
            TryGetComponent(out _idle);
            TryGetComponent(out _attack);
            TryGetComponent(out _patrol);
            TryGetComponent(out _alertPatrol);
            TryGetComponent(out _pursuit);
            TryGetComponent(out _gaze);
            #endregion

        }

        protected virtual void Start()
        {
            MyStateMachine.ChangeState(GetComponent<State_Patrol>()) ;
            MyStateMachine.GlobalState = GetComponent<State_BotGlobalState>();
        }

        protected void Update()
        {
            OnAlertLevelChanged();
        }

        #region IDLE
        
        public float MINWaitingTime => minWaitingTime;

        public float MAXWaitingTime => maxWaitingTime;

        public bool HasWaitedEnough => _hasWaitedEnough;
        
        public float WaitTime
        {
            get => _waitTime;
            set => _waitTime = value;
        }
        public IEnumerator Wait(float duration)
        {
            _hasWaitedEnough = false;
            yield return new WaitForSeconds(duration);
            _hasWaitedEnough = true;
        }
        #endregion
        
        #region PURSUIT
        public void StartPositionPursuit(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);
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
        public void BuildNewRoute(List<Waypoint> knownWaypoints, ref List<Waypoint> route)
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
            route = knownWaypoints.Where(waypoint => indexes.Contains(knownWaypoints.IndexOf(waypoint))
                                                     && !IsEqualToFirstWaypoint(NormalRoute,waypoint)).ToList();
            
            PaintWaypoints(route);
        }


        public void PaintWaypoints(List<Waypoint> route)
        {
            if (route == normalRoute)
            {
                foreach (var waypoint in route)
                {
                    waypoint.CurrentColor = waypoint.NormalRouteColor;
                }

                return;
            }
            
            if (route == alertRoute)
            {
                foreach (var waypoint in route)
                {
                    waypoint.CurrentColor = waypoint.AlertRouteColor;
                }
            }

        }
        
        public bool HasARoute(List<Waypoint> route)
        {
            return route.Count > 0;
        }
        
        public bool IsEqualToFirstWaypoint(List<Waypoint> waypoints, Waypoint a)
        {
            if (waypoints.Count <= 0)
            {
                return false;
            }
            return waypoints[0] == a;
        }
        
        public void TraverseRoute(List<Waypoint> route)
        {
            if (route.Count <= 0)
            {
                throw new InvalidOperationException("There must waypoints in this route");
            }
        
            if (_navMeshAgent.destination != route[0].Transform.position)
            {
                _navMeshAgent.SetDestination(route[0].Transform.position);
            }

        
            if (_navMeshAgent.HasReachedDestination(Transform.position, route[0].Transform.position))
            {
                
                if (route.Count == 1)
                {
                    _alertLevel--;
                    _alertLevel.KeepNatural();
                }

                route[0].CurrentColor = route[0].DefaultColor;
                route.RemoveAt(0);
                
                
                MyStateMachine.ChangeState(_idle);
            }
        }
        

        #endregion



        private bool HasAlertLevelChanged()
        {
            return _alertLevel != _previousAlertLevel;
        }

        private void SetPreviousAlertLevel()
        {
            _previousAlertLevel = _alertLevel;
        }

        private void OnAlertLevelChanged()
        {
            if (!HasAlertLevelChanged())
                return;
            
            SetPreviousAlertLevel();
            
            switch (_alertLevel)
            {
                case 0:
                    _material.color = Color.green;
                    _audioSource.PlayOneShot(alert0SFX);
                    _navMeshAgent.speed = normalSpeed;
                    break;
                case 1:
                    _material.color = Color.yellow;
                    _audioSource.PlayOneShot(alert1SFX);
                    StartCoroutine(alertText.tempModText(new Color(1f, 0.4f, 0f), "!", 1));
                    break;
                case 2:
                    _material.color = Color.red;
                    _audioSource.PlayOneShot(alert2SFX);
                    _navMeshAgent.speed = alertSpeed;
                    StartCoroutine(alertText.tempModText(Color.red, "!!!", 2));
                    break;
            }
        }



        // public List<Waypoint> GetAllNearbyWaypoints()
        // {
        //     if (Physics.OverlapSphereNonAlloc(Transform.position, alertRadius, nearbyWaypoints, alarmMask) <= 0)
        //     {
        //         print("couldn't get any waypoints");
        //         return null;
        //     }
        //     
        //     List<Waypoint> nWaypoints = new List<Waypoint>(5);
        //     Waypoint waypoint;
        //
        //     foreach (var coll in nearbyWaypoints)
        //     {
        //         waypoint = null;
        //         if (coll.TryGetComponent(out waypoint))
        //         {
        //             nWaypoints.Add(waypoint);
        //         }
        //     }
        //
        //     return nWaypoints;
        // }
        
        public List<Waypoint> GetAllNearbyWaypoints()
        {
            nearbyWaypoints = Physics.OverlapSphere(Transform.position, alertRadius, alarmMask);
            if (nearbyWaypoints.Length <= 0)
            {
                print("couldn't get any waypoints");
                return null;
            }
            
            List<Waypoint> nWaypoints = new List<Waypoint>(nearbyWaypoints.Length);
            Waypoint waypoint;

            
            foreach (var coll in nearbyWaypoints)
            {
                if (coll.TryGetComponent(out waypoint))
                {
                    nWaypoints.Add(waypoint);
                }
            }

            // var limit = nWaypoints.Count < 3 ? nWaypoints.Count : 3;
            //
            // for (var i = 0; i < limit; i++)
            // {
            //     if (nWaypoints[i].TryGetComponent(out waypoint))
            //     {
            //         nWaypoints.Add(waypoint);
            //     }
            // }

            return nWaypoints;
        }

        public override float ProcessSound(float intensity, Vector3 sourcePosition)
        {
            Vector3 posDiff = Transform.position - sourcePosition;
            float sqrdDistance = Vector3.SqrMagnitude(posDiff);
            float volume = intensity / sqrdDistance;
            Vector3 sourceDirection = (posDiff).normalized;
            int obstacles = Physics.RaycastNonAlloc(Transform.position, sourceDirection, soundBlockingObstacles, posDiff.magnitude, soundObstacleMask);

            volume -= obstacles * (0.4f * volume);
            volume = Mathf.Clamp(volume, 0, float.MaxValue);

            if (volume >= alarmingSound)
            {
                _alertLevel = 2;
                _waitTime = 0.1f;
            }
            else if (_alertLevel == 0)
            {
                _alertLevel = 1;
                _waitTime = 2f;
            }

            _targetPosition = sourcePosition;

            if (StateMachine.CurrentState != _attack || StateMachine.CurrentState != _pursuit)
            {
                StateMachine.ChangeState(_pursuit);
            }
            
            return volume;
            
        }


        #region MISC_PROPERTIES

        public NavMeshAgent NavMeshAgent
        {
            get => _navMeshAgent;
            set => _navMeshAgent = value;
        }
        
        public MyStateMachine StateMachine
        {
            get => MyStateMachine;
            set => MyStateMachine = value;
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
        

        #region States_Properties
        public State_Idle Idle => _idle;

        public State_Attack Attack => _attack;

        public State_Patrol Patrol => _patrol;

        public State_AlertPatrol AlertPatrol => _alertPatrol;

        public State_Pursuit Pursuit => _pursuit;

        public State_Gaze Gaze => _gaze;
        #endregion
        
        


        
        
        public Vector3? TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }
        
        #endregion



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
        }
        
        
        
    }
}