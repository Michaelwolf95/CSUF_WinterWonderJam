using UnityEngine;
using UnityEngine.AI;

namespace MichaelWolfGames.AI.Playmaker.Pathing
{
    public class PatrolPath : MonoBehaviour
    {
        // ToDo: Create a PathNode struct that hosts unique data, such as wait time at each point.
        public Transform[] PathNodes;
        public int CurrentNodeIndex = 0;
        public bool IsCircular = false;
        private int direction = 1;

        public Transform GetNextNode()
        {
            if (CurrentNodeIndex + direction > (PathNodes.Length-1)) // Direction is positive, reached end.
            {
                if (IsCircular)
                {
                    CurrentNodeIndex = 0;
                }
                else
                {
                    direction = -1;
                    CurrentNodeIndex = (PathNodes.Length - 2);
                }
            }
            else if (CurrentNodeIndex + direction < 0) //Direction is negative, reached start.
            {
                if (IsCircular)
                {
                    CurrentNodeIndex = (PathNodes.Length - 1);
                }
                else
                {
                    direction = 1;
                    CurrentNodeIndex = 1;
                }
            }
            else
            {
                CurrentNodeIndex += direction;
            }
            return PathNodes[CurrentNodeIndex];
        }

        private void OnDrawGizmosSelected()
        {
            if (PathNodes.Length > 1)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < PathNodes.Length; i++)
                {
                    Gizmos.DrawSphere(PathNodes[i].position, 0.1f);

                    if (i != PathNodes.Length - 1)
                    {
                        Gizmos.DrawLine(PathNodes[i].position, PathNodes[i+1].position);
                    }
                    else if (IsCircular)
                    {
                        Gizmos.DrawLine(PathNodes[i].position, PathNodes[0].position);
                    }
                }
            }
        }

        //public NavMeshPath GetNavMeshPath()
        //{
        //    NavMeshPath p = new NavMeshPath();
        //    Vector3[] corners = new Vector3[PathNodes.Length];
        //    for (int i = 0; i < PathNodes.Length - 1; i++)
        //    {
        //        corners[i] = PathNodes[i].position;
        //    }
        //    p.corners = corners;
        //}
    }
}