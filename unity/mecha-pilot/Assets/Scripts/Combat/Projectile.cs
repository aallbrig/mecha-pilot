using System;
using UnityEngine;

namespace Combat
{
    public interface IImpact
    {
        public GameObject Impacter { get; set; }

        public GameObject Impactee { get; set; }
    }

    public class Impact : IImpact
    {
        public GameObject Impacter { get; set; }

        public GameObject Impactee { get; set; }
    }

    public interface IProduceImpact
    {
        public event Action<Impact> ImpactHasOccurred;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IProduceImpact
    {
        public Vector3 firingDirectionNormalized = Vector3.zero;
        public float speedInSeconds = 10f;
        public float projectileDamage = 10f;
        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody ??= GetComponent<Rigidbody>();
        private void OnEnable()
        {
            if (firingDirectionNormalized == Vector3.zero) gameObject.SetActive(false);
            _rigidbody.velocity = firingDirectionNormalized * speedInSeconds;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {

                damageable.Damage(projectileDamage);
                ImpactHasOccurred?.Invoke(new Impact { Impacter = gameObject, Impactee = collision.gameObject });
                gameObject.SetActive(false);
            }
        }

        public event Action<Impact> ImpactHasOccurred;
    }

}
