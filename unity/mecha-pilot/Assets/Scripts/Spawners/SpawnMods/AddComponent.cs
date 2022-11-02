using Gameplay.OutOfPlaySphereBehaviors;
using UnityEngine;

namespace Spawners.SpawnMods
{
    public class AddComponent : SpawnerModBase
    {
        public bool deActiveOnOutOfPlaySphere = true;
        private void Start() => Spawner.Spawned += AddComponents;
        private void AddComponents(GameObject spawnedGameObject)
        {
            if (deActiveOnOutOfPlaySphere) spawnedGameObject.AddComponent<DeactivateWhenOutsidePlaySphere>();
        }
    }
}
