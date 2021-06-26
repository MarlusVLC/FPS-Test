using Auxiliary;
using DefaultNamespace;
using UnityEngine;
using Weapons;

namespace AI.States
{
    public class State_Attack : State
    {
        // private Vector3? _targetPosition;
        // private int _attacksExecuted;
        private int _burstRange;
        private float _interTimer;


        public override void Enter()
        {
            _entity._navMeshAgent.ResetPath();
            _entity.AlertLevel = 2;
            
        }

        public override void Execute()
        {
            if (_entity.InnerFow.VisibleTargets.Count > 0)
            {
                _entity.TargetPosition = _entity.InnerFow.VisibleTargets[0].position;
                transform.RotateTowards(_entity.TargetPosition.Value, _entity.StaticTurnSpeed);
                _entity.AttackInBurstPattern();
            }
            if (_entity.OuterFow.VisibleTargets.Count <= 0)
            {
                // _entity.Pursuit.TargetPosition = _entity.TargetPosition.Value;
                _entity.MyStateMachine.ChangeState(_entity.Pursuit);
            }
            
        }

        public override void Exit()
        {
        }
    }
}