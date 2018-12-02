using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

namespace MichaelWolfGames.AI.Playmaker
{
    /// <summary>
    /// Base class for AI bahaviours inside of Playmaker.
    /// </summary>
    public class StateActionBehaviour : FsmStateAction
	{
        public Transform transform { get { return  Owner.transform; } }
        public GameObject gameObject { get { return this.Owner; } }
        public Transform targetTransform { get { return BehaviourController.MoveTargetTransform; } }
	    //public FsmObject targetGameObject;
	    protected PlaymakerBehaviourController BehaviourController;

        public override void Init(FsmState state)
	    {
	        base.Init(state);
            BehaviourController = gameObject.GetComponent<PlaymakerBehaviourController>();
	    }

	    public override void OnEnter()
	    {
	        base.OnEnter();
            
	    }

	    public override void OnUpdate()
	    {
	        base.OnUpdate();
            if (BehaviourController.SuspendStateActions) return;
        }

	    public override void OnExit()
	    {
	        base.OnExit();
	    }

	}
}
