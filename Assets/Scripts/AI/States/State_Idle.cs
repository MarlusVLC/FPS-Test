using System.Collections;
using UnityEngine;

namespace AI.States
{
    public class State_Idle : State
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;

        private bool _hasWaitedEnough;
        public override void Enter()
        {
            StartCoroutine(Wait(minTime, maxTime));
        }

        public override void Execute()
        {
            if (_hasWaitedEnough)
            {
                _stateMachine.ChangeState(_patrol);
            }
        }

        public override void Exit()
        {

        }

        private IEnumerator Wait(float minTime, float maxTime)
        {
            _hasWaitedEnough = false;
            var timeToWait = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(timeToWait);
            _hasWaitedEnough = true;
        }
    }
}