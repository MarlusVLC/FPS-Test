using System;
using UnityEngine;

namespace AI
{
    public class MyStateMachine : MonoBehaviour
    {
        
        private State _currentState;
        private State _previousState;
        private State _globalState;

        
        private void Update()
        {
            _globalState?.Execute();
            _currentState?.Execute();
        }

        public void ChangeState(State newState)
        {
            if (_currentState)
            {
                _previousState = _currentState;
                _currentState.Exit();
            }
            _currentState = newState;

            _currentState.Enter();
        }

        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }
        
        
        public State CurrentState => _currentState;
        public State PreviousState => _previousState;

        public State GlobalState
        {
            get => _globalState;
            set => _globalState = value;
        }
    }
    
    
}