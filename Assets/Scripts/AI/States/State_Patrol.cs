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
            if (_entity.AlertLevel > 0)
            {
                if (!HasARoute(_entity.AlertRoute))
                {
                    print("       AAAAAAAAAAAAAAAAAAAAAA   ");
                    BuildNewRoute(_entity.GetAllNearbyWaypoints(), ref _entity.alertRoute);
                    return;
                }
            }
            
            if (!HasARoute(_entity.NormalRoute))
            {
                BuildNewRoute(_entity.Waypoints, ref _entity.normalRoute);
            }
        }

        public override void Execute()
        {
            if (_entity.AlertLevel > 0)
            {
                TraverseRoute(_entity.AlertRoute);
                return;
            }
            
            TraverseRoute(_entity.NormalRoute);
        }

        public override void Exit()
        {
            if (_entity.AlertLevel > 0)
            {
                if (!HasARoute(_entity.AlertRoute))
                {
                    _entity.AlertLevel = 0;
                }
            }
        }
        

        private void BuildNewRoute(List<Waypoint> knownWaypoints, ref List<Waypoint> route)
        {

            //TODO estudar:
            //TODO Queue
            //TODO Dictionary
            //TODO HashSet
            //TODO Value types e reference types
            //TODO LINQ 
            //TODO Regras de Negocio e Regras de Enterprise 
            
            
            HashSet<int> indexes =
                MathTools.GenerateDistinctNumbersWithinRange(0, knownWaypoints.Count,  Random.Range(1, knownWaypoints.Count));
            route = knownWaypoints.Where(waypoint => indexes.Contains(knownWaypoints.IndexOf(waypoint))
            && !IsEqualToFirstWaypoint(_entity.NormalRoute,waypoint)).ToList();
        }
        
        
        
        
        

        private bool HasARoute(List<Waypoint> route)
        {
            return route.Count > 0;
        }
        
        private bool IsEqualToFirstWaypoint(List<Waypoint> waypoints, Waypoint a)
        {
            if (waypoints.Count <= 0)
            {
                return false;
            }
            return waypoints[0] == a;
        }

        private void TraverseRoute(List<Waypoint> route)
        {
            if (route.Count <= 0)
            {
                throw new InvalidOperationException("There must waypoints in this route");
            }

            if (_entity._navMeshAgent.destination != route[0].Transform.position)
            {
                _entity._navMeshAgent.SetDestination(route[0].Transform.position);
            }

            if (_entity._navMeshAgent.HasReachedDestination(Transform.position, route[0].Transform.position))
            {
                route.RemoveAt(0);
                _stateMachine.ChangeState(_idle);
            }
        }
        
        // private IEnumerator TraverseRoute(List<Waypoint> route, float minWaitTime, float maxWaitTime)
        // {
        //     
        //     print("RouteCount: " + route.Count );
        //     if (route.Count <= 0)
        //     {
        //         throw new InvalidOperationException("There must waypoints in this route");
        //     }
        //     while (route.Count > 0)
        //     {
        //         _entity._navMeshAgent.SetDestination(route[0].Transform.position);
        //         yield return new WaitUntil(() =>
        //             _entity._navMeshAgent.HasReachedDestination(Transform.position, route[0].Transform.position));
        //         yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        //         route.RemoveAt(0);
        //     }
        // }
        
    }
}