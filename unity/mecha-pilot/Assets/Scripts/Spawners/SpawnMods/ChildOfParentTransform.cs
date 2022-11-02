using UnityEngine;

namespace Spawners.SpawnMods
{
    public class ChildOfParentTransform : SpawnerModBase
    {
        public Transform parentTransform;
        private bool _active;
        private void Start() => Spawner.Spawned += AssignParentTransform;
        private void OnValidate() => _active = parentTransform != null;
        private void AssignParentTransform(GameObject obj)
        {
            if (!_active) return;

            obj.transform.parent = parentTransform;
        }
    }
}
