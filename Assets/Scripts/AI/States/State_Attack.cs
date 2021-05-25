using DefaultNamespace;
using UnityEngine;
using Weapons;

namespace AI.States
{
    public class State_Attack : State
    {
        private Vector3? _targetPosition;
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
                _targetPosition = _entity.InnerFow.VisibleTargets[0].position;
                transform.RotateTowards(_targetPosition.Value, _entity.StaticTurnSpeed);
                AttackInBurstPattern();
            }
            if (_entity.OuterFow.VisibleTargets.Count <= 0)
            {
                _pursuit.TargetPosition = _targetPosition.Value;
                _stateMachine.ChangeState(_pursuit);
            }
            
        }

        public override void Exit()
        {
        }
        
        private void AttackInBurstPattern()
        {
            if (_entity.AttacksExecuted > _burstRange)
            {
                if (_interTimer < _entity.TimeLimitBetweenBursts)
                {
                    _interTimer += Time.deltaTime;
                }
                else
                {
                    _burstRange = Random.Range(_entity.MINBurst, _entity.MAXBurst);
                    _interTimer = 0;
                    _entity.AttacksExecuted = 0;
                }
            }
            else
            {
                _entity.Gun.Attack(_entity.Head, true);
            }
        }
        
        
        
        public Vector3? TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }

    }
}