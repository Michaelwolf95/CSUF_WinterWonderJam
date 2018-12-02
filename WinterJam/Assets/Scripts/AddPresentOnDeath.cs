using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MichaelWolfGames.DamageSystem;
using UnityEngine.AI;

public class AddPresentOnDeath : HealthManagerEventListenerBase
{
    public int count = 2;

    public Rigidbody rb;
    public NavMeshAgent agent;
    protected override void DoOnDeath()
    {
        PresentCollectionManager.Instance.AddPresent(count);
        agent.enabled = false;
        rb.isKinematic = false;
    }

    protected override void DoOnRevive()
    {
    }

    protected override void DoOnTakeDamage(object sender, Damage.DamageEventArgs damageEventArgs)
    {
    }

}
