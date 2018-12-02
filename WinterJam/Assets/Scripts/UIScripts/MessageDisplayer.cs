using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDisplayer : MonoBehaviour
{

    public TextMeshProUGUI textElement;

    private void Start()
    {
        textElement.enabled = false;
    }

    public void DisplayMessage(string message, float duration)
    {
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
