using System;
using System.Collections;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    // [RequireComponent(typeof(RecoilEffector))]
    [RequireComponent(typeof(AudioSource))]
    public class AutoGun : Gun
    {
        [Header("Bullet Spreading System - Aimless")] [SerializeField] [Range(0f, 0.1f)]
        private float initialSpreadFactor_hipfire = 0.05f;

        [SerializeField] [Range(0f, 0.1f)] private float progressiveSpreadingFactor_hipfire = 0.05f;
        [SerializeField] [Range(0.1f, 1.0f)] private float maximumSpreadFactor_hipfire = 0.3f;

        //O Spread deve ser reduzido pelo jogador
        [Header("Bullet Spreading System - Aiming")] [SerializeField] [Range(0f, 0.1f)]
        private float initialSpreadFactor_aim = 0.05f;

        [SerializeField] [Range(0f, 0.1f)] private float progressiveSpreadingFactor_aim = 0.05f;
        [SerializeField] [Range(0.1f, 1.0f)] private float maximumSpreadFactor_aim = 0.3f;


        private float _nextTimeToFire;
        private float _currSpreadFactor;

        private float _initialSpreadFactor;
        private float _progressiveSpreadingFactor;
        private float _maximumSpreadFactor;

        private bool _isFiring;


        protected override void Awake()
        {
            base.Awake();
            AttackType = AttackType.AutoFire;
            _currSpreadFactor = _initialSpreadFactor;
        }








        protected override void Fire(Transform aimOrigin, bool inputReceived, bool isAiming)
        {
            if (inputReceived)
            {
                if (Time.time >= _nextTimeToFire && _currAmmo > 0)
                {
                    SetSpreading(isAiming);


                    _nextTimeToFire = Time.time + 1f / fireRate;

                    _currSpreadFactor += _progressiveSpreadingFactor;
                    _currSpreadFactor = Mathf.Clamp(_currSpreadFactor, _initialSpreadFactor, _maximumSpreadFactor);

                    if (_recoil)
                    {
                        _recoil.AddRecoil(isAiming);
                    }
                    _audio.PlayOneShot(fireSound);
  
                    Shoot(aimOrigin);
                }
                return;
            }

            ResetSpreadFactor();
        }

        void Shoot(Transform shotOrigin)
        {
            muzzleFlash.Play();

            _currAmmo--;

            RaycastHit hit;
            if (Physics.Raycast(shotOrigin.position, SpreadProjectileDirection(shotOrigin, _currSpreadFactor), out hit, range,
                ~unShootable))
            {
                BulletImpact(hit);
            }
        }
        
        
        
        private void SetSpreading(bool isAiming)
        {
            if (isAiming)
            {
                _initialSpreadFactor = initialSpreadFactor_aim;
                _maximumSpreadFactor = maximumSpreadFactor_aim;
                _progressiveSpreadingFactor = progressiveSpreadingFactor_aim;
                return;
            }

            _initialSpreadFactor = initialSpreadFactor_hipfire;
            _maximumSpreadFactor = maximumSpreadFactor_hipfire;
            _progressiveSpreadingFactor = progressiveSpreadingFactor_hipfire;
        }

        public void ResetSpreadFactor()
        {
            _currSpreadFactor = _initialSpreadFactor;
        }
        
        
        
        

        protected override IEnumerator Reload()
        {
            _isReloadin = true;

            _anim.SetBool("isReloadin", true);
            _audio.PlayOneShot(reloadSound);

            yield return new WaitForSeconds(reloadTime - .25f);
            _anim.SetBool("isReloadin", false);
            yield return new WaitForSeconds(.25f);

            var missingAmmo = maxAmmo - _currAmmo;
            _currAmmo += Mathf.Clamp(missingAmmo, 0, _ammoReserve.GetAmmo(ammoType));
            _ammoReserve.ClampAmmo(ammoType, missingAmmo);

            _isReloadin = false;
        }

        protected override bool CanReload()
        {
            return _ammoReserve.GetAmmo(ammoType) > 0 && _currAmmo < maxAmmo;
        }
        
       
        
    }
}