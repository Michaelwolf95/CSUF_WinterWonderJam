using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    //Orient the camera after all movement is completed this frame to avoid jittering
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
             Camera.main.transform.rotation * Vector3.up);
    }
}