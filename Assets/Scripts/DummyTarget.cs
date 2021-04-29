using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UI;
using UnityEngine;

namespace Weapons
{
    public class DummyTarget : MonoBehaviour
    {

        [SerializeField] private AudioClip raiseSound;
        
        
        private bool _dead;

        private AudioSource _audio;
        private HingeJoint _hinge; 
        void Start()
        {
            _hinge = GetComponent<HingeJoint>();
            _hinge.useSpring = false;
            _audio = GetComponent<AudioSource>();
            GameManager.GetInstance.AddTarget(this);

        }
        

        private void Update()
        {
            if (transform.localRotation.eulerAngles.x < 10 && !_dead)
            {
                _dead = true;
            }

        }

        public IEnumerator Raise()
        {
            if (transform.localRotation.eulerAngles.x < 10)
                _hinge.useSpring = true;
            _audio.PlayOneShot(raiseSound);
            yield return new WaitUntil(() => transform.rotation.eulerAngles.x >= 89f);
            _dead = false;

            _hinge.useSpring = false;
        }

        public bool IsDead()
        {
            return _dead;
        }




    }
}