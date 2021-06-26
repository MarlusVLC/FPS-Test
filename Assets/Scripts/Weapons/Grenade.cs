using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

public class Grenade : MonoCache
{

    [SerializeField] private float delay = 5f;
    [SerializeField] private float explosionRadius = 5f; 
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float damage = 25;
    [SerializeField] private LayerMask explodablesMask;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip hitSound;
    
    float _countdown;
    private bool _hasExploded;
    private AudioSource _audio;
    private Collider[] _colliders;


    protected override void Awake()
    {
        base.Awake();
        _audio = GetComponent<AudioSource>();
        _colliders = new Collider[10];
    }

    void Start()
    {
        _countdown = delay;
    }

    void Update()
    {
        _countdown -= Time.deltaTime;
        if (_countdown <= 0f && !_hasExploded)
        {
            StartCoroutine(Explode());
            _hasExploded = true;
        }
    }


    private IEnumerator Explode()
    {
        if (explosionSound)
            _audio.PlayOneShot(explosionSound);
        Instantiate(explosionEffect, Transform.position, Transform.rotation);
        
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(0.1f);
        
        var numberOfTargets = Physics.OverlapSphereNonAlloc(Transform.position, explosionRadius, _colliders, explodablesMask);

        for (int i = 0; i < numberOfTargets; i++)
        {
            _colliders[i].GetComponent<Rigidbody>()?.AddExplosionForce(explosionForce, Transform.position, explosionRadius);
           _colliders[i].GetComponent<Health>()?.TakeDamage(damage);
        }
        if (explosionSound)
        {
            while (_audio.isPlaying)
            {
                yield return null;
            }
        }

        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (hitSound && !other.gameObject.CompareTag("Player"))
            _audio.PlayOneShot(hitSound);
    }
}
