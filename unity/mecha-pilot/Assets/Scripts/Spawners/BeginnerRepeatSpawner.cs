using Combat;
using Gameplay.OutOfPlaySphereBehaviors;
using UnityEngine;

namespace Spawners
{
    public class BeginnerRepeatSpawner : RepeatSpawner
    {
        [Range(1.0f, 10.0f)] public float enemiesLimit = 1.0f;
        public float amountToAddToEnemyCountPerKill = 0.25f;
        public float beginnerTimeBuffer = 3f;
        public float bufferToDecreaseByPerKill = 0.2f;
        private float _currentBeginnerTimeBuffer;
        private int _currentEnemiesCount;
        private float _currentEnemiesLimit;

        private void OnEnable()
        {
            _currentBeginnerTimeBuffer = beginnerTimeBuffer;
            _currentEnemiesLimit = enemiesLimit;
            _currentEnemiesCount = 0;
        }
        protected override void OnSpawnerSpawned(GameObject spawnee)
        {
            base.OnSpawnerSpawned(spawnee);

            _currentEnemiesCount++;

            if (spawnee.TryGetComponent<DeactivateWhenOutsidePlaySphere>(out var outsidePlaySphere))
                outsidePlaySphere.Deactivated += () => _currentEnemiesCount--;
            if (spawnee.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ =>
                {
                    _currentEnemiesCount--;
                    _currentEnemiesLimit += amountToAddToEnemyCountPerKill;
                    _currentBeginnerTimeBuffer -= bufferToDecreaseByPerKill;
                };
        }
        protected override bool ShouldSpawn()
        {
            var calculatedLimit = (int)Mathf.Floor(_currentEnemiesLimit);
            var shouldSpawn = _currentEnemiesCount < calculatedLimit && base.ShouldSpawn();
            return shouldSpawn;
        }
        protected override float NewDelayTime()
        {
            var calculatedDelayTime = base.NewDelayTime();
            if (_currentBeginnerTimeBuffer > 0f)
                calculatedDelayTime += _currentBeginnerTimeBuffer;
            return calculatedDelayTime;
        }
    }
}
