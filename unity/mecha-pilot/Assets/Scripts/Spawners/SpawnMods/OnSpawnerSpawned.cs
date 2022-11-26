using UnityEngine;
using UnityEngine.Events;

namespace Spawners.SpawnMods
{

    [RequireComponent(typeof(Spawner))]
    public class OnSpawnerSpawned : MonoBehaviour
    {
        public UnityEvent<GameObject> onSpawned;
        [SerializeField] private Spawner spawner;
        private void Start()
        {
            spawner = GetComponent<Spawner>();
            spawner.Spawned += HandleSpawnerSpawned;
        }
        private void HandleSpawnerSpawned(GameObject spawnedGameObject) => onSpawned?.Invoke(spawnedGameObject);
    }
}
