using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace DefaultNamespace
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private List<DummyTarget> _targets = new List<DummyTarget>();
        
        
        private static TargetManager INSTANCE;

        public static TargetManager GetInstance
        {
            get => INSTANCE;
        }
        
        

        private void Awake()
        {
            //SINGLETON check
            if (INSTANCE != null && INSTANCE != this)
            {
                Destroy(gameObject);
            }
            //SINGLETON instantiate
            else
            {
                INSTANCE = this;
            }
        }





        public void AddTarget(DummyTarget target)
        {
            if (_targets.Contains(target))
                return;
            _targets.Add(target);
        }

        public void RaiseTargets()
        {
            foreach (DummyTarget target in _targets)
            {
                if (target.IsDead())
                    StartCoroutine(target.Raise());
            }
        }
        
        
        
    }
}