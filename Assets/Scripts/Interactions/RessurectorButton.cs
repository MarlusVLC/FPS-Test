using System;
using DefaultNamespace.Interactions;
using UnityEngine;
using Weapons;

namespace DefaultNamespace
{
    public class RessurectorButton : Interactive
    {
        private Animator _anim;
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public override void Interact()
        {
            _anim.SetTrigger("Click");
            TargetManager.GetInstance.RaiseTargets();
        }
        
        
    }
}