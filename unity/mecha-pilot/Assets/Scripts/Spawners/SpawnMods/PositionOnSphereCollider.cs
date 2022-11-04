using UnityEngine;

namespace Spawners.SpawnMods
{
    public class PositionOnSphereCollider : SpawnerModBase
    {
        public SphereCollider sphereCollider;
        public float insetValue;
        private Transform _sphereColliderTransform;
        private void Start()
        {
            Spawner.Spawned += PositionSpawnedOnSphereCollider;
            if (sphereCollider) _sphereColliderTransform = sphereCollider.gameObject.transform;
        }
        private void PositionSpawnedOnSphereCollider(GameObject spawnedGameObject)
        {
            var randomizedPosition = Random.insideUnitCircle.normalized * sphereCollider.radius;
            var directionToCenter = (randomizedPosition - Vector2.zero).normalized;
            var insetOffset = directionToCenter * insetValue;
            var calculatedPosition =
                _sphereColliderTransform.position + new Vector3(
                    randomizedPosition.x - insetOffset.x,
                    randomizedPosition.y - insetOffset.y,
                    0
                );
            spawnedGameObject.transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, 0);
            // Trail renderer will leave trails if set active doesn't happen here
            spawnedGameObject.SetActive(true);
        }
    }
}
