using System;
using DefaultNamespace;
using Players;
using Scripts.NewPlayerControls;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(KeepTransform))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected FirstPersonController fpControl;
        
        protected Animator _anim;
        private WeaponHandler _weaponHandler;


        public void Awake()
        {
            
        }



        public FirstPersonController FPControl
        {
            get => fpControl;
            set => fpControl = value;
        }
        
        public Animator Anim
        {
            get => _anim;
            set { _anim = value; }
        }
    }
}