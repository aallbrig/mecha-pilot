using UnityEngine;

namespace Spawners.SpawnMods
{
    public class PositionOnSphereCollider : SpawnerModBase
    {
        public SphereCollider sphereCollider;
        public float insetValue;
        private bool _active;
        private Transform sphereColliderTransform;
        private void Start()
        {
            Spawner.Spawned += PositionSpawnedOnSphereCollider;
            if (sphereCollider) sphereColliderTransform = sphereCollider.gameObject.transform;
        }
        private void OnValidate() => _active = sphereCollider != default;
        private void PositionSpawnedOnSphereCollider(GameObject spawnedGameObject)
        {
            if (!_active) return;

            var randomizedPosition = Random.insideUnitCircle.normalized * sphereCollider.radius;
            var directionToCenter = (randomizedPosition - Vector2.zero).normalized;
            var insetOffset = directionToCenter * insetValue;
            var calculatedPosition =
                sphereColliderTransform.position + new Vector3(
                    randomizedPosition.x - insetOffset.x,
                    randomizedPosition.y - insetOffset.y,
                    0
                );
            spawnedGameObject.transform.position = calculatedPosition;
        }
    }
}
