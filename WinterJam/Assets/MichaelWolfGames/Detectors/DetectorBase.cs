using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.DetectorSystem
{
    /// <summary>
    /// Base class for detectors.
    /// Currently, detectors need to have a target passed into them.
    /// ToDo: Refactor these to be more like a trigger event.
    /// 
    /// </summary>
	public abstract class DetectorBase : MonoBehaviour, IDetector
	{
        [SerializeField] protected string _targetTag = "Player";
        [SerializeField] protected bool _checkTag;
        [SerializeField] protected LayerMask _layerMask;
	    [SerializeField] protected QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Collide;
        public abstract bool CheckForTarget(Transform targetTransform);
	    public abstract bool CheckForTarget(Transform targetTransform, float range);

	    public virtual bool CheckIfTarget(Transform checkTransform, Transform target)
	    {
	        // WARNING! - THIS CURRENTLY REFERENCES ROOTS!
            return (checkTransform == target || (checkTransform.IsChildOf(target)));
	    }

	}
}