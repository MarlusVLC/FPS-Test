using DefaultNamespace;
using UnityEngine;

namespace AI.States
{
    public class State_Gaze : State
    {
        private Vector3? _targetPosition;
        
        public override void Enter()
        {
            _entity.AlertLevel = 1;
        }
        

        public override void Execute()
        {

            if (_entity.InnerFow.VisibleTargets.Count > 0)
            {
                _stateMachine.ChangeState(_attack);
                return;
            }

            if (_entity.OuterFow.VisibleTargets.Count > 0)
            {
                _targetPosition = _entity.OuterFow.VisibleTargets[0].position;
                Transform.RotateTowards(_targetPosition.Value, _entity.StaticTurnSpeed);
            }
            else
            {
                _pursuit.TargetPosition = _targetPosition.Value;
                _targetPosition = null;
                _stateMachine.ChangeState(_pursuit);
                return;
            }
        }

        public override void Exit()
        {
        }
        
        public Vector3? TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }
    }
}