using System;
using Scripts.NewPlayerControls;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Weapons;

namespace Players
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private WeaponHandler _weaponHandler;
        [SerializeField] private GrenadeThrower _grenadeThrower;
        [SerializeField] private AnimationHandler _animHandler;
        [SerializeField] private FirstPersonMovement _movement;

        private int _weaponKey;
        private bool _isAiming;
        private Weapon _currWeapon;


        private void Update()
        {
            SetMovementDirection();
            _isAiming = Input.GetMouseButton(1);
            SetJump();
            SetCrouch();
            SetSprint();
            ChangeWeapons();
            SetAim();
            TryFire();
            TryThrowGrenade();
            TriggerReload();
        }


        private void SetMovementDirection()
        {
            _movement.SetMovementOrientation(CrossPlatformInputManager.GetAxis("Horizontal"), 
                CrossPlatformInputManager.GetAxis("Vertical"));
        }

        private void ChangeWeapons()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _weaponHandler.SetWeaponThroughMousewheel((int) Input.mouseScrollDelta.y);
                return;
            }
            

            if (Int32.TryParse(Input.inputString, out _weaponKey))
            {
                _weaponHandler.SetWeaponThroughKeyboard(_weaponKey);
            }
        }

        private void TryFire()
        {
            switch (_weaponHandler.CurrWeapon.AttackType)
            {
                case AttackType.AutoFire:
                    if (_weaponHandler.CurrWeapon.CanAttack())
                    {
                        _animHandler.SetFire(Input.GetButton("Fire1")); 
                        _weaponHandler.CurrWeapon.Attack(Input.GetButton("Fire1"), _isAiming);
                    }
                    break;
                case AttackType.PumpFire:
                    if (_weaponHandler.CurrWeapon.CanAttack())
                    {
                        _animHandler.SetFire(Input.GetButtonDown("Fire1")); 
                        _weaponHandler.CurrWeapon.Attack(Input.GetButtonDown("Fire1"), _isAiming);
                    }
                    break;
            }

        }

        private void SetAim()
        {
            _animHandler.SetAim(_isAiming);
        }

        private void TriggerReload()
        {
            if (_weaponHandler.CurrWeapon.CanExecSpecialAction0() && Input.GetButtonDown("Reload"))
            {
                _weaponHandler.CurrWeapon.ExecSpecialAction0();
            }
        }

        private void SetJump()
        {
            if (!_movement.Jump)
            {
                _movement.Jump = Input.GetButtonDown("Jump");
            }
        }

        private void SetCrouch()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _movement.ToggleCrouch();
            }
        }
        
        private void SetSprint()
        {
            _movement.IsWalking = !Input.GetButton("Sprint");
        }

        private void TryThrowGrenade()
        {
            if (Input.GetMouseButtonDown(2))
                _grenadeThrower.GrenadeThrowProcess();
        }
        
    }
}