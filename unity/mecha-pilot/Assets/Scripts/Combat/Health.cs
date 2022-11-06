using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable, ICanDie
    {
        public float maxHealth = 10f;
        private float _currentHealth;
        private bool _dead;
        private void Start() => _currentHealth = maxHealth;
        private void OnEnable() => _dead = false;

        public void OnCollisionEnter() => Damage(maxHealth);

        public event Action<GameObject> Died;

        public void Damage(float amount)
        {
            if (_dead) return;

            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, maxHealth);
            Damaged?.Invoke(new DamageResult(amount, gameObject));

            if (_currentHealth <= 0)
            {
                _dead = true;
                Died?.Invoke(gameObject);
            }
        }

        public event Action<DamageResult> Damaged;
    }
}
