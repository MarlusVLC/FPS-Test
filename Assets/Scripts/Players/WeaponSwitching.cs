using System;
using Players;
using UnityEngine;
using Weapons;

namespace Scripts.NewPlayerControls
{
    [RequireComponent(typeof(WeaponHandler))]
    public class WeaponSwitching : MonoBehaviour
    {
        [SerializeField] private FirstPersonController fp_Controler;
        [SerializeField] private Weapon currWeapon;
        [SerializeField] private int selectedWeapon;
        
        private WeaponHandler _weaponHandler;
        private int _selectKey;
        
        void Start()
        {
            fp_Controler = GetComponentInParent<FirstPersonController>();
            _weaponHandler = GetComponent<WeaponHandler>();
            SelectWeapon(selectedWeapon = 0);
        }


        private void Update()
        {
            InputSelect();
        }

        
        
        private void SelectWeapon(int index)
        {
            int i = 0;
            foreach (Transform weapon in _weaponHandler.heldWeapons)
            {
                if (i == index)
                {
                    WeaponSetup(weapon.gameObject);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                i++;
            }
        }


        private void InputSelect()
        {
            if (FirstPerson_InputHandler.WeaponWheel != 0)
            {
                SetWeaponThroughMousewheel((int) FirstPerson_InputHandler.WeaponWheel);
                return;
            }
            

            if (Int32.TryParse(Input.inputString, out _selectKey))
            {
                SetWeaponThroughKeyboard(_selectKey);
            }
        }

        private void WeaponSetup(GameObject weapon)
        {
            weapon.gameObject.SetActive(true);
            currWeapon = weapon.GetComponent<Weapon>();
            currWeapon.FPControl = fp_Controler;
            fp_Controler.Anim = currWeapon.Anim;
        }


        private void SetWeaponThroughKeyboard(int selectKey)
        {
            if (selectKey > 0 && selectKey <= _weaponHandler.CountWeapons())
            {
                selectedWeapon = selectKey-1;
                SelectWeapon(selectedWeapon);
            }
        }

        private void SetWeaponThroughMousewheel(int selectKey)
        {
            selectedWeapon += selectKey;
            if (selectedWeapon < 0)
            {
                selectedWeapon = _weaponHandler.CountWeapons() - 1;
            }
            else
            {
                selectedWeapon %= _weaponHandler.CountWeapons();
            }
            SelectWeapon(selectedWeapon);
        }



    }
}