using UnityEngine;

namespace AI.States
{
    public class State_BotGlobalState : State
    {
        
        public override void Enter()
        {
        }

        public override void Execute()
        {
            if (_entity.OuterFow.VisibleTargets.Count > 0 && _entity.MyStateMachine.CurrentState != _entity.Gaze &&
                _entity.MyStateMachine.CurrentState != _entity.Attack && _entity.MyStateMachine.CurrentState != _entity.Pursuit)
            {
                _entity.TargetPosition = _entity.OuterFow.VisibleTargets[0].position;
                _entity.MyStateMachine.ChangeState(_entity.Gaze);
            }

            if (_entity.InnerFow.VisibleTargets.Count > 0 && _entity.MyStateMachine.CurrentState != _entity.Attack)
            {
                _entity.MyStateMachine.ChangeState(_entity.Attack);
            }
        }

        public override void Exit()
        {
        }
    }
}