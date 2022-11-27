using System;
using System.Collections.Generic;
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
        public GameManager gameManager;
        private readonly Dictionary<GameObject, Action<GameObject>> _activeMethods = new();
        private readonly List<GameObject> _activePoolItems = new();
        private ObjectPool<GameObject> _indicatorPool;
        public void Reset()
        {
            foreach (var poolItem in _activePoolItems)
            {
                if (_activeMethods.TryGetValue(poolItem, out var onDied) &&
                    poolItem.TryGetComponent<ICanDie>(out var ableToDie))
                {
                    ableToDie.Died -= onDied;
                    _activeMethods.Remove(poolItem);
                }

                _indicatorPool.Release(poolItem);
            }
            _activePoolItems.Clear();
        }
        private void Start()
        {
            _indicatorPool = new ObjectPool<GameObject>(CreatePoolItem, OnGet, OnRelease);
            if (gameManager) gameManager.GameIsOver += Reset;
        }
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
            _activePoolItems.Add(poolItem);
            if (poolItem.TryGetComponent<Indicator>(out var indicator))
                indicator.SetTrack(track.transform);
            if (track.TryGetComponent<ICanDie>(out var ableToDie))
            {
                void OnDied(GameObject _)
                {
                    ableToDie.Died -= OnDied;
                    _indicatorPool.Release(poolItem);
                    _activePoolItems.Remove(poolItem);
                    _activeMethods.Remove(poolItem);
                }

                _activeMethods.Add(poolItem, OnDied);
                ableToDie.Died += OnDied;
            }
        }
    }
}
