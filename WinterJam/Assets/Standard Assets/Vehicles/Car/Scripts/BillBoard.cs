using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;

    //Orient the camera after all movement is completed this frame to avoid jittering
    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}