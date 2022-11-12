using Gameplay.OutOfPlaySphereBehaviors;
using UnityEngine;

namespace Spawners.SpawnMods
{
    public class AddComponent : SpawnerModBase
    {
        private void Start() => Spawner.Spawned += AddComponents;
        private void AddComponents(GameObject spawnedGameObject)
        {
            if (!spawnedGameObject.TryGetComponent<DeactivateWhenOutsidePlaySphere>(out _))
                spawnedGameObject.AddComponent<DeactivateWhenOutsidePlaySphere>();
        }
    }
}
