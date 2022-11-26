using Combat;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay
{
    public class IndicatorManager : MonoBehaviour
    {
        public Transform player;
        public GameObject indicatorPrefab;
        public Vector3 offset = Vector3.zero;
        public float distanceAway = 2f;
        private ObjectPool<GameObject> _indicatorPool;
        private void Start() => _indicatorPool = new ObjectPool<GameObject>(CreatePoolItem, OnGet, OnRelease);
        private void OnRelease(GameObject poolItem) => poolItem.SetActive(false);
        private void OnGet(GameObject poolItem) => poolItem.SetActive(true);
        private GameObject CreatePoolItem()
        {
            var poolItem = Instantiate(indicatorPrefab);
            if (player && poolItem.TryGetComponent<Indicator>(out var indicator))
            {
                indicator.follow = player;
                indicator.offset = offset;
                indicator.distanceAway = distanceAway;
            }
            return poolItem;
        }
        public void IndicatorFor(GameObject track)
        {
            var poolItem = _indicatorPool.Get();
            if (poolItem.TryGetComponent<Indicator>(out var indicator))
                indicator.SetTrack(track.transform);
            if (track.TryGetComponent<ICanDie>(out var ableToDie))
            {
                void OnDied(GameObject _)
                {
                    ableToDie.Died -= OnDied;
                    _indicatorPool.Release(poolItem);
                }

                ableToDie.Died += OnDied;
            }
        }
    }
}
