using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class ExplosionOnDeath : MonoBehaviour
    {
        public GameObject explosionPrefab;
        private GameObject _explosionInstance;
        private void Start()
        {
            if (TryGetComponent<ICanDie>(out var ableToDie))
            {
                _explosionInstance = Instantiate(explosionPrefab);
                _explosionInstance.SetActive(false);
                ableToDie.Died += ExplodeOnDeath;
            }
            else
            {
                enabled = false;
            }
        }
        private void ExplodeOnDeath(GameObject deadGameObject)
        {
            if (explosionPrefab == null) return;
            var explosion = GetExplosion();
            explosion.SetActive(false);
            explosion.transform.position = deadGameObject.transform.position;
            explosion.SetActive(true);
        }
        private GameObject GetExplosion() => _explosionInstance;
    }
}
