using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MichaelWolfGames.MeterSystem;
using TMPro;

public class MeterText_TMP : MeterBase
{
    public TextMeshProUGUI m_Text;
    public bool ShowPercent;
    public bool Bold;
    public bool CastAsInteger;

    protected virtual void Start()
    {
        if (!m_Text) m_Text = this.GetComponent<TextMeshProUGUI>();
    }

    protected override void UpdateMeter(float percentValue)
    {
        if (m_Text)
        {
            string str = "";
            if (ShowPercent)
            {
                str = Meterable.PercentValue.ToString("P");
            }
            else
            {
                if (CastAsInteger)
                    str = ((int)Meterable.CurrentValue).ToString();// + "/" + ((int)Meterable.MaxValue);
                else
                    str = (Meterable.CurrentValue).ToString("0.##") + "/" + Meterable.MaxValue.ToString("0.##");
            }

            if (Bold) str = "<b>" + str + "</b>";

            m_Text.text = str;
        }
    }
}
