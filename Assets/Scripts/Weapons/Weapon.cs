using System;
using System.Collections;
using DefaultNamespace;
using Players;
using Scripts.NewPlayerControls;
using TMPro;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoBehaviour
    {
        // [SerializeField] protected Transform _head;
        protected Animator _anim;
        public AttackType AttackType { get; protected set; }


        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        
        

        
        
        
        
        public Animator Anim
        {
            get => _anim;
            set { _anim = value; }
        }

        public abstract bool CanAttack();

        public abstract bool CanExecSpecialAction0();

        public abstract void Attack(Transform eyeOrigin, bool inputReceived, bool changingCondition = false);

        public abstract void ExecSpecialAction0();
    }
}