using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Players
{
    public class WeaponHandler : MonoBehaviour
    {
        public List<Transform> heldWeapons { get; private set; }

        public void Start()
        {
            heldWeapons = new List<Transform>();
            AddAllWeapon();
        }
        


        #region WeaponManagement

        public void AddHeldWeapon(Transform weapon)
        {
            if (weapon.GetComponent<Weapon>() == null)
            {
                throw new AccessViolationException("Only weapons can be associated in the HeldWeapons list");
            }
            
            heldWeapons.Add(weapon);
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
            return heldWeapons.Count;
        }

        #endregion

        


    }
}