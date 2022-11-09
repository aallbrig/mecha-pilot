using Combat;
using Gameplay.OutOfPlaySphereBehaviors;
using UnityEngine;

namespace Spawners
{
    public class BeginnerRepeatSpawner : RepeatSpawner
    {
        [Range(1.0f, 10.0f)] public float enemiesLimit = 1.0f;

        public float amountToRampUpPerKill = 0.25f;
        private int _currentEnemiesCount;
        private float _currentEnemiesLimit;
        private void Start() => Spawned += OnSpawnerSpawned;

        private void OnEnable() => _currentEnemiesLimit = enemiesLimit;
        private void OnSpawnerSpawned(GameObject spawnee)
        {
            _currentEnemiesCount++;
            if (spawnee.TryGetComponent<DeactivateWhenOutsidePlaySphere>(out var outsidePlaySphere))
                outsidePlaySphere.Deactivated += () => _currentEnemiesCount--;
            if (spawnee.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ =>
                {
                    _currentEnemiesCount--;
                    _currentEnemiesLimit += amountToRampUpPerKill;
                };
        }
        protected override bool ShouldSpawn()
        {
            var calculatedLimit = (int)Mathf.Floor(_currentEnemiesLimit);
            return _currentEnemiesCount < calculatedLimit && base.ShouldSpawn();
        }
    }
}
