using System;
using DefaultNamespace;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBarUI : MonoCache

    {
        [SerializeField] private Health playerHealth;

        private Image _healthImage;

        protected override void Awake()
        {
            base.Awake();
            _healthImage = GetComponent<Image>();
        }

        public void Start()
        {
            playerHealth.OnHealthPctChanged += SetFillAmount;
            playerHealth.OnHealthPctChanged += SetColor;
        }

        public void OnDestroy()
        {
            playerHealth.OnHealthPctChanged -= SetFillAmount;
            playerHealth.OnHealthPctChanged -= SetColor;        }

        public void SetFillAmount(float amount)
        {
            _healthImage.fillAmount = amount;

        }

        public void SetColor(float amount)
        {
            if (amount < 0.3f)
                _healthImage.color = Color.red;
            else if (amount < 0.6f)
                _healthImage.color = Color.yellow;
        }
    }
}