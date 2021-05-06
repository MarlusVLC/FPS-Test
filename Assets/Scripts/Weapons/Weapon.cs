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
        [Header("External Tools")]
        // [SerializeField] protected FirstPersonController fpControl;
        [SerializeField] protected Camera fpsCam;
        [SerializeField] protected TextMeshProUGUI statusUI;


        
        protected Animator _anim;
        // private WeaponHandler _weaponHandler;


        protected virtual void OnEnable()
        {
            _anim = GetComponent<Animator>();
        }


        // public FirstPersonController FPControl
        // {
        //     get => fpControl;
        //     set => fpControl = value;
        // }
        
        
        public Animator Anim
        {
            get => _anim;
            set { _anim = value; }
        }

        public abstract bool CanAttack();

        public abstract void Attack(bool changingCondition = false);
    }
}