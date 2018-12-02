using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody != null)
        {
            if(other.attachedRigidbody.tag == "Player")
            {
                Destroy(this.gameObject);
                PresentCollectionManager.Instance.AddPresent(1);
            }
        }
    }
}
