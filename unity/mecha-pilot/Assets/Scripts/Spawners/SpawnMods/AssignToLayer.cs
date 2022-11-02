using UnityEngine;

namespace Spawners.SpawnMods
{
    public class AssignToLayer : SpawnerModBase
    {
        public GameObject matchLayer;
        private void Start()
        {
            matchLayer ??= gameObject;
            Spawner.Spawned += AssignLayer;
        }
        private void AssignLayer(GameObject spawnedGameObject) => spawnedGameObject.layer = matchLayer.layer;
    }
}
