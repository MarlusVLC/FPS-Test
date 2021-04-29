using System;
using System.Collections;
using DefaultNamespace;
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
        
        [Header("Bullet Spreading System - Aiming")]
        [SerializeField][Range(0f,0.1f)] private float initialSpreadFactor_aim = 0.05f;
        [SerializeField][Range(0f,0.1f)] private float progressiveSpreadingFactor_aim = 0.05f;
        [SerializeField] [Range(0.1f, 1.0f)] private float maximumSpreadFactor_aim = 0.3f;

        [Header("Reloading System")]
        [SerializeField] int maxAmmo = 30;
        [SerializeField] private int maxGuardedAmmo = 120;
        [SerializeField] float reloadTime = 3f;
        [SerializeField] private AudioClip reloadSound;
        
        [Header("External Tools")]
        [SerializeField] Camera fpsCam;
        [SerializeField] ParticleSystem impactEffect;
        // [SerializeField] Animator anim;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] private TextMeshProUGUI currAmmoUI;
        [SerializeField] private LayerMask collectables;
       

        private float _nextTimeToFire = 0f;
        private int _currAmmo; 
        private int _guardedAmmo;
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
            fpsCam = Camera.main;

            // muzzleFlash = transform.GetChild(0).GetComponent<ParticleSystem>();
            
            _guardedAmmo = maxGuardedAmmo; //Tirar depois
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
            // anim.SetBool("isReloadin", false);
            UpdateUI();
        }

        private void Update()
        {
            _anim.SetBool("isFiring", Input.GetButton("Fire1") && !_isReloadin && _currAmmo > 0);
            _recoil.aim = FPControl.IsAiming;

            if (_isReloadin)
                return;
            
            if (_guardedAmmo > 0 && _currAmmo < maxAmmo)
                if (_currAmmo <= 0f || Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine(Reload());
                    return;
                }
            
            

            if (Input.GetButton("Fire1"))
            {
                if (Time.time >= _nextTimeToFire && _currAmmo > 0)
                {
                    SetSpreading(FPControl.IsAiming);
                    
                    
                    _nextTimeToFire = Time.time + 1f / fireRate;
                    
                    _currSpreadFactor += progressiveSpreadingFactor;
                    _currSpreadFactor = Mathf.Clamp(_currSpreadFactor, initialSpreadFactor, maximumSpreadFactor);
                    
                    // print("Spread: " + _currSpreadFactor);
                    
                    _recoil.Fire();
                    _audio.PlayOneShot(fireSound);
                    
                    Shoot();
                }
                return;
            }

            _currSpreadFactor = initialSpreadFactor;
            // print("Spread: " + _currSpreadFactor);


            
        }

        void Shoot()
        {
            muzzleFlash.Play();

            _currAmmo--;
            UpdateUI();
            
            Vector3 shootDirection = fpsCam.transform.forward;


            Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            spread.Normalize();
            float multiplier = Random.Range(0f, _currSpreadFactor);
            spread *= multiplier;
            shootDirection += fpsCam.transform.TransformDirection(spread);
            
            
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, range, 
                ~collectables))
            {
                // Debug.Log(hit.transform.name);

                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                
                ParticleSystem impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                impactGO.transform.parent = hit.transform;
            }
        }



        void UpdateUI()
        {
            currAmmoUI.text = _currAmmo + "/" + _guardedAmmo;
        }

        IEnumerator Reload()
        {
            _isReloadin = true;

            _anim.SetBool("isReloadin", true);
            _audio.PlayOneShot(reloadSound);
            
            yield return new WaitForSeconds(reloadTime - .25f);
            _anim.SetBool("isReloadin", false);
            yield return new WaitForSeconds(.25f);

            var missingAmmo = maxAmmo - _currAmmo;
            _currAmmo += Mathf.Clamp(missingAmmo, 0, _guardedAmmo);
            _guardedAmmo = Mathf.Clamp(_guardedAmmo - missingAmmo, 0, int.MaxValue);

            UpdateUI();
            _isReloadin = false;
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

        public void AddAmmo(int newAmmo)
        {
            _guardedAmmo += newAmmo;
            UpdateUI();
        }
        


    }
}