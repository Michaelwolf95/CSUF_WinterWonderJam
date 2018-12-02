using HutongGames.PlayMaker;
using MichaelWolfGames.Utility;
using UnityEngine;
using UnityEngine.AI;
using MichaelWolfGames.AI.Playmaker.Pathing;

namespace MichaelWolfGames.AI.Playmaker
{
    public class PatrolStateAction : IdleStateAction
    {
        public PatrolPath Path;
        public bool UsePath = true;

        public override void OnEnter()
        {
            base.OnEnter();
            //reachedDestinationThreshold = 0.01f;
        }

        public override void OnUpdate()
        {
            if (Path && UsePath)
            {
                FollowPatrol();
            }
            else
            {
                base.OnUpdate();
            }
        }

        private void FollowPatrol()
        {
            if (HasReachedDestination && !navMeshAgent.pathPending)
            {
                if (_waitTimer.Tick())
                {
                    //FindNewRandomDestination(_wanderRadius);
                    FindNextPathPosition();
                    _waitTimer.Reset();
                }

            }
        }

        protected void FindNextPathPosition()
        {
            Vector3 newPos = Path.GetNextNode().position;
            NavMeshHit hit;
            if (GetClosestPointOnNavMesh(newPos, out hit))
            {
                newPos = hit.position;
            }
            navMeshAgent.destination = newPos;
            navMeshAgent.SetDestination(newPos);
            
        }
    }
}