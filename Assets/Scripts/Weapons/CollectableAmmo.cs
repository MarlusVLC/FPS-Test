﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BoxCollider))]

    
    public class CollectableAmmo : MonoBehaviour
    {
        [SerializeField] private AmmoType ammoType;
        [SerializeField] int guardedAmmo;
        [SerializeField] private AudioClip collectingSound;

        private AudioSource _audio;
        private BoxCollider _collider;
        
        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _collider = GetComponent<BoxCollider>();
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AmmoReserve ammoReserve = other.GetComponentInChildren<AmmoReserve>();

                StartCoroutine(CollectionEndCycle(collectingSound, ammoReserve));
            }
        }


        private IEnumerator CollectionEndCycle(AudioClip sound, AmmoReserve ammoReserve)
        {
            _audio.PlayOneShot(collectingSound);
            ammoReserve.ComplementAmmo(ammoType, guardedAmmo);
            transform.localScale = Vector3.zero;
            _collider.enabled = false;
            yield return new WaitForSeconds(sound.length);
            Destroy(gameObject);
        }
    }
}