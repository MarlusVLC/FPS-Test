using System;
using System.Net;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : Weapon
    {
        
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
        protected AmmoReserve _ammoReserve;
        public static event Action<int, int> AmmoChanged;


        protected override void Start()
        {
            base.Start();
        }

        protected virtual void Awake()
        {
            if (ReferenceEquals(_ammoReserve, null))
            {
                _ammoReserve = GetComponentInParent<AmmoReserve>();
            }        
        }
        
        protected override void OnEnable()
        {
            print("sassssasasa");

            SetPreviousAmmo();
            base.OnEnable();

            OnAmmoChanged(_currAmmo, _ammoReserve.GetAmmo(ammoType));
        }
        
        protected virtual void Update()
        {
            if (HasAmmoChanged())
            {
                OnAmmoChanged(_currAmmo, _ammoReserve.GetAmmo(ammoType));
                SetPreviousAmmo();
            }
                
        }

        

        protected abstract bool CanShoot();
        
        protected abstract void Fire(bool inputReceived, bool isAiming);
        
        public override bool CanAttack()
        {
            return CanShoot();
        }
        
        public override void Attack(bool inputReceived, bool stoppingCondition = false)
        {
            Fire(inputReceived, stoppingCondition);
        }
        
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
    }
}