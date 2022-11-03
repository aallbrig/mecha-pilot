using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable, ICanDie
    {
        public float maxHealth = 10f;
        private float _currentHealth;
        private void Start() => _currentHealth = maxHealth;

        public void OnCollisionEnter(Collision collision) => Damage(maxHealth);

        public event Action<GameObject> Died;

        public void Damage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, maxHealth);
            Damaged?.Invoke(new DamageResult(amount, gameObject));

            if (_currentHealth <= 0)
                Died?.Invoke(gameObject);
        }

        public event Action<DamageResult> Damaged;
    }
}
