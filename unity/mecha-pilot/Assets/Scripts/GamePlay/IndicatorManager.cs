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
        [SerializeField] private List<GameObject> _activePoolInstances = new();
        private readonly Dictionary<ICanDie, Action<GameObject>> _activeMethods = new();
        private ObjectPool<GameObject> _indicatorPool;
        public void Reset()
        {
            for (var i = _activePoolInstances.Count - 1; i >= 0; i--)
            {
                var poolObject = _activePoolInstances[i];
                if (poolObject.activeSelf) _indicatorPool.Release(poolObject);
            }
            _activePoolInstances.Clear();
        }
        private void Start() =>
            _indicatorPool = new ObjectPool<GameObject>(CreatePoolItem, OnGet, OnRelease);
        private void OnRelease(GameObject poolItem)
        {
            poolItem.SetActive(false);
            _activePoolInstances.Remove(poolItem);
        }
        private void OnGet(GameObject poolItem)
        {
            _activePoolInstances.Add(poolItem);
            poolItem.SetActive(true);
        }
        private GameObject CreatePoolItem()
        {
            var poolItem = Instantiate(indicatorPrefab);
            poolItem.name = $"{indicatorPrefab.name} {_indicatorPool.CountAll + 1}";
            if (player && poolItem.TryGetComponent<Indicator>(out var indicator))
            {
                indicator.follow = player;
                indicator.onTrackDeactivated.AddListener(() => _indicatorPool.Release(poolItem));
            }
            return poolItem;
        }
        public void IndicatorFor(GameObject track)
        {
            var poolItem = _indicatorPool.Get();
            if (poolItem.TryGetComponent<Indicator>(out var indicator))
                indicator.SetTrack(track.transform);
        }
    }
}
