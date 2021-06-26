using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.States;
using DefaultNamespace;
using Entities;
using Players;
using Scripts.NewPlayerControls;
using TMPro;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoCache, ISoundEmitter
    {

        // [SerializeField] protected Transform _head;
        [SerializeField] protected bool DEBUGaudioDistance;
        [SerializeField] protected float audioIntensity;
        [SerializeField] protected LayerMask listenersMask;
        
        public AttackType AttackType { get; protected set; }
        public event Action<float, Vector3> SoundEmitted;
        public event Action AttackExecuted;
        
        protected Animator _anim;
        protected Collider[] _listeners;
        protected float _audioMaxDist;
        protected float _audioAlertDist;
        
        

        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
            _listeners = new Collider[10];
        }

        protected void OnValidate()
        {
            _audioMaxDist = Mathf.Sqrt(audioIntensity / 0.1f);
            _audioAlertDist = Mathf.Sqrt(audioIntensity / 1.2f);
        }
        
        protected void OnAttackExecution()
        {
            AttackExecuted?.Invoke();
        }
        
        public void OnSoundEmitted(float audioScale, Vector3 sourcePosition)
        {
            // SoundEmitted?.Invoke(audioScale, sourcePosition);
        }
        
        
        
        

        // protected void EmitSound(float intensity, Vector3 sourcePosition)
        // {
        //     var maxDistance = Mathf.Sqrt(audioIntensity / 0.1f);
        //     _listeners = Physics.OverlapSphere(sourcePosition, maxDistance, listenersMask);
        //     if (_listeners.Length > 0)
        //     {
        //         Array.ForEach(_listeners,sb => sb.GetComponent<SensorialBeing>()?.ProcessSound(intensity, sourcePosition));
        //     }
        // }
        
        protected void EmitSound(float intensity, Vector3 sourcePosition)
        {
            var maxDistance = Mathf.Sqrt(intensity / 0.1f);
            var numberOfListeners =
                Physics.OverlapSphereNonAlloc(sourcePosition, maxDistance, _listeners, listenersMask);
 
            // Array.ForEach(_listeners.Take(numberOfListeners).ToArray(), ReceiveCollider);
            
            // foreach (var collider_ in _listeners.Take(numberOfListeners))
            // {
            //     collider_.GetComponent<SensorialBeing>()?.ProcessSound(intensity, sourcePosition);
            // }

            for (var i = 0; i < numberOfListeners; i++)
            {
                _listeners[i].GetComponent<SensorialBeing>()?.ProcessSound(intensity, sourcePosition);
            }
        }


        protected void ReceiveCollider(Collider _collider)
        {
            
        }
        
        
        
        
        
        
        public Animator Anim
        {
            get => _anim;
            set { _anim = value; }
        }

        public abstract bool CanAttack();

        public abstract bool CanExecSpecialAction0();

        public abstract void Attack(Transform eyeOrigin, bool inputReceived, bool changingCondition = false);

        public abstract void ExecSpecialAction0();

        public abstract void ToggleSpecialCondition0();


        protected virtual void OnDrawGizmos()
        {
            if (DEBUGaudioDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _audioAlertDist);
                Gizmos.DrawWireSphere(transform.position, _audioMaxDist);
            }
        }
    }
}