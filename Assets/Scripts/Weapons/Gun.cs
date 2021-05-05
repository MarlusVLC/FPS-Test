using System;
using System.Collections;
using DefaultNamespace;
using Players;
using Scripts.NewPlayerControls;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    [RequireComponent(typeof(New_Weapon_Recoil_Script))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(KeepTransform))]

    
    public class Gun : Weapon
    {
        [Header("Firing System")]
        [SerializeField] float damage = 10f;
        [SerializeField] float range = 100f;
        [SerializeField] float impactForce = 30f;
        [SerializeField] float fireRate = 15f;
        [SerializeField] private AudioClip fireSound;
        
        [Header("Bullet Spreading System - Aimless")]
        [SerializeField][Range(0f,0.1f)] private float initialSpreadFactor_hipfire = 0.05f;
        [SerializeField][Range(0f,0.1f)] private float progressiveSpreadingFactor_hipfire = 0.05f;
        [SerializeField] [Range(0.1f, 1.0f)] private float maximumSpreadFactor_hipfire = 0.3f;
        
        //O Spread deve ser reduzido pelo jogador
        [Header("Bullet Spreading System - Aiming")]
        [SerializeField][Range(0f,0.1f)] private float initialSpreadFactor_aim = 0.05f;
        [SerializeField][Range(0f,0.1f)] private float progressiveSpreadingFactor_aim = 0.05f;
        [SerializeField] [Range(0.1f, 1.0f)] private float maximumSpreadFactor_aim = 0.3f;

        [Header("Reloading System")]
        [SerializeField] private AmmoType ammoType;
        [SerializeField] int maxAmmo = 30;
        [SerializeField] float reloadTime = 3f;
        [SerializeField] private AudioClip reloadSound;
        
        [Header("External Tools")]
        [SerializeField] Camera fpsCam;
        [SerializeField] private AmmoReserve _ammoReserve;
        [SerializeField] ParticleSystem impactEffect;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] private TextMeshProUGUI currAmmoUI;
        [SerializeField] private LayerMask collectables;


        private float _nextTimeToFire;
        private int _currAmmo; 
        private float _currSpreadFactor;
        private bool _isReloadin;
        private New_Weapon_Recoil_Script _recoil;
        private AudioSource _audio;

        private float initialSpreadFactor;
        private float progressiveSpreadingFactor ;
        private float maximumSpreadFactor;


        private void Start()
        {
            _recoil = GetComponent<New_Weapon_Recoil_Script>();
            _audio = GetComponent<AudioSource>();
            _ammoReserve = GetComponentInParent<AmmoReserve>();
            fpsCam = Camera.main;
            
            _currAmmo = maxAmmo;
            _currSpreadFactor = initialSpreadFactor;

            UpdateUI();
        }
        
        
        
        
        

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        
        
        
        
        

        private void OnEnable()
        {
            _isReloadin = false;
            UpdateUI();
        }
        
        
        
        
        

        private void Update()
        {
            _anim.SetBool("isFiring", CanShoot() && FireInputReceived());

            if (_isReloadin)
                return;
            
            if (CanReload())
                if (isOutOfAmmo() || ReloadInputReceived())
                {
                    StartCoroutine(Reload());
                    return;
                }
            
            
            //Esse chamado deve ser responsdabilidade do jogador
            if (FireInputReceived())
            {
                Fire();
                return;
            }

            ResetSpreadFactor();
        }




        bool FireInputReceived()
        {
            return FirstPerson_InputHandler.PrimaryFireKey_Auto;
        }

        bool CanShoot()
        {
            return !_isReloadin && _currAmmo > 0;
        }

        bool isOutOfAmmo()
        {
           return _currAmmo <= 0f;
        }

        void Shoot()
        {
            muzzleFlash.Play();

            _currAmmo--;
            UpdateUI(); //Responsa da UI
            
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, SpreadBulletDirection(), out hit, range, 
                ~collectables))
            {
                // Debug.Log(hit.transform.name);
                BulletImpact(hit);
            }
        }

        //Tornar responsabilidade da UI e usar DELEGATES
        void UpdateUI()
        {
            if (_ammoReserve == null)
            {
                _ammoReserve = GetComponentInParent<AmmoReserve>();
            } 
            currAmmoUI.text = _currAmmo + "/" + _ammoReserve.GetAmmo(ammoType);
        }
        
        //Recarregar a arma deve ser responsabilidade do jogdor
        IEnumerator Reload()
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

            UpdateUI();
            _isReloadin = false;
        }

        bool CanReload()
        {
            return _ammoReserve.GetAmmo(ammoType) > 0 && _currAmmo < maxAmmo;
        }

        bool ReloadInputReceived()
        {
            return FirstPerson_InputHandler.ReloadKey;
        }
        private void SetSpreading(bool isAiming)
        {
            if (isAiming)
            {
                initialSpreadFactor = initialSpreadFactor_aim;
                maximumSpreadFactor = maximumSpreadFactor_aim;
                progressiveSpreadingFactor = progressiveSpreadingFactor_aim;
                return;
            }
            initialSpreadFactor = initialSpreadFactor_hipfire;
            maximumSpreadFactor = maximumSpreadFactor_hipfire;
            progressiveSpreadingFactor = progressiveSpreadingFactor_hipfire;
            
        }

        private void Fire()
        {
            if (Time.time >= _nextTimeToFire && _currAmmo > 0)
            {
                SetSpreading(FPControl.IsAiming);
                    
                    
                _nextTimeToFire = Time.time + 1f / fireRate;
                    
                _currSpreadFactor += progressiveSpreadingFactor;
                _currSpreadFactor = Mathf.Clamp(_currSpreadFactor, initialSpreadFactor, maximumSpreadFactor);
                
                _recoil.AddRecoil(FPControl.IsAiming);
                _audio.PlayOneShot(fireSound);
                    
                Shoot();
            }
        }
        
        private void BulletImpact(RaycastHit hitObject)
        {
            Target target = hitObject.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitObject.rigidbody != null)
            {
                hitObject.rigidbody.AddForce(-hitObject.normal * impactForce);
            }
                
            ParticleSystem impactGO = Instantiate(impactEffect, hitObject.point, Quaternion.LookRotation(hitObject.normal));
            impactGO.transform.parent = hitObject.transform;
        }
        
        private Vector3 SpreadBulletDirection()
        {
            Vector3 shootDirection = fpsCam.transform.forward;


            Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            spread.Normalize();
            float multiplier = Random.Range(0f, _currSpreadFactor);
            spread *= multiplier;
            shootDirection += fpsCam.transform.TransformDirection(spread);

            return shootDirection;
        }

        private void ResetSpreadFactor()
        {
            _currSpreadFactor = initialSpreadFactor;
        }

        
        


        


    }
}