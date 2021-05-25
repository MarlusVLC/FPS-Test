using AI.States;
using DefaultNamespace;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(MyStateMachine))]
    public abstract class State : MonoCache
    {
        protected SimpleBot _entity;
        
        protected MyStateMachine _stateMachine;
        protected State_Idle _idle;
        protected State_Attack _attack;
        protected State_Patrol _patrol;
        protected State_Pursuit _pursuit;
        protected State_Gaze _gaze;

        protected override void  Awake()
        {
            base.Awake();
            _entity = GetComponent<SimpleBot>();
            TryGetComponent(out _stateMachine);
            TryGetComponent(out _idle);
            TryGetComponent(out _attack);
            TryGetComponent(out _patrol);
            TryGetComponent(out _pursuit);
            TryGetComponent(out _gaze);

        }
        
        public abstract void Enter();

        public abstract void Execute();

        public abstract void Exit();

        // public bool onMessage(Message msg);
    }
}