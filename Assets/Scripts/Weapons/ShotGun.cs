using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ShotGun : Gun
    {
        [Header("Shotgun Properties")] 
        [SerializeField] private int numberOfPellets;
        [SerializeField] private float spreadRadius;
        [SerializeField] private AudioClip pumpSound;
        [SerializeField] private AudioClip pelletSound;

        private bool _canShoot;
        private bool _isPumping;
        
        protected override void Awake()
        {
            base.Awake();
            AttackType = AttackType.PumpFire;
        }

        protected override bool CanShoot()
        {
            return !_isPumping;
        }

        protected override void Fire(Transform aimOrigin, bool inputReceived, bool isAiming)        {
            if (inputReceived)
            {
                if (_isReloadin)
                {
                    _isReloadin = false;
                }
                
                
                muzzleFlash.Play();
                _audio.PlayOneShot(fireSound);


                _currAmmo--;

                RaycastHit hit;

                for (int i = 0; i <= numberOfPellets; i++)
                {
                    if (Physics.Raycast(aimOrigin.position, SpreadProjectileDirection(aimOrigin, spreadRadius), out hit, range,
                        ~unShootable))
                    {
                        BulletImpact(hit, true);
                    }
                }

                StartCoroutine(Pump());
                EmitSound(audioIntensity, transform.position);
            }
        }

        private IEnumerator Pump()
        {
            _isPumping = true;
            Anim.SetBool("isPumping", _isPumping);
            _audio.PlayOneShot(pumpSound);
            yield return new WaitForSeconds(Anim.GetCurrentAnimatorClipInfo(0).Length+0.01f);
            Anim.SetBool("isPumping", false);
            _isPumping = false;
        }

        protected override bool CanReload()
        {
            return _ammoReserve.GetAmmo(ammoType) > 0 && _currAmmo < maxAmmo;
            
        }

        protected override IEnumerator Reload()
        {
            _isReloadin = true;
            _anim.SetBool("isReloadin", _isReloadin);

            while (_currAmmo < maxAmmo && _ammoReserve.GetAmmo(ammoType) > 0)
            {
                if (!_isReloadin)
                {
                    _anim.SetBool("isReloadin", false);
                    yield break;
                }
                _audio.PlayOneShot(pelletSound);
                yield return new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(3).Length);
                _currAmmo++;
                _ammoReserve.ComplementAmmo(ammoType, -1);


            }

            _isReloadin = false;
            _anim.SetBool("isReloadin", _isReloadin);

            StartCoroutine(Pump());

        }
    }
}