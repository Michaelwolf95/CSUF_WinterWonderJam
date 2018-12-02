using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MichaelWolfGames;
using MichaelWolfGames.MeterSystem;
using System;

public class PresentCollectionManager : Singleton<PresentCollectionManager>, IMeterable
{

    [SerializeField] private int currentCount;
    public int maxCount;

    public float CurrentValue
    {
        get
        {
            return currentCount;
        }
    }

    public float MaxValue
    {
        get
        {
            return maxCount;
        }
    }

    public float PercentValue
    {
        get
        {
            return ((float)currentCount) / ((float)maxCount);
        }
    }

    public Action<float> OnUpdateValue { get; set; }

    // Use this for initialization
    void Start () {
        OnUpdateValue(0);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddPresent(1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddPresent(-1);
        }
    }
    public void AddPresent(int addAmount)
    {
        currentCount = Mathf.Clamp(currentCount + addAmount, 0, maxCount);
        OnUpdateValue(currentCount);
    }
}
