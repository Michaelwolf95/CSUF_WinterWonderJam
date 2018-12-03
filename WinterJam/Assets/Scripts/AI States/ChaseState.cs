using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.AI;

public class ChaseState : FsmStateAction
{
    public FsmOwnerDefault gameObject;
    public FsmGameObject target;

    private NavMeshAgent agent;

    public override void OnEnter()
    {
        var go = Fsm.GetOwnerDefaultTarget(gameObject);
        if (go == null)
        {
            return;
        }
        agent = go.GetComponent<NavMeshAgent>();
    }
    public override void OnUpdate()
    {
        agent.SetDestination(target.Value.transform.position);
    }

    public override void OnExit()
    {
        if(agent.enabled)
            agent.SetDestination(agent.transform.position);
    }
}
