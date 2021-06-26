using AI.States;
using DefaultNamespace;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(MyStateMachine))]
    public abstract class State : MonoCache
    {
        protected SimpleBot _entity;
        
        
        protected override void  Awake()
        {
            base.Awake();
            _entity = GetComponent<SimpleBot>();
        }

        public abstract void Enter();

        public abstract void Execute();

        public abstract void Exit();

        // public bool onMessage(Message msg);
    }
}