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

    public class Projectile : MonoBehaviour, IProduceImpact
    {
        public Vector3 initialSpeedVector = Vector3.zero;
        public Vector3 firingDirectionNormalized = Vector3.zero;
        public float speedInSeconds = 10f;
        public float projectileDamage = 10f;
        private void Update()
        {
            if (firingDirectionNormalized == Vector3.zero) gameObject.SetActive(false);
            transform.Translate(initialSpeedVector * Time.deltaTime +
                                firingDirectionNormalized * (speedInSeconds * Time.deltaTime));
        }
        private void OnCollisionEnter(Collision collision)
        {
            // deactivate on collision
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {

                Debug.Log("Collided with something damageable");
                damageable.Damage(projectileDamage);
                ImpactHasOccurred?.Invoke(new Impact { Impacter = gameObject, Impactee = collision.gameObject });
                gameObject.SetActive(false);
            }
        }

        public event Action<Impact> ImpactHasOccurred;
    }

}
