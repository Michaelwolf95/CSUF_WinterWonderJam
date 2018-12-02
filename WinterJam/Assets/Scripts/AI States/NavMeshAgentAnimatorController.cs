using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAnimatorController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator; 
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat("Speed", agent.velocity.magnitude);
	}
}
