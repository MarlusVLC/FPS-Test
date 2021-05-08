using TMPro;
using UnityEngine;
using Weapons;

namespace UI
{
    public class AmmoText : MonoBehaviour
    {
        private TextMeshProUGUI statusUI; 
        
        private void Start()
        {
            statusUI = GetComponent<TextMeshProUGUI>();
            Gun.AmmoChanged += UpdateText;
        }


        private void UpdateText(int currAmmo, int reserveAmmo)
        {
            statusUI.text = currAmmo + "/" + reserveAmmo;
        }
    }
}