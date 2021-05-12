using System;
using Players;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(AmmoReserve))]
    public class GrenadeThrower : MonoBehaviour
    {
        [SerializeField] float throwForce = 20f;
        [SerializeField] private AudioClip throwSound;
        [SerializeField] GameObject grenadePrefab;

        [Header("ExternalAssets")] 
        [SerializeField] private GameObject fpsCam;
        
        private AmmoReserve _ammoReserve;
        private AudioSource _audio;
        
        

        private void Start()
        {
            _ammoReserve = GetComponent<AmmoReserve>();
            _audio = GetComponentInParent<AudioSource>();
        }
        
        
        
        private void ThrowGrenade()
        {
            _audio.PlayOneShot(throwSound);
            GameObject grenade = Instantiate(grenadePrefab, fpsCam.transform.position, fpsCam.transform.rotation);
            grenade.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * throwForce, ForceMode.Impulse);
        }

        private void DecrementAmmo()
        {
            _ammoReserve.Grenades--;
        }

        public void GrenadeThrowProcess()
        {
            if (_ammoReserve.Grenades > 0)
            {
                ThrowGrenade();
                DecrementAmmo();
            }
        }
    }
}