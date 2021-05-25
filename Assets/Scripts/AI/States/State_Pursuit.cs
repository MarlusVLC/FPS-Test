using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class State_Pursuit : State
    {
        private Vector3 _targetPosition;

        public override void Enter()
        {
            StartPositionPursuit(_targetPosition);
        }

        public override void Execute()
        {
            if (_entity._navMeshAgent.HasReachedDestination())
            {
                _stateMachine.ChangeState(_idle);
            }
        }

        public override void Exit()
        {
        }
        
        private void StartPositionPursuit(Vector3 target)
        {
            _entity._navMeshAgent.SetDestination(target);
        }
        
        
        public Vector3 TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }

    }
}