﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
    
	// Use this for initialization
	public void loadGame()
    {
        SceneManager.LoadScene(1);
        //PresentCollectionManager.Instance.OnUpdateValue(0);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
