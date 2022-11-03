using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class ExplosionOnDeath : MonoBehaviour
    {
        public GameObject explosionPrefab;
        private void Start()
        {
            if (TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += ExplodeOnDeath;
            else enabled = false;
        }
        private void ExplodeOnDeath(GameObject deadGameObject)
        {
            if (explosionPrefab == null) return;
            var explosion = GetExplosion();
            explosion.transform.position = deadGameObject.transform.position;
        }
        private GameObject GetExplosion() => Instantiate(explosionPrefab);
    }
}
