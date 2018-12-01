using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentRotation : MonoBehaviour {

    public float rotation = 5.0f;
    public float hopHeight = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * rotation);
        transform.localPosition = new Vector3(0f, hopHeight * (0.5f + (Mathf.Sin(Time.time) / 2f)), 0f);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

        }
    }
}
