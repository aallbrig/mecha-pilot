using System;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable, ICanDie
    {
        public float maxHealth = 10f;
        public UnityEvent onDamage;
        public UnityEvent onDied;
        private float _currentHealth;
        private bool _dead;
        private void Start() => _currentHealth = maxHealth;
        private void OnEnable()
        {
            TimeOfDeath = 0;
            _dead = false;
        }

        public void OnCollisionEnter() => Damage(maxHealth);

        public float TimeOfDeath { get; private set; }

        public event Action<GameObject> Died;

        public void Damage(float amount)
        {
            if (_dead) return;

            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, maxHealth);
            Damaged?.Invoke(new DamageResult(amount, gameObject));
            onDamage?.Invoke();

            if (_currentHealth <= 0)
            {
                _dead = true;
                TimeOfDeath = Time.time;
                Died?.Invoke(gameObject);
                onDied?.Invoke();
            }
        }

        public event Action<DamageResult> Damaged;
    }
}
