using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using MichaelWolfGames.Utility;

namespace MichaelWolfGames.AI.Playmaker
{
	public class CombatMovementState : StateActionNavBehaviour
	{
        [ActionSection("Combat Range")]
        public float _maxCombatRange = 2f;
	    public float _minCombatRange = 0.75f;
        //public FsmEvent _attackState;
        [ActionSection("Attack Trigger")]
        public FsmEvent _attackTrigger;
        public FsmFloat _attackDelay = 1f;
	    public bool _resetDelayOnMove = false;
	    private bool _isAttacking = false;
	    private Timer _attackTimer;
        private bool _isStopped = false;
        public override void Awake()
        {
            base.Awake();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //navMeshAgent.updateRotation = false;
            //navMeshAgent.updatePosition = true;
            //FollowTransform(targetTransform);
            navMeshAgent.isStopped = false;
            _isStopped = false;
            _attackTimer = new Timer(_attackDelay.Value);
            _attackTimer.Restart();
        }

	    public override void OnExit()
	    {
	        base.OnExit();
	        navMeshAgent.destination = navMeshAgent.transform.position;
	        //navMeshAgent.SetDestination(navMeshAgent.transform.position);
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = false;
            _isStopped = false;
            _attackTimer.Reset();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            float distance = this.ProjectedToTargetVector.magnitude;
            if (distance > _maxCombatRange)
            {
                if (_isStopped)
                {
                    ResumeMovement();
                }
                float moveBackDist = _minCombatRange - distance;
                var nextPos = navMeshAgent.transform.position - ProjectedToTargetDirection * (moveBackDist + navMeshAgent.radius);
                SetDestination(nextPos);
                //FollowTransform(targetTransform);
            }
            else if (distance < _minCombatRange)
            {
                if (_isStopped)
                {
                    ResumeMovement();
                }
                float moveBackDist = _minCombatRange - distance;
                var nextPos = navMeshAgent.transform.position - ProjectedToTargetDirection * (moveBackDist + navMeshAgent.radius);
                SetDestination(nextPos);
                Debug.DrawLine(navMeshAgent.transform.position, nextPos, Color.red);
                
            }
            else
            {
                if (!_isStopped)
                {
                    StopMovement();
                }
                if (_attackTimer.Tick())
                {
                    Fsm.Event(_attackTrigger);
                    _attackTimer.Restart();
                }
            }
        }

	    private void StopMovement()
	    {
            //navMeshAgent.Stop();
            navMeshAgent.isStopped = true; //ToDo: Make sure adding this didn't cause side effects.
            _isStopped = true;
	    }

	    private void ResumeMovement()
	    {
            navMeshAgent.isStopped = false;
            _isStopped = false;
	        _attackTimer.Restart();
	    }
	}
}