using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour {

    private bool isCollected = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if(other.attachedRigidbody != null)
        {
            if(other.attachedRigidbody.tag == "Player")
            {
                isCollected = true;
                Destroy(this.gameObject);
                PresentCollectionManager.Instance.AddPresent(1);
            }
        }
    }
}
