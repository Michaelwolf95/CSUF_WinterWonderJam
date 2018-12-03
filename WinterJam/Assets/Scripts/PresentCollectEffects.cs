using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MichaelWolfGames;
using MichaelWolfGames.MeterSystem;
using TMPro;
using System;

public class PresentCollectEffects : SubscriberBase<PresentCollectionManager>, IMeterable
{
    public Animator effectAnimator;
    public string effectAnimState = "CollectEffect";
    public AudioSource effectAudioSource;
    public AudioClip collectSound;
    public float effectTime = 0.15f;
    public float delayTime = 0.1f;
    public TextMeshProUGUI textElement;

    private Queue<Action> effectQueue = new Queue<Action>();
    private bool effectPlaying = false;
    private int lastValue = 0;

    private int fakeCurrentValue = 0;

    public float CurrentValue
    {
        get
        {
            return fakeCurrentValue;
        }
    }

    public float MaxValue { get { return SubscribableObject.MaxValue; } }

    public float PercentValue { get { return fakeCurrentValue / SubscribableObject.MaxValue; } }

    public Action<float> OnUpdateValue { get; set; } 

    protected override void DoOnInitialize()
    {
    }

    protected override void SubscribeEvents()
    {
        SubscribableObject.OnUpdateValue += OnUpdateCollection;
    }

    protected override void UnsubscribeEvents()
    {
        SubscribableObject.OnUpdateValue -= OnUpdateCollection;
    }

    protected void OnUpdateCollection(float percentValue)
    {
        int currentValue = (int)SubscribableObject.CurrentValue;
        
        if(currentValue > lastValue)
        {
            for (int i = 1; i <= (currentValue - lastValue); i++)
            {
                int value = lastValue + i;
                effectQueue.Enqueue(() =>
                {
                    StartCoroutine(CoCollectEffect(value));
                });
            }
            
            if(effectPlaying == false)
            {
                effectQueue.Dequeue()();
            }
        }

        lastValue = currentValue;
    }

    private IEnumerator CoCollectEffect(int value)
    {
        effectPlaying = true;
        yield return new WaitForSeconds(delayTime);
        effectAnimator.Play(effectAnimState);
        effectAudioSource.PlayOneShot(collectSound, effectAudioSource.volume);

        fakeCurrentValue = value;

        if (textElement != null)
        {
            textElement.text = (fakeCurrentValue).ToString();
        }

        OnUpdateValue(PercentValue);
        yield return new WaitForSeconds(effectTime);

        if(effectQueue.Count > 0)
        {
            effectQueue.Dequeue()();
        }
        else
        {
            effectPlaying = false;
        }
    }
}
