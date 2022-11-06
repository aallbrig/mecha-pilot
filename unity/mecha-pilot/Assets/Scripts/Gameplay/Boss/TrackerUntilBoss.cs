using Combat;
using Gameplay.OutOfPlaySphereBehaviors;
using Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Boss
{
    public class TrackerUntilBoss : MonoBehaviour
    {
        public Spawner spawner;
        public int enemiesBeforeBoss = 20;
        public Slider sliderUi;
        private int _currentEnemiesKilledCount;
        private void Awake()
        {
            sliderUi = GetComponent<Slider>();
            sliderUi.minValue = 0;
            sliderUi.maxValue = enemiesBeforeBoss;
            spawner ??= FindObjectOfType<Spawner>();
            if (spawner)
                spawner.Spawned += spawnee =>
                {
                    var ableToDie = spawnee.GetComponent<ICanDie>();
                    ableToDie.Died += TrackDeath;
                    spawnee.GetComponent<DeactivateWhenOutsidePlaySphere>().Deactivated += () =>
                    {
                        ableToDie.Died -= TrackDeath;
                    };
                };
        }
        private void OnEnable()
        {
            _currentEnemiesKilledCount = 0;
            SyncSlider();
        }
        private void TrackDeath(GameObject enemy)
        {
            _currentEnemiesKilledCount++;
            SyncSlider();
        }
        private void SyncSlider() => sliderUi.value = _currentEnemiesKilledCount;
    }
}
