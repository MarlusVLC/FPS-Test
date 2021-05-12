using System;
using TMPro;
using UnityEngine;
using Weapons;

namespace UI
{
    public class AmmoText : MonoBehaviour
    {
        private TextMeshProUGUI statusUI;


        private void Awake()
        {
            statusUI = GetComponent<TextMeshProUGUI>();
        }
        
        private void OnEnable()
        {
            Gun.AmmoChanged += UpdateText;
        }

        private void OnDisable()
        {
            Gun.AmmoChanged -= UpdateText;
        }
        
        
        
        private void UpdateText(int currAmmo, int reserveAmmo)
        {
            statusUI.text = currAmmo + "/" + reserveAmmo;
        }
        
        
    }
}