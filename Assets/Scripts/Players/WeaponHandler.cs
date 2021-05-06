using System;
using System.Collections.Generic;
using Scripts.NewPlayerControls;
using UnityEngine;
using Weapons;

namespace Players
{
    public class WeaponHandler : MonoBehaviour
    {
        public List<Transform> HeldWeapons { get; private set; }

        private bool _isAiming;
        private Weapon _currWeaponScript;
        private FirstPersonController _fp_Controler;
        private AnimationHandler _animHandler;

        private void Awake()
        {
            HeldWeapons = new List<Transform>();
            AddAllWeapon();
            _animHandler = GetComponentInParent<AnimationHandler>();
        }

        private void Start()
        {
            _fp_Controler = GetComponentInParent<FirstPersonController>();
        }

        private void Update()
        {
            #region Animacao

            _isAiming = FirstPerson_InputHandler.AimKey;
            _animHandler.SetAim(_isAiming);
            if (_currWeaponScript.CanAttack())
                _animHandler.SetFire(FirstPerson_InputHandler.PrimaryFireKey_Auto); 

            #endregion


            if (FirstPerson_InputHandler.PrimaryFireKey_Auto)
            {
                _currWeaponScript.Attack(_isAiming);
            }
            
            
            

            
        }
        
        bool FireInputReceived()
        {
            return FirstPerson_InputHandler.PrimaryFireKey_Auto;
        }


        #region WeaponManagement

        private void AddHeldWeapon(Transform weapon)
        {
            if (weapon.GetComponent<Weapon>() == null)
            {
                throw new AccessViolationException("Only weapons can be associated in the HeldWeapons list");
            }
            
            HeldWeapons.Add(weapon);
        }

        private void AddAllWeapon()
        {
            foreach (Transform child in transform)
            {
                AddHeldWeapon(child);
            }
        }

        public int CountWeapons()
        {
            return HeldWeapons.Count;
        }

        public Weapon CurrWeaponScript
        {
            get => _currWeaponScript;
            set => _currWeaponScript = value;
        }
        
        public void WeaponSetup(GameObject weapon)
        {
            weapon.gameObject.SetActive(true);
            _currWeaponScript = weapon.GetComponent<Weapon>();
            // _currWeaponScript.FPControl = _fp_Controler;
            _animHandler.Anim = weapon.GetComponent<Animator>();
        }

        #endregion
        
        

        


    }
}