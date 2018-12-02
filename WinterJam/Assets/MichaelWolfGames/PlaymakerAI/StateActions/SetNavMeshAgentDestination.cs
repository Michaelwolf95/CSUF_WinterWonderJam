using HutongGames.PlayMaker;

namespace MichaelWolfGames.AI.Playmaker
{
    public class SetNavMeshAgentDestination : StateActionNavBehaviour
    {
        public FsmVector3 Destination;
        public bool SetCurrentPosition;
        public override void OnEnter()
        {
            base.OnEnter();
            if (SetCurrentPosition)
            {
                navMeshAgent.SetDestination(navMeshAgent.transform.position);
            }
            else
            {
                navMeshAgent.SetDestination(Destination.Value);
            }
        }
    }
}