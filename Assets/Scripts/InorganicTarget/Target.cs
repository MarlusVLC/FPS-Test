using System;
using UnityEngine;

namespace Weapons
{
    public class Target : MonoBehaviour
    {
        public float health = 50f;

        private Destructible _destructible;

        private void Start()
        {
            _destructible = GetComponent<Destructible>();
        }

        public void TakeDamage(float amount)
        {
            health -= amount;

            if (health <= 0f)
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