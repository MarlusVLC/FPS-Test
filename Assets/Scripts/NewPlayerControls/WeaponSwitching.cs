using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Scripts.NewPlayerControls
{
    public class WeaponSwitching : MonoBehaviour
    {
        [SerializeField] private FirstPersonController fp_Controler;
        [SerializeField] private Weapon currWeapon;
        [SerializeField] private int selectedWeapon;
        


        void Start()
        {

            SelectWeapon();
        }


        private void Update()
        {
            InputSelect();
        }




        private void SelectWeapon()
        {
            int i = 0;
            foreach (Transform weapon in transform)
            {
                if (i == selectedWeapon)
                {
                    weapon.gameObject.SetActive(true);
                    currWeapon = weapon.GetComponent<Weapon>();
                    currWeapon.FPControl = fp_Controler;
                    fp_Controler.Anim = currWeapon.Anim;
                }

                else
                    weapon.gameObject.SetActive(false);
                i++;
            }
        }

        private void InputSelect()
        {
            
            int previousSelectedWeapon = selectedWeapon;
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                SelectWeapon();
            }
        }
    }
}