using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Auxiliary;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class State_Patrol : State
    {
        public override void Enter()
        {
            if (!_entity.HasARoute(_entity.NormalRoute))
            {
                if (_entity.AlertLevel > 0)
                {
                    _entity.AlertLevel = 0;
                }
                _entity.WaitTime = 0f;
                _entity.BuildNewRoute(_entity.Waypoints, ref _entity.normalRoute);
            }
        }

        public override void Execute()
        {
            _entity.TraverseRoute(_entity.NormalRoute);
        }

        public override void Exit()
        {
        }
    }
}