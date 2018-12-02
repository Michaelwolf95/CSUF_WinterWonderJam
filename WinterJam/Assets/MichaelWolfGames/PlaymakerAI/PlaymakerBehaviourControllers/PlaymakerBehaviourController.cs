using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;
using MichaelWolfGames.DamageSystem;

//using VRTK;

namespace MichaelWolfGames.AI.Playmaker
{
	public class PlaymakerBehaviourController : MonoBehaviour
	{
        //[Header("Transition Event Names")]
        //[SerializeField] protected string ResumeStateTransition = "RESUME";
        //[SerializeField] protected string InterruptStateTransition = "INTERRUPT";

        //================================================================================

	    #region Playmaker Variables

        public GameObject ControllerGameObject
        {
            get { return gameObject; }
        }

	    [SerializeField] private Transform _characterTransform;
	    public Transform CharacterTransform
	    {
	        get
	        {
	            if (!_characterTransform)
	            {
	                _characterTransform = transform;
	            }
	            return _characterTransform;
	        }
	    }

        [SerializeField] private Transform _lookTarget;
        public Transform LookTargetTransform
        {
            get
            {
                if (!_lookTarget)
                {
                    // ,,, Auto find player ...

                    if (!_lookTarget)
                    {
                        _lookTarget = new GameObject("Look Target").transform;
                        _lookTarget.position = transform.position;
                        //Debug.LogWarning("[PlaymakerBehaviourController]: Look Target Created.");
                    }
                }
                return _lookTarget;
            }
            set { _lookTarget = value; }
        }
        [SerializeField] private Transform _moveTarget;
        public Transform MoveTargetTransform
        {
            get
            {
                if (!_moveTarget)
                {
                    // .... Auto find player...
                    if (!_moveTarget)
                    {
                        _moveTarget = new GameObject("Move Target").transform;
                        _moveTarget.position = transform.position;
                        //Debug.LogWarning("[PlaymakerBehaviourController]: Move Target Created.");
                    }
                }
                return _moveTarget;
            }
            set { _moveTarget = value; }
        }

        protected Animator Anim;
        public Animator StateAnimator
        {
            get { return Anim; }
        }

        #endregion
        //================================================================================

	    #region Events

        public event DamageSystem.Damage.DamageEventHandler OnTakeDamage = delegate { };

        protected Action onDeath = delegate { };
        public Action OnDeath
        {
            get { return onDeath; }
            set { onDeath = value; }
        }

	    public Action OnEnterCombatAction = delegate { };
	    public Action OnExitCombatAction = delegate { };

	    public Action OnReset = delegate { };
        #endregion
        //================================================================================

        public bool SuspendStateActions { get; protected set; }

        public PlayMakerFSM playMakerFSM;
        
        protected Vector3 _startPosition;
        protected Quaternion _startRotation;

        protected virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
            playMakerFSM = GetComponent<PlayMakerFSM>();

            _startPosition = CharacterTransform.position;
            _startRotation = CharacterTransform.rotation;
        }


        protected virtual void Start()
        {
            if (playMakerFSM)
            {
                InitializePlaymakerVariables();
            }
        }

	    public void InitializePlaymakerVariables()
	    {
            if(!playMakerFSM) playMakerFSM = GetComponent<PlayMakerFSM>();
            if (CharacterTransform) playMakerFSM.FsmVariables.GetFsmGameObject("CharacterRoot").Value = CharacterTransform.gameObject;
            if (LookTargetTransform) playMakerFSM.FsmVariables.GetFsmGameObject("LookTarget").Value = LookTargetTransform.gameObject;
            if (MoveTargetTransform) playMakerFSM.FsmVariables.GetFsmGameObject("MoveTarget").Value = MoveTargetTransform.gameObject;
	    }

        FsmGameObject GetGameObjectVariable(string targetName)
        {
            foreach (FsmGameObject go in playMakerFSM.FsmVariables.GameObjectVariables)
            {
                if (go.Name == targetName)
                    return go;
            }
            return null;
        }

	    public void TogglePlaymaker(bool state)
	    {
	        playMakerFSM.enabled = state;
            ToggleStateActions(state);
	    }

        public void ToggleStateActions(bool state)
        {
            SuspendStateActions = state;
        }

	    public void DoOnReset()
	    {
	        if (OnReset != null)
	        {
                ResetPositionAndRotation();
                OnReset();
	        }
	    }

	    public void ResetPositionAndRotation()
	    {
	        CharacterTransform.position = _startPosition;
	        CharacterTransform.rotation = _startRotation;
	    }

	    public void SendPlaymakerEvent(string eventName)
	    {
	        playMakerFSM.SendEvent(eventName);
	    }

	    protected void ToggleCharacterController(bool state)
	    {
	        
	    }
    }
}
