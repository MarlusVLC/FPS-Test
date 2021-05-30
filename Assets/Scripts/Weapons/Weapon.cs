using System;
using System.Collections;
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
    public abstract class Weapon : MonoBehaviour, ISoundEmitter
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
        
        

        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
            // _listeners = new Collider[10];
        }

        protected void OnValidate()
        {
            _audioMaxDist = Mathf.Sqrt(audioIntensity / 0.1f);
        }
        
        protected virtual void OnDrawGizmos()
        {
            if (DEBUGaudioDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _audioMaxDist);
            }
        }


        protected void OnAttackExecution()
        {
            AttackExecuted?.Invoke();
        }
        
        public void OnSoundEmitted(float audioScale, Vector3 sourcePosition)
        {
            // SoundEmitted?.Invoke(audioScale, sourcePosition);
        }

        protected void EmitSound(float intensity, Vector3 sourcePosition)
        {
            // _listeners = new Collider[10];
            SensorialBeing sb;
            float maxDistance = Mathf.Sqrt(audioIntensity / 0.1f);
            _listeners = Physics.OverlapSphere(sourcePosition, maxDistance, listenersMask);
            if (_listeners.Length > 0){
            // if (Physics.OverlapSphereNonAlloc(sourcePosition, maxDistance, _listeners, listenersMask) > 0)
            // {
                int i = 1;
                foreach (Collider listener in _listeners)
                {
                    // print($"Quem escutou #{i}: {listener.gameObject.name} ");
                    i++;
                    sb = listener.GetComponent<SensorialBeing>();
                    if (sb)
                    {
                        sb.ProcessSound(intensity, sourcePosition);
                    }
                }
            }
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


    }
}