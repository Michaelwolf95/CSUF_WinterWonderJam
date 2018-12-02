using UnityEngine;

namespace MichaelWolfGames.DetectorSystem
{
    /// <summary>
    /// This optional component can be added to and referenced by any Detector 
    /// to change how it interprets its detected objects.
    /// </summary>
    public abstract class DetectorConditionBase : MonoBehaviour
    {
        public abstract bool Check(GameObject gameObject);
    }
}