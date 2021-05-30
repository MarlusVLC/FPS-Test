using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float delay = 5f;
    [SerializeField] private float explosionRadius = 5f; 
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip hitSound;
    
    float _countdown;
    private bool _hasExploded;
    private AudioSource _audio;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        _countdown = delay;
    }

    // Update is called once per frame
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
        // Mostrar efeito
        if (explosionSound)
            _audio.PlayOneShot(explosionSound);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        //Fazer desaperecer
        transform.localScale = Vector3.zero;

        //Pegar objetos próximos
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb)
            {
                //Adicionar forca
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest)
            {
                dest.Die();
            }
        }
        
        
        
        
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb)
            {
                //Adicionar forca
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
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
