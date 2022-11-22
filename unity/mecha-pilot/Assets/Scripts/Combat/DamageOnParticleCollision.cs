using UnityEngine;

namespace Combat
{
    public class DamageOnParticleCollision : MonoBehaviour
    {
        public float particleDamage = 3f;
        private void OnParticleCollision(GameObject other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.Damage(particleDamage);
        }
    }
}
