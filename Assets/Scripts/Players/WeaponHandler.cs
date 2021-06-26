using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Players
{
    public class WeaponHandler : MonoBehaviour
    {
        public List<Weapon> HeldWeapons { get; private set; }
        public Weapon CurrWeapon { get; private set;}
        
        private AnimationHandler AnimHandler;
        private int _selectedWeapon;


        
        
        private void Awake()
        {
            HeldWeapons = new List<Weapon>();
            AddAllWeapons();
            AnimHandler = GetComponentInParent<AnimationHandler>();
            EnableWeapon(_selectedWeapon = 0);
        }



        #region --WeaponManagement--

        
        
        private void AddHeldWeapon(Weapon weapon)
        {
            HeldWeapons.Add(weapon);
        }

        private void AddAllWeapons()
        {
            foreach (Transform child in transform)
            {
                AddHeldWeapon(child.GetComponent<Weapon>());
            }
        }

        private int CountWeapons()
        {
            return HeldWeapons.Count;
        }
        
        private void WeaponSetup(Weapon weapon)
        {
            weapon.gameObject.SetActive(true);
            CurrWeapon = weapon;
            AnimHandler.Anim = weapon.Anim;
        }

        
        
        
        #endregion
        
        
        
        
        
        #region --WeaponSwitching--
        
        
        
        
        private void EnableWeapon(int index)
        {
            var i = 0;
            foreach (var weapon in HeldWeapons)
            {

                if (i == index)
                {
                    WeaponSetup(weapon);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                i++;
            }
        }
        
        public void SetWeaponThroughKeyboard(int selectKey)
        {
            if (selectKey > 0 && selectKey <= CountWeapons())
            {
                _selectedWeapon = selectKey-1;
                EnableWeapon(_selectedWeapon);
            }
        }
        
        public void SetWeaponThroughMousewheel(int selectKey)
        {
            _selectedWeapon += selectKey;
            if (_selectedWeapon < 0)
            {
                _selectedWeapon = CountWeapons() - 1;
            }
            else
            {
                _selectedWeapon %= CountWeapons();
            }
            EnableWeapon(_selectedWeapon);
        }
        
        
        
        #endregion
        
    }
}