using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.DetectorSystem
{
	public class RadialDetector : DetectorBase
	{
	    [SerializeField] private float _defaultRange = 5f;
        public bool seeThroughColliders = false;

	    public override bool CheckForTarget(Transform targetTransform)
        {
            if (Vector3.Distance(targetTransform.position, transform.position) > _defaultRange) return false;
            return CheckRaycastLineOfSight(targetTransform, _defaultRange, _layerMask);
        }

        public override bool CheckForTarget(Transform targetTransform, float range)
        {
            if (Vector3.Distance(targetTransform.position, transform.position) > range) return false;
            return CheckRaycastLineOfSight(targetTransform, range, _layerMask);
        }
        // ToDo: Add overload with layermask to Interface and Base Class.

        protected bool CheckRaycastLineOfSight(Transform targetTransform, float checkRange, LayerMask layerMask)
        {
            var eyePos = this.transform.position;
            var eye2Target = targetTransform.position - eyePos;

            if(seeThroughColliders == true)
            {
                RaycastHit[] hits = Physics.RaycastAll(eyePos, eye2Target.normalized, checkRange, layerMask, _triggerInteraction);
                foreach (RaycastHit h in hits)
                {
                    //debugString += "Raycast hit -> ";
                    if (!_checkTag || (_checkTag && h.collider.gameObject.CompareTag(_targetTag)))
                    {
                        var hitTransform = (h.collider.attachedRigidbody)
                                                ? h.collider.attachedRigidbody.transform
                                                : h.collider.transform;
                        if (CheckIfTarget(hitTransform, targetTransform))
                        {
                            Debug.DrawLine(eyePos, h.point, Color.green);

                            return true;
                        }
                        else
                        {
                            Debug.DrawLine(eyePos, h.point, Color.red);
                        }
                    }
                    else
                    {
                        //Debug.DrawLine(eyePos, targetTransform.position, Color.cyan);
                    }
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(eyePos, eye2Target.normalized, out hit, checkRange, layerMask, _triggerInteraction))
                {
                    //debugString += "Raycast hit -> ";
                    GameObject hitGo = (hit.collider.attachedRigidbody) ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;
                    if (!_checkTag || (_checkTag && hitGo.CompareTag(_targetTag)))
                    {
                        var hitTransform = hitGo.transform;
                        if (CheckIfTarget(hitTransform, targetTransform))
                        {
                            Debug.DrawLine(eyePos, hit.point, Color.green);
                        
                            return true;
                        }
                        else
                        {
                            Debug.DrawLine(eyePos, hit.point, Color.red);
                        }
                    }
                    else
                    {
                        Debug.DrawLine(eyePos, targetTransform.position, Color.cyan);
                    }
                }
                else
                {
                    Debug.DrawLine(eyePos, targetTransform.position, Color.magenta);
                }
            }
            return false;
        }

	}
}