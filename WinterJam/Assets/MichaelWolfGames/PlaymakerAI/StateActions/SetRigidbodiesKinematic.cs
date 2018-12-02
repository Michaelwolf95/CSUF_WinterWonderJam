using HutongGames.PlayMaker;
using UnityEngine;

namespace MichaelWolfGames.AI.Playmaker
{
    public class SetRigidbodiesKinematic : FsmStateAction
    {
        public Rigidbody[] Rigidbodies;
        public FsmBool State;
        public override void OnEnter()
        {
            base.OnEnter();
            foreach (Rigidbody rigidbody in Rigidbodies)
            {
                rigidbody.isKinematic = State.Value;
            }
            Finish();
        }
    }
}