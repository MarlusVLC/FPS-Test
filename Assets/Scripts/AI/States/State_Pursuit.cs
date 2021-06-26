using Auxiliary;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class State_Pursuit : State
    {
        public override void Enter()
        {
            _entity.PaintWaypoints(_entity.alertRoute, true);
            _entity.alertRoute.Clear();
            _entity.StartPositionPursuit(_entity.TargetPosition.Value);
        }

        public override void Execute()
        {
            if (_entity._navMeshAgent.HasReachedDestination())
            {
                _entity.MyStateMachine.ChangeState(_entity.Idle);
            }
        }

        public override void Exit()
        {
        }
    }
}