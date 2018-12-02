using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.AI;

namespace MichaelWolfGames.AI.Playmaker
{
	public class StateActionNavBehaviour : StateActionBehaviour
	{
        [ActionSection("NavMeshAgent Variables")]
        public float AgentSpeed = 0f;
	    public float AgentAcceleration = 0f;
	    public bool UseDefaultAgentVariables = true;

        public NavMeshAgent navMeshAgent { get; protected set; }
        protected float reachedDestinationThreshold = 0.1f;
        protected Vector3 startNavPos = Vector3.zero;
        protected float DefaultAgentSpeed;
        protected float DefaultAgentAcceleration;
        //protected Timer _navigationTimeOutTimer = new Timer(2f);
	    public override void Init(FsmState state)
	    {
	        base.Init(state);
	       
	    }

	    public override void Awake()
	    {
	        base.Awake();
            InitializeNavMeshAgent();
        }

	    public override void OnEnter()
	    {
	        base.OnEnter();
	        if (!UseDefaultAgentVariables)
	        {
	            SetNavMeshAgentVariables();
	        }
	    }


	    public override void OnExit()
	    {
	        base.OnExit();
            ResetNavMeshAgentVariables();
	    }

	    protected virtual bool InitializeNavMeshAgent()
        {
            navMeshAgent = BehaviourController.CharacterTransform.GetComponent<NavMeshAgent>();
            if (!navMeshAgent)
            {
                BehaviourController.CharacterTransform.GetComponentInParent<NavMeshAgent>();
            }
            if (!navMeshAgent) return false;
            if (navMeshAgent)
            {
                DefaultAgentSpeed = navMeshAgent.speed;
                DefaultAgentAcceleration = navMeshAgent.acceleration;
            }
            InitializeStartPosition(10f);
            return true;
        }

        private void SetNavMeshAgentVariables()
        {
            navMeshAgent.speed = AgentSpeed;
            navMeshAgent.acceleration = AgentAcceleration;
        }
        protected void ResetNavMeshAgentVariables()
	    {
	        navMeshAgent.speed = DefaultAgentSpeed;
	        navMeshAgent.acceleration = DefaultAgentAcceleration;
        }

        protected virtual bool InitializeStartPosition(float sampleDistance = 5f)
        {
            NavMeshHit hit;
            bool pointFound = GetClosestPointOnNavMesh(out hit, sampleDistance);
            if (pointFound)
            {
                navMeshAgent.enabled = true;
                if (startNavPos == Vector3.zero)
                {
                    startNavPos = hit.position;
                }
                //startNavPos = hit.position;
            }
            else
                startNavPos = navMeshAgent.transform.position;
            //navMeshAgent.enabled = pointFound;
            return pointFound;
        }



        #region Common Navigation Methods and Parameters

        public Transform NavTransform
        {
            get { return navMeshAgent.transform; }
        }

        public Vector3 ToTargetVector
        {
            get { return (targetTransform.position - navMeshAgent.transform.position); }
        }

        public Vector3 ToTargetDirection
        {
            get { return ToTargetVector.normalized; }
        }

        public Vector3 ProjectedToTargetVector
        {
            get
            {
                return (Vector3.ProjectOnPlane(targetTransform.position, Vector3.up) -
                        Vector3.ProjectOnPlane(navMeshAgent.transform.position, Vector3.up));
            }
        }

        /// <summary>
        /// The ToTargetVector projected on the XZ-plane and normalized.
        /// </summary>
        public Vector3 ProjectedToTargetDirection
        {
            get { return ProjectedToTargetVector.normalized; }
        }

        public void SmoothLookAtTarget(float speed, float timeDelta = 0f)
        {
            if (timeDelta <= 0f) timeDelta = Time.deltaTime;
            var targetRot = Quaternion.LookRotation(ProjectedToTargetDirection, Vector3.up);
            //navMeshAgent.
            NavTransform.rotation = Quaternion.Slerp(NavTransform.rotation, targetRot, speed * timeDelta);
        }

        public void SmoothLookAtTarget()
        {
            var targetRot = Quaternion.LookRotation(ProjectedToTargetDirection, Vector3.up);
            NavTransform.rotation = Quaternion.Slerp(NavTransform.rotation, targetRot,
                navMeshAgent.angularSpeed * Time.deltaTime);
        }

        public Vector3 ProjectedTargetPositionAtSameHeight
        {
            get
            {
                var v = Vector3.ProjectOnPlane(targetTransform.position, Vector3.up);
                v.y = navMeshAgent.transform.position.y;
                return v;
            }
        }
        //public Vector3 ProjectedPositionAtSameHeight
        //public float GetRange(Vector3 position, bool isProjected = true)
        //{

        //}
        public bool CheckRange(float range, bool isProjected = true)
        {
            return ((isProjected) ? ProjectedToTargetVector : ToTargetVector).magnitude < range;
        }

        public bool CheckRange(float range, out bool isInRange, bool isProjected = true)
        {
            isInRange = ((isProjected) ? ProjectedToTargetVector : ToTargetVector).magnitude < range;
            return isInRange;
        }

	    protected virtual void SetDestination(Vector3 position, float offsetDistance = 0f)
	    {
            if(!navMeshAgent.enabled) return;
            NavMeshHit hit;
            if (GetClosestPointOnNavMesh(position, out hit, 100f))
            {
                var pos = hit.position - ((position - navMeshAgent.transform.position).normalized * offsetDistance);
                navMeshAgent.SetDestination(pos);
            }
            else
            {
                navMeshAgent.SetDestination(position); //ToDo: Expand this and make it more reliable.
            }
        }
        protected virtual void FollowTransform(Transform target, float offsetDistance = 0f)
        {
            SetDestination(target.position, offsetDistance);
        }

        protected virtual bool HasReachedDestination
        {
            get
            {
                return
                ((Vector3.Distance(navMeshAgent.transform.position + Vector3.down * navMeshAgent.baseOffset, navMeshAgent.destination) < reachedDestinationThreshold
                 || !navMeshAgent.hasPath) && !navMeshAgent.pathPending);
            }
        }

        protected virtual float DistanceToDestination
        {
            get
            {
                return
                (Vector3.Distance(navMeshAgent.transform.position + Vector3.down * navMeshAgent.baseOffset,
                    navMeshAgent.destination));
            }
        }
        public virtual bool GetClosestPointOnNavMesh(Vector3 worldPos, out NavMeshHit closestHit, float sampleDistance = 5f)
        {
            if (NavMesh.SamplePosition(worldPos, out closestHit, sampleDistance, 1))
            {
                return true;
            }
            return false;
        }
        public virtual bool GetClosestPointOnNavMesh(out NavMeshHit closestHit, float sampleDistance = 5f)
        {
            if (NavMesh.SamplePosition(navMeshAgent.transform.position + Vector3.down * navMeshAgent.baseOffset, out closestHit,
                sampleDistance, 1))
            {
                //navMeshAgent.enabled = true;
                //if (startNavPos == Vector3.zero)
                //{
                //    startNavPos = closestHit.position;
                //}
                return true;
            }
            return false;
        }

        protected void FindNewRandomDestination(float randomDestinationDistance)
        {
            if (!navMeshAgent.isOnNavMesh) return;
            var randVector = Random.insideUnitCircle *
                             Random.Range(randomDestinationDistance / 2f, randomDestinationDistance);
            var newPos = navMeshAgent.transform.position +
                         Vector3.down * navMeshAgent.baseOffset +
                         new Vector3(randVector.x,
                             navMeshAgent.transform.position.y - navMeshAgent.baseOffset,
                             randVector.y);
            if (Vector3.Distance(startNavPos, newPos) > randomDestinationDistance)
            {
                newPos = startNavPos;
            }
            NavMeshHit hit;
            if (GetClosestPointOnNavMesh(newPos, out hit))
            {
                newPos = hit.position;
            }
            navMeshAgent.SetDestination(newPos);
        }

        /// <summary>
        /// Finds a valid NavMesh position along a defined ray.
        /// </summary>
        protected bool NavigationSampleCast(Vector3 startPosition, Vector3 castDirection, out Vector3 result,
            float minRange, float maxRange, float castDelta)
        {
            var currentDist = minRange;
            var samplePosition = startPosition + (castDirection * currentDist);
            samplePosition = Vector3.ProjectOnPlane(samplePosition, Vector3.up);

            int breakOut = 0;
            result = Vector3.zero;
            while (!SampleNavMeshPosition(samplePosition, out result) && currentDist <= maxRange)
            {
                currentDist += castDelta;
                samplePosition = startPosition + (castDirection * currentDist);
                samplePosition = Vector3.ProjectOnPlane(samplePosition, Vector3.up);
                if (++breakOut > 10)
                    break;
            }
            if (result == Vector3.zero)
            {
                Debug.LogWarning("Could not find Retreat Path.");
                return false;
            }
            return true;
        }

        protected bool SampleNavMeshPosition(Vector3 samplePosition, out Vector3 result, float maxDistance = 50f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePosition, out hit, maxDistance, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
            result = Vector3.zero; //Maybe use current position instead?
            return false;
        }

        protected bool CheckRaycastLineOfSight(float checkRange, LayerMask layerMask)
        {
            var eyePos = navMeshAgent.transform.position +
                         Vector3.up * (((navMeshAgent) ? navMeshAgent.height : 1) * 0.75f);
            var eye2Target = targetTransform.position - eyePos;
            RaycastHit hit;
            if (Physics.Raycast(eyePos, eye2Target.normalized, out hit, checkRange, layerMask,
                QueryTriggerInteraction.Ignore))
            {
                if (
                ((hit.collider.attachedRigidbody)
                    ? hit.collider.attachedRigidbody.transform
                    : hit.collider.transform).root == targetTransform)
                {
                    //Debug.Log("Saw Target!");
                    return true;
                }
            }
            return false;
        }

        protected void PlaceOnClosestNavMeshPoint()
        {
            Vector3 v;
            if (SampleNavMeshPosition(navMeshAgent.transform.position, out v))
            {
                Debug.Log("[StateActionNavBehaviour]: Placing Agent On Closest NavMesh.");
                navMeshAgent.transform.position = v;
            }
        }

        #endregion
    }
}
