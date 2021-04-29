using System;
using DefaultNamespace;
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