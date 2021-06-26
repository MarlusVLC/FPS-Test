using System;
using UnityEngine;

namespace Entities
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 50;

        private float _currentHealth;
        private Destructible _destructible;
        
        public event Action<float> OnHealthPctChanged = delegate {  };  

        private void Start()
        {
            _currentHealth = maxHealth;
            _destructible = GetComponent<Destructible>();
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;

            var currentHealthPct = _currentHealth / maxHealth;
            OnHealthPctChanged(currentHealthPct);

            if (_currentHealth <= 0)
            {
                if (_destructible)
                {
                    _destructible.Die();
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
        }


    }
}