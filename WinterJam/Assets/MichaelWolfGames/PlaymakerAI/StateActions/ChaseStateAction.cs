using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using MichaelWolfGames.Utility;

namespace MichaelWolfGames.AI.Playmaker
{
	public class ChaseStateAction : StateActionNavBehaviour 
	{
        [ActionSection("Attack Conditions")]
        public FsmEvent _attackState;
        public float _attackRange = 20f;
        [ActionSection("Time Out Conditions")]
        public FsmEvent _timeOutState;
        public float _timeOutTime = 5f;
        private Timer _timeOutTimer = new Timer(5f);
        [ActionSection("Lose Aggro Conditions")]
        public FsmEvent _idleState;
        public float _loseAggroRange = 100f;
        [ActionSection("Rush Timer Conditions")]
        public FsmEvent _rushState;
        public float _rushTime = 2f;
        private Timer _rushTimer = new Timer(2f);

        private bool _isPaused = false;
        private bool _ignoringLoseAggroRange = false;
	    private bool _isStopped = false;
        public override void Awake()
        {
            base.Awake();
            _rushTimer = new Timer(_rushTime);
            _timeOutTimer = new Timer(_timeOutTime);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            navMeshAgent.updateRotation = true;
            _rushTimer.Restart();
            if (ToTargetVector.magnitude > _loseAggroRange)
            {
                _ignoringLoseAggroRange = true;
            }
            FollowTransform(targetTransform);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_isPaused) return;
            if (CheckRange(_attackRange))
            {
                navMeshAgent.isStopped = true;//Stop();
                _isStopped = true;
                this.Fsm.Event(_attackState);
            }
            else if(_isStopped)
            {
                _isStopped = false;
                navMeshAgent.isStopped = false;//Resume();
            }
            else if (!CheckRange(_loseAggroRange) && !_ignoringLoseAggroRange)
            {
                if (_ignoringLoseAggroRange)
                {
                    this.Fsm.Event(_idleState);
                }
            }
            else
            {
                if (_ignoringLoseAggroRange) _ignoringLoseAggroRange = false;
                FollowTransform(targetTransform);
            }
            if (_timeOutTimer.Tick())
            {
                this.Fsm.Event(_timeOutState);
            }
            else if (_rushTimer.Tick())
            {
                this.Fsm.Event(_rushState);
            }
        }

    }
}
