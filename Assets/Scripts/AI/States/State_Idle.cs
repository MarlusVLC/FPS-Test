using System.Collections;
using UnityEngine;

namespace AI.States
{
    public class State_Idle : State
    {

        public override void Enter()
        {
            StartCoroutine(_entity.Wait(_entity.WaitTime));
        }

        public override void Execute()
        {
            if (_entity.HasWaitedEnough)
            {
                if (_entity.AlertLevel > 1)
                {
                    _entity.MyStateMachine.ChangeState(_entity.AlertPatrol);
                }
                else
                {
                    _entity.MyStateMachine.ChangeState(_entity.Patrol);
                }
            }
        }

        public override void Exit()
        {
            StopCoroutine(_entity.Wait(_entity.WaitTime));

        }


    }
}