using System;
using UnityEngine;

namespace Scripts
{
    public class Enemy : MonoBehaviour
    {
        private void Start()
        {
            PlayerA.OnEnemyHit += Damage;
        }

        void Damage(Color color)
        {
            transform.GetComponent<Renderer>().material.color = color;
        }

        private void OnDisable()
        {
            PlayerA.OnEnemyHit -= Damage;
        }
    }
}