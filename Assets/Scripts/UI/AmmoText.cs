using System;
using System.Linq;
using Auxiliary;
using Players;
using TMPro;
using UnityEngine;
using Weapons;

namespace UI
{
    public class AmmoText : MonoBehaviour
    {
        [SerializeField] WeaponHandler playerWeaponHandler;
        private TextMeshProUGUI statusUI;


        private void Awake()
        {
            statusUI = GetComponent<TextMeshProUGUI>();
        }
        
        private void Start()
        {
            // Gun.AmmoChanged += UpdateText;

            AssignPlayerWeapons();
        }

        private void OnDisable()
        {
            DeAssignPlayerWeapons();
        }
        
        
        
        private void UpdateText(int currAmmo, int reserveAmmo)
        {
            statusUI.text = currAmmo + "/" + reserveAmmo;
        }


        private void AssignPlayerWeapons()
        {
            // foreach (Gun weapon in playerWeaponHandler.HeldWeapons)
            // {
            //     weapon.AmmoChanged += UpdateText;
            // }

            foreach (var weapon in playerWeaponHandler.HeldWeapons)
            {
                if (weapon is Gun gun)
                {
                    //TODO verificar conflitos
                    // weapon.AmmoChanged += UpdateText;
                }
            }
            
            playerWeaponHandler.HeldWeapons.Where(w => w is Gun ).
            ForEach(g  => (g as Gun).
            AmmoChanged += UpdateText);
        }
        
        private void DeAssignPlayerWeapons()
        {
            // foreach (Gun weapon in playerWeaponHandler.HeldWeapons)
            // {
            //     weapon.AmmoChanged -= UpdateText;
            // }
            playerWeaponHandler.HeldWeapons.Where(w => w is Gun).ForEach(g  => (g as Gun).AmmoChanged -= UpdateText);

        }
        
        
    }
}