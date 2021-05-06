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
        [SerializeField] protected AmmoReserve _ammoReserve;
        [SerializeField] protected ParticleSystem impactEffect;
        [SerializeField] protected ParticleSystem muzzleFlash;



        protected abstract bool CanShoot();

        protected abstract void Fire(bool isAiming);
        
        public override bool CanAttack()
        {
            return CanShoot();
        }

        public override void Attack(bool changingCondition = false)
        {
            Fire(changingCondition);
        }
        
        public override 
        
        
        
        
        
    }
}