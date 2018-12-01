using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplayer : MonoBehaviour {

    public bool isDisplay;
    public float testDuration;
    public Text textElement;
    public string testString;

	// Use this for initialization
	void Start () {
        isDisplay = false;
	}
	
	// TESTING ONLY
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayMessage(testString, testDuration);
        }
	}
    public void DisplayMessage(string message, float duration)
    {
        isDisplay = true;
        StartCoroutine(MessageOnCoro(message, duration));
    }

    IEnumerator MessageOnCoro(string message, float duration)
    {
        textElement.enabled = true;
        textElement.text = message;
        yield return new WaitForSeconds(duration);
        textElement.enabled = false;
    }
}
