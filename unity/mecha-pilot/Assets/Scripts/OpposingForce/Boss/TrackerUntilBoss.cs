using System.Collections.Generic;
using Combat;
using Spawners;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OpposingForce.Boss
{
    [RequireComponent(typeof(Slider))]
    public class TrackerUntilBoss : MonoBehaviour
    {
        public Spawner spawner;
        public int enemiesBeforeBoss = 20;
        public Slider sliderUi;
        public UnityEvent onEffortPotentialReached;
        private readonly List<ICanDie> _deathsTracked = new();
        private int _currentEnemiesKilledCount;
        private void Awake()
        {
            sliderUi = GetComponent<Slider>();
            sliderUi.minValue = 0;
            sliderUi.maxValue = enemiesBeforeBoss;
            spawner ??= FindObjectOfType<Spawner>();
        }
        private void OnEnable()
        {
            _currentEnemiesKilledCount = 0;
            SyncSlider();
            if (spawner) spawner.Spawned += HandleSpawnerSpawned;
        }
        private void OnDisable()
        {
            if (spawner) spawner.Spawned -= HandleSpawnerSpawned;
        }
        private void HandleSpawnerSpawned(GameObject spawnee)
        {
            var ableToDie = spawnee.GetComponent<ICanDie>();
            if (!_deathsTracked.Contains(ableToDie))
            {
                ableToDie.Died += TrackDeath;
                _deathsTracked.Add(ableToDie);
            }
        }
        private void TrackDeath(GameObject enemy)
        {
            _currentEnemiesKilledCount = Mathf.Clamp(_currentEnemiesKilledCount + 1, 0, enemiesBeforeBoss);
            if (_currentEnemiesKilledCount >= enemiesBeforeBoss)
                onEffortPotentialReached?.Invoke();
            SyncSlider();
        }
        private void SyncSlider() => sliderUi.value = _currentEnemiesKilledCount;
    }
}
