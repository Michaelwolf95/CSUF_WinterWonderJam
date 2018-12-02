namespace MichaelWolfGames.AI.Playmaker
{
    public class ReturnToOriginStateAction : StateActionNavBehaviour
    {
        public override void OnEnter()
        {
            base.OnEnter();
            navMeshAgent.destination = startNavPos;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HasReachedDestination) Finish();
        }
    }
}