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
            if (_entity.OuterFow.VisibleTargets.Count > 0 && _stateMachine.CurrentState != _gaze &&
                _stateMachine.CurrentState != _attack && _stateMachine.CurrentState != _pursuit)
            {
                _gaze.TargetPosition = _entity.OuterFow.VisibleTargets[0].position;
                _stateMachine.ChangeState(_gaze);
            }

            if (_entity.InnerFow.VisibleTargets.Count > 0)
            {
                _stateMachine.ChangeState(_attack);
            }
        }

        public override void Exit()
        {
        }
    }
}