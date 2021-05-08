using System;
using DefaultNamespace;
using Players;
using Scripts.NewPlayerControls;
using TMPro;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    public abstract class Weapon : MonoBehaviour
    {
        protected Camera _fpsCam;
        protected Animator _anim;

        protected virtual void Start()
        {
            _fpsCam = Camera.main;
        }
        
        protected virtual void OnEnable()
        {
            _anim = GetComponent<Animator>();
        }

        
        
        
        
        public Animator Anim
        {
            get => _anim;
            set { _anim = value; }
        }

        public abstract bool CanAttack();

        public abstract void Attack(bool inputReceived, bool changingCondition = false);
    }
}