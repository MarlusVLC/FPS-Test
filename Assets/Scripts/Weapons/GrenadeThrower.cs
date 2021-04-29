using System;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(AmmoReserve))]
    public class GrenadeThrower : MonoBehaviour
    {
        [SerializeField] float throwForce = 20f;
        [SerializeField] GameObject grenadePrefab;

        [Header("ExternalAssets")] 
        [SerializeField] private GameObject fpsCam;


        private AmmoReserve _ammoReserve;

        private void Start()
        {
            _ammoReserve = GetComponent<AmmoReserve>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(2) && _ammoReserve.Grenades > 0)
            {
                ThrowGrenade();
                _ammoReserve.Grenades--;
            }
        }

        private void ThrowGrenade()
        {
            GameObject grenade = Instantiate(grenadePrefab, fpsCam.transform.position, fpsCam.transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(fpsCam.transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}