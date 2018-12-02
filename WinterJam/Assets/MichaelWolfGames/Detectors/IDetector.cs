using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MichaelWolfGames.DetectorSystem
{
	public interface IDetector
	{
		//Transform _targetTransform { get; }
	    bool CheckForTarget(Transform targetTransform);
	    bool CheckForTarget(Transform targetTransform, float range);
	}
}