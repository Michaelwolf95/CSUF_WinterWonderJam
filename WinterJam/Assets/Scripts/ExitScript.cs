﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ExitScript : MonoBehaviour {
    public GameObject resultMenu;
    public TextMeshProUGUI presentText;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.tag == "Player" && PresentCollectionManager.Instance.PercentValue >= (float).7)
            {
                //MessageDisplayer.instance.DisplayMessage("You Win!", 3);
                //Destroy(this.gameObject);
                Time.timeScale = 0;
                resultMenu.SetActive(true);
                presentText.text = "Presents: " + PresentCollectionManager.Instance.CurrentValue;
            }
        }
    }
}
