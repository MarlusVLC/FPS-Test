using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace DefaultNamespace
{
    public class CollectableAmmo : MonoBehaviour
    {
        [SerializeField] int guardedAmmo;
        [SerializeField] private AudioClip collectingSound;

        private AudioSource _audio;
        private MeshRenderer _renderer;
        private BoxCollider _collider;
        
        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Gun gun = other.GetComponentInChildren<Gun>();

                StartCoroutine(PlaySoundAndDestroy(collectingSound, gun));
            }
        }


        private IEnumerator PlaySoundAndDestroy(AudioClip sound, Gun gun)
        {
            _audio.PlayOneShot(collectingSound);
            gun.AddAmmo(guardedAmmo);
            _renderer.enabled = false;
            _collider.enabled = false;
            yield return new WaitForSeconds(sound.length);
            Destroy(gameObject);
        }
    }
}