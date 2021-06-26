using Auxiliary;
using DefaultNamespace;
using UnityEngine;

namespace AI.States
{
    public class State_Gaze : State
    {
        
        public override void Enter()
        {
        }
        

        public override void Execute()
        {

            if (_entity.InnerFow.VisibleTargets.Count > 0)
            {
                _entity.MyStateMachine.ChangeState(_entity.Attack);
                return;
            }

            if (_entity.OuterFow.VisibleTargets.Count > 0)
            {
                _entity.TargetPosition = _entity.OuterFow.VisibleTargets[0].position;
                Transform.RotateTowards(_entity.TargetPosition.Value, _entity.StaticTurnSpeed);
            }
            else
            {
                _entity.MyStateMachine.ChangeState(_entity.Pursuit);
            }
        }

        public override void Exit()
        {
        }
    }
}