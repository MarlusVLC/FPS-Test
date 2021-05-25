using System;
using System.Collections;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public abstract class Gun : Weapon
    {
        [Header("Firing System")]
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float range = 100f;
        [SerializeField] protected float impactForce = 30f;
        [SerializeField] protected float fireRate = 15f;
        [SerializeField] protected AudioClip fireSound;
        [SerializeField] protected LayerMask unShootable;
        
        [Header("Reloading System")]
        [SerializeField] protected AmmoType ammoType;
        [SerializeField] protected int maxAmmo = 30;
        [SerializeField] protected float reloadTime = 3f;
        [SerializeField] protected AudioClip reloadSound;
        
        [Header("External Tools")]
        [SerializeField] protected ParticleSystem impactEffect;
        [SerializeField] protected ParticleSystem muzzleFlash;
        
        protected int _currAmmo;
        private int _PREVcurrAmmo;
        private int _PREVreserveAmmo;
        protected bool _isReloadin;
        protected AmmoReserve _ammoReserve;
        protected AudioSource _audio;
        protected RecoilEffector _recoil;

        public event Action<int, int> AmmoChanged;
        

        protected override void Awake()
        {
            base.Awake();
            _currAmmo = maxAmmo;
            _recoil = GetComponent<RecoilEffector>();
            _audio = GetComponent<AudioSource>();
            if (ReferenceEquals(_ammoReserve, null))
            {
                _ammoReserve = GetComponentInParent<AmmoReserve>();
            }        
        }
        
        protected virtual void OnEnable()
        {
            OnAmmoChanged(_currAmmo, _ammoReserve.GetAmmo(ammoType));
            SetPreviousAmmo();
            _isReloadin = false;
        }

        protected virtual void Start()
        {
            OnAmmoChanged(_currAmmo, _ammoReserve.GetAmmo(ammoType));
        }

        protected virtual void Update()
        {
            if (HasAmmoChanged())
            {
                OnAmmoChanged(_currAmmo, _ammoReserve.GetAmmo(ammoType));
                SetPreviousAmmo();
            }
            
            if (IsOutOfAmmo() && CanReload())
            {
                StartCoroutine(Reload());
            }
                
        }






        protected virtual bool CanShoot()
        {
            return !_isReloadin && _currAmmo > 0;
        }
        
        protected abstract void Fire(Transform shotOrigin, bool inputReceived, bool isAiming);
        
        
        

        protected abstract bool CanReload();

        protected abstract IEnumerator Reload();
        
        
        

        private bool HasAmmoChanged()
        {
            return _PREVcurrAmmo != _currAmmo || _PREVreserveAmmo != _ammoReserve.GetAmmo(ammoType);
        }
        
        private void SetPreviousAmmo()
        {
            _PREVcurrAmmo = _currAmmo;
            _PREVreserveAmmo = _ammoReserve.GetAmmo(ammoType);
        }
        
        private void OnAmmoChanged(int currAmmo, int reserveAmmo)
        {
            AmmoChanged?.Invoke(currAmmo, reserveAmmo);
        }


        
        private bool IsOutOfAmmo()
        {
            return _currAmmo <= 0f && !_isReloadin;
        }
        
        protected void BulletImpact(RaycastHit hitObject, bool considerDistance = false)
        {
            Target target = hitObject.transform.GetComponent<Target>();
            if (target)
            {
                target.TakeDamage(considerDistance ? DamageOnDistance(hitObject) : damage);
            }

            if (hitObject.rigidbody)
            {
                hitObject.rigidbody.AddForce(-hitObject.normal * impactForce);
            }

            ParticleSystem impactGO =
                Instantiate(impactEffect, hitObject.point, Quaternion.LookRotation(hitObject.normal));
            impactGO.transform.parent = hitObject.transform;
        }
        
        protected Vector3 SpreadProjectileDirection(Transform shotOrigin, float spreadMultiplier)
        {
            Vector3 shootDirection = shotOrigin.forward;


            Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            spread.Normalize();
            float multiplier = Random.Range(0f, spreadMultiplier);
            spread *= multiplier;
            shootDirection += shotOrigin.TransformDirection(spread);

            return shootDirection;
        }


        protected float DamageOnDistance(RaycastHit hit)
        {
            return damage / hit.distance * 10f;
        }
        
        public override bool CanAttack()
        {
            return CanShoot();
        }
        
        public override void Attack(Transform eyeOrigin, bool inputReceived, bool changingCondition = false)
        {
            Fire(eyeOrigin, inputReceived, changingCondition);
        }

        public override bool CanExecSpecialAction0()
        {
            return CanReload();
        }

        public override void ExecSpecialAction0()
        {
            StartCoroutine(Reload());
        }
    }
}