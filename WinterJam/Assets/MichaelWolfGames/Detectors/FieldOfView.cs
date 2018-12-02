using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.DetectorSystem
{

    /// <summary>
    /// Utility class used for detection using Field Of View
    /// 
    /// </summary>
    public class FieldOfView : DetectorBase
    {
        private Transform _eyeTransform;

        [Tooltip("The Maximum Distance the view will reach.")]
        [SerializeField] private float _defaultRange = 10f;

        //[Tooltip("The scale of the view at detection distance of 1")]
        [SerializeField] private Vector2 _viewProportions = new Vector2(0.75f, 1f);
        [Range(0, 180f)] [SerializeField] private float _xAngle = 45f;
        [Range(0, 180f)] [SerializeField] private float _yAngle = 30f;

        [SerializeField] private Vector3 _standardOffset = new Vector3(0f, 0.2f, 0f);
        [SerializeField] private bool _drawDetectionBounds;

        private void Awake()
        {
            _eyeTransform = transform;
        }

        public void Update()
        {
            if (_drawDetectionBounds)
            {
                _viewProportions =
                    new Vector2(CalculateBaseLengthFromAngleAndHeight(1, _xAngle),
                        CalculateBaseLengthFromAngleAndHeight(1, _yAngle))/2f;
                DrawBounds();
            }
        }

        // For IDetector Interface and DetectorBase
        public override bool CheckForTarget(Transform target)
        {
            if (target == null) return false;
            if (Vector3.Distance(target.position, transform.position) > _defaultRange) return false;
            return CanSeeTarget(target);
        }
        public override bool CheckForTarget(Transform target, float maxRange)
        {
            if (target == null) return false;
            if (Vector3.Distance(target.position, transform.position) > maxRange) return false;
            return CanSeeTarget(target, maxRange);
        }

        public bool CanSeeTarget(Transform target)
        {
            return CanSeeTarget(target, _defaultRange, _layerMask);
        }

        public bool CanSeeTarget(Transform target, LayerMask layerMask)
        {
            return CanSeeTarget(target, _defaultRange, layerMask);
        }

        public bool CanSeeTarget(Transform target, float maxDetectionDistance)
        {
            return CanSeeTarget(target, maxDetectionDistance, _layerMask);
        }

        public bool CanSeeTarget(Transform target, float maxDetectionDistance, LayerMask layerMask)
        {
            if (target == null) return false;
            //string debugString = "";
            var eye2Target = (target.position + _standardOffset)- _eyeTransform.position;
            // Only bother calculating detection if within maxDetectionDistance.
            if (eye2Target.magnitude < maxDetectionDistance)
            {
                // If the target is behind the FOV, there is no way it could see them. This also prevents possible problems with the following calculations due to multiplicity.
                if (_eyeTransform.InverseTransformPoint(target.position).z < 0f) return false;

                var targetRelativeToEye = _eyeTransform.InverseTransformPoint(target.position);
                var projectedVector = Vector3.Project(targetRelativeToEye, Vector3.forward);
                if (projectedVector.magnitude > 0f)
                {
                    //debugString += "Within Range -> ";
                    var targetOnLookPlane = Vector3.ProjectOnPlane(targetRelativeToEye, Vector3.forward);

                    if (Mathf.Abs(targetOnLookPlane.x) <=
                        CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, _xAngle)/2)
                    {
                        if (Mathf.Abs(targetOnLookPlane.y) <=
                            CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, _yAngle)/2)
                        {
                            //debugString += "Within Bounds -> ";
                            //Ray eye2TargetRay = new Ray(_eyeTransform.position, eye2Target.normalized);
                            //Debug.DrawRay(_eyeTransform.position, eye2Target.normalized*maxDetectionDistance, Color.cyan);
                            RaycastHit hit;
                            if (Physics.Raycast(_eyeTransform.position, eye2Target, out hit, maxDetectionDistance,
                                layerMask, _triggerInteraction))
                            {
                                Debug.DrawRay(_eyeTransform.position, eye2Target.normalized * maxDetectionDistance, Color.cyan);
                                //debugString += "Raycast hit -> ";
                                if (!_checkTag || (_checkTag && hit.collider.gameObject.CompareTag(_targetTag)))
                                {
                                    //debugString += "Raycast saw player -> ";
                                    float radius = 0.2f;
                                    if (Physics.SphereCast(_eyeTransform.position, radius, eye2Target, out hit,
                                        maxDetectionDistance, layerMask))
                                    {
                                        //debugString += "Spherecast hit -> ";
                                        Vector3 eye2point = hit.point - _eyeTransform.position;
                                        Debug.DrawLine(_eyeTransform.position, hit.point, Color.green);
                                        Debug.DrawLine(_eyeTransform.position,
                                            _eyeTransform.position + Vector3.Project(eye2point, eye2Target.normalized),
                                            Color.green);
                                        //debugString += "Spherecast saw target.";
                                        var hitTransform = (hit.collider.attachedRigidbody)
                                            ? hit.collider.attachedRigidbody.transform
                                            : hit.collider.transform;
                                        if (CheckIfTarget(hitTransform, target))
                                        {
                                            //debugString += "Spherecast saw target.";
                                            Debug.DrawLine(_eyeTransform.position, hit.point, Color.red, 2f);
                                            return true;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                Debug.DrawRay(_eyeTransform.position, eye2Target.normalized * maxDetectionDistance, Color.magenta);
                            }
                            //Debug.Log(projectedVector + " , " + projectedVector.magnitude + " , " + targetOnLookPlane + " , " + CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, _yAngle)/2);
                        }
                    }

                }
            }
            return false;
        }

        private static float CalculateBaseLengthFromAngleAndHeight(float viewDist, float viewAngle)
        {
            // Note: viewDist = height of isosceles triangle.
            var phi = viewAngle/2f;
            var bse = ((2*viewDist)*Mathf.Sin(phi*Mathf.Deg2Rad))/(Mathf.Sin((90 - phi)*Mathf.Deg2Rad));
            return bse;
        }

        private void DrawBounds()
        {
            var boundsColor = Color.red;
            var center = _eyeTransform.position + (_eyeTransform.forward*_defaultRange);
            var endCorner1 = center - (_eyeTransform.up*(_viewProportions.y*_defaultRange)) +
                             (_eyeTransform.right*(_viewProportions.x*_defaultRange));
            var endCorner2 = center + (_eyeTransform.up*(_viewProportions.y*_defaultRange)) +
                             (_eyeTransform.right*(_viewProportions.x*_defaultRange));
            var endCorner3 = center + (_eyeTransform.up*(_viewProportions.y*_defaultRange)) -
                             (_eyeTransform.right*(_viewProportions.x*_defaultRange));
            var endCorner4 = center - (_eyeTransform.up*(_viewProportions.y*_defaultRange)) -
                             (_eyeTransform.right*(_viewProportions.x*_defaultRange));
            Debug.DrawLine(_eyeTransform.position, endCorner1, boundsColor);
            Debug.DrawLine(_eyeTransform.position, endCorner2, boundsColor);
            Debug.DrawLine(_eyeTransform.position, endCorner3, boundsColor);
            Debug.DrawLine(_eyeTransform.position, endCorner4, boundsColor);
            Debug.DrawLine(endCorner1, endCorner2, boundsColor);
            Debug.DrawLine(endCorner2, endCorner3, boundsColor);
            Debug.DrawLine(endCorner3, endCorner4, boundsColor);
            Debug.DrawLine(endCorner4, endCorner1, boundsColor);
        }

        public LayerMask GetDetectionMask()
        {
            return _layerMask;
        }

        /* ToDo: Make a method-defined FOV method that wraps back into the main calculations.
         * 
        /// <summary>
        /// Use this for method-defined FOV angles rather than the defined angles in this object. 
        /// </summary>
        /// <param name="xAngle"></param>
        /// <param name="yAngle"></param>
        /// <param name="target"></param>
        /// <param name="maxDetectionDistance"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public bool CanSeeTarget(float xAngle, float yAngle, Transform target, float maxDetectionDistance,
            LayerMask layerMask)
        {
            var debugString = "";
            var eye2Target = target.position - _eyeTransform.position; //_eyeTransform.position - target.position;
            // Only bother calculating detection if within maxDetectionDistance.
            if (eye2Target.magnitude < maxDetectionDistance)
            {
                // If the target is behind the FOV, there is no way it could see them. This also prevents possible problems with the following calculations due to multiplicity.
                if (_eyeTransform.InverseTransformPoint(target.position).z < 0f) return false;

                var targetRelativeToEye = _eyeTransform.InverseTransformPoint(target.position);
                var projectedVector = Vector3.Project(targetRelativeToEye, Vector3.forward);
                if (projectedVector.magnitude > 0f)
                {
                    debugString += "Within Range -> ";
                    var projectedPoint = _eyeTransform.TransformPoint(projectedVector);
                    var point2Target = projectedPoint - target.position;
                    var targetOnLookPlane = Vector3.ProjectOnPlane(targetRelativeToEye, Vector3.forward);

                    if (Mathf.Abs(targetOnLookPlane.x) <=
                        CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, xAngle)/2)
                    {
                        if (Mathf.Abs(targetOnLookPlane.y) <=
                            CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, yAngle)/2)
                        {

                            debugString += "Within Bounds -> ";
                            Ray eye2TargetRay = new Ray(_eyeTransform.position, eye2Target.normalized);
                            Debug.DrawRay(_eyeTransform.position, eye2Target.normalized*maxDetectionDistance, Color.cyan);
                            RaycastHit hit;
                            if (Physics.Raycast(_eyeTransform.position, eye2Target, out hit, maxDetectionDistance,
                                _detectionLayerMask))
                            {
                                debugString += "Raycast hit -> ";
                                if (hit.collider.gameObject.tag == "Player")
                                {
                                    debugString += "Raycast saw player -> ";
                                    float radius = 0.2f;
                                    if (Physics.SphereCast(_eyeTransform.position, radius, eye2Target, out hit,
                                        maxDetectionDistance, _detectionLayerMask))
                                    {
                                        debugString += "Spherecast hit -> ";
                                        Vector3 eye2point = hit.point - _eyeTransform.position;
                                        Debug.DrawLine(_eyeTransform.position, hit.point, Color.green);
                                        Debug.DrawLine(_eyeTransform.position,
                                            _eyeTransform.position + Vector3.Project(eye2point, eye2Target.normalized),
                                            Color.green);
                                        debugString += "Spherecast saw target.";
                                        if (hit.collider.transform == target)
                                        {
                                            //DoOnDetectPlayer();
                                            Debug.DrawLine(_eyeTransform.position, hit.point, Color.green, 2f);
                                            return true;
                                        }
                                    }
                                }
                            }
                            //Debug.Log(projectedVector + " , " + projectedVector.magnitude + " , " + targetOnLookPlane + " , " + CalculateBaseLengthFromAngleAndHeight(projectedVector.magnitude, _yAngle)/2);
                        }
                    }

                }
            }
            return false;
        }

        public bool CanSeeTarget(float xAngle, float yAngle, Transform target)
        {
            return CanSeeTarget(xAngle, yAngle, target, _defaultRange, _detectionLayerMask);
        }
        */
    }
}