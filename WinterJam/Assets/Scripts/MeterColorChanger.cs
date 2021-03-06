﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MichaelWolfGames;
using MichaelWolfGames.MeterSystem;
using UnityEngine.UI;

public class MeterColorChanger : MeterBase
{
    public Image image;
    public Color startColor;
    public Color endColor;
    private bool enoughPresent = false;

    public string exitMessage = "Enough presents obtained";
    public float displayTime = 7.5f;

    protected override void UpdateMeter(float percentValue)
    {
        //((IMeterable)MeterableObject).CurrentValue
        //Debug.Log(percentValue);
        if (percentValue >= (float).5)
        {
            image.color = endColor;
            if (!enoughPresent)
            {
                if(MessageDisplayer.instance != null)
                    MessageDisplayer.instance.DisplayMessage(exitMessage, displayTime);
                enoughPresent = true;
            }
        }
        else
        {
            enoughPresent = false;
            image.color = startColor;
        }
        
    }

}
