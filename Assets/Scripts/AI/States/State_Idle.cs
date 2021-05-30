using System.Collections;
using UnityEngine;

namespace AI.States
{
    public class State_Idle : State
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;

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
            _entity.WaitTime = Random.Range(_entity.MINWaitingTime, _entity.MAXWaitingTime);

        }


    }
}