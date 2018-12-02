using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAtVelocity : MonoBehaviour
{
    public GameObject target;
    public Rigidbody rb;
    public float activationVelocity = 5f;

	// Use this for initialization
	void Start () {
		if(target == null && transform.GetChild(0) != null)
        {
            target = transform.GetChild(0).gameObject;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (rb == null) return;

        target.SetActive(rb.velocity.magnitude > activationVelocity);

	}
}
