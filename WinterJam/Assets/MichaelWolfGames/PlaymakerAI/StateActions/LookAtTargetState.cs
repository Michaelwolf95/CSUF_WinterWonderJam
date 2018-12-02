using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using MichaelWolfGames.Utility;

namespace MichaelWolfGames.AI.Playmaker
{
	public class LookAtTargetState : StateActionNavBehaviour
	{

	    [ActionSection("Turning Variables")]
        public bool _rotateAnimatorInstead = true;
        public float _minimumLookRange = 0.3f;
        public bool _useNavMeshTurningVariables = true;
        public float _lookAtTurnSpeed = 1f;
	    public bool _disableNavMeshAgentRotation = false;

        [ActionSection("Duration Variables")]
        public bool _finishAfterDuration = false;
        public float _duration = 5f;

        protected Timer _durationTimer = new Timer(5f);

	    private float LookTurnSpeed
	    {
            get
            {
                if (_useNavMeshTurningVariables) return navMeshAgent.angularSpeed;
                return _lookAtTurnSpeed;
            }
	    }

        public override void Awake()
        {
            base.Awake();
            _durationTimer = new Timer(_duration);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _durationTimer.Restart();
            if(_disableNavMeshAgentRotation) navMeshAgent.updateRotation = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_finishAfterDuration)
            {
                if (_durationTimer.Tick())
                {
                    this.Finish();
                }
            }

            if (ProjectedToTargetVector.magnitude < _minimumLookRange) return;

            if (LookTurnSpeed > 0f)
            {
                if (_rotateAnimatorInstead)
                {
                    SmoothLookAtTarget(BehaviourController.StateAnimator.transform, LookTurnSpeed);
                }
                else
                {
                    SmoothLookAtTarget(LookTurnSpeed);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _durationTimer.Reset();
            if (_rotateAnimatorInstead)
            {
                ResetAnimatorRotation();
            }
            if (_disableNavMeshAgentRotation) navMeshAgent.updateRotation = true;
        }

	    private void ResetAnimatorRotation()
	    {
            var animTransform = BehaviourController.StateAnimator.transform;
            var animRot = animTransform.rotation;
            navMeshAgent.transform.rotation = animRot;
            animTransform.rotation = Quaternion.identity;
        }

        public void SmoothLookAtTarget(Transform rotatingTransform, float speed, float timeDelta = 0f)
        {
            if (timeDelta <= 0f) timeDelta = Time.deltaTime;
            var projectedToTargetDirection = Vector3.ProjectOnPlane(targetTransform.position, Vector3.up) -
                        Vector3.ProjectOnPlane(rotatingTransform.transform.position, Vector3.up);

            var targetRot = Quaternion.LookRotation(projectedToTargetDirection, Vector3.up);
            rotatingTransform.rotation = Quaternion.Slerp(rotatingTransform.rotation, targetRot, speed * timeDelta);
        }
    }
}
