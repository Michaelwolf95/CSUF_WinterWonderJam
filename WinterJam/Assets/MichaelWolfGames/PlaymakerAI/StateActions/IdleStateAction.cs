using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using MichaelWolfGames.Utility;

namespace MichaelWolfGames.AI.Playmaker
{
	public class IdleStateAction :  StateActionNavBehaviour 
	{
        public float _wanderRadius = 1f;
	    public float _waitTime = 1f;

	    //public bool _rotateWander = false;

	    protected Timer _waitTimer = new Timer(1f);

	    public override void Init(FsmState state)
	    {
	        base.Init(state);

	    }

	    public override void Awake()
	    {
	        base.Awake();
            _waitTimer = new Timer(_waitTime);
        }

	    public override void OnEnter()
	    {
	        base.OnEnter();
            navMeshAgent.updateRotation = true;
            _waitTimer = new Timer(_waitTime);
            _waitTimer.Reset();
            PlaceOnClosestNavMeshPoint();
            if (_wanderRadius > 0f) FindNewRandomDestination(_wanderRadius);
        }
        public override void OnUpdate()
        {
            if (_wanderRadius > 0f)
            {
                WanderAround();
            }
        }

        private void WanderAround()
        {
            if (HasReachedDestination)
            {
                if (_waitTimer.Tick())
                {
                    FindNewRandomDestination(_wanderRadius);
                    _waitTimer.Reset();
                }
            }
        }
    }
}
