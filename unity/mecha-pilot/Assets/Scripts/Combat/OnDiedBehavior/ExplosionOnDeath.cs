using UnityEngine;
using UnityEngine.Pool;

namespace Combat.OnDiedBehavior
{
    public class ExplosionOnDeath : MonoBehaviour
    {
        public GameObject explosionPrefab;
        private GameObject _explosionInstance;
        public IObjectPool<GameObject> _particleSystemPool;

        private void Awake() => _particleSystemPool =
            new ObjectPool<GameObject>(PoolCreate, PoolGet, PoolRelease, PoolDestroy, true, 10, 1000);

        private void Start()
        {
            if (TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += ExplodeOnDeath;
            else
                enabled = false;
        }
        private void PoolDestroy(GameObject obj) => Destroy(obj);
        private void PoolRelease(GameObject obj) => obj.SetActive(false);
        private void PoolGet(GameObject obj) => obj.SetActive(true);
        private GameObject PoolCreate()
        {
            var explosionInstance = Instantiate(explosionPrefab);
            explosionInstance.SetActive(false);
            return explosionInstance;
        }
        private void ExplodeOnDeath(GameObject deadGameObject)
        {
            if (explosionPrefab == null) return;
            var explosion = _particleSystemPool.Get();
            explosion.transform.position = deadGameObject.transform.position;
        }
        private GameObject GetExplosion() => _explosionInstance;
    }
}
