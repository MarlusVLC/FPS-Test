using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Auxiliary;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI.States
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class State_AlertPatrol : State
    {

        public override void Enter()
        {
                if (!_entity.HasARoute(_entity.AlertRoute))
                {
                    _entity.BuildNewRoute(_entity.GetAllNearbyWaypoints(), ref _entity.alertRoute);
                }
        }

        public override void Execute()
        {
            _entity.TraverseRoute(_entity.AlertRoute);
        }

        public override void Exit()
        {
        }
    }
}