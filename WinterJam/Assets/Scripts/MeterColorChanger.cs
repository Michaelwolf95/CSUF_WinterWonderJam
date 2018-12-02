using System.Collections;
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

    protected override void UpdateMeter(float percentValue)
    {
        //((IMeterable)MeterableObject).CurrentValue
        //Debug.Log(percentValue);
        if (percentValue >= (float).7)
        {
            image.color = endColor;
            if (!enoughPresent)
            {
                if(MessageDisplayer.instance != null)
                    MessageDisplayer.instance.DisplayMessage("Enough presents obtained", 3);
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
