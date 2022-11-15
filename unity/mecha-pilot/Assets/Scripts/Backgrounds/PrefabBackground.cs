using System.Collections.Generic;
using UnityEngine;

namespace Backgrounds
{
    public class PrefabBackground : MonoBehaviour
    {
        public Camera perspectiveCamera;
        public GameObject prefab;
        public Vector3 containerSize;
        public int numberOfPrefabInstances = 5;
        public List<Transform> backgroundContainers = new();
        private readonly Queue<Transform> _backgroundGeneratorQueue = new();

        private readonly List<Vector3> _directions = new()
        {
            Vector3.up, Vector3.right, Vector3.down, Vector3.left
        };

        private readonly int _limit = 25;

        private Transform _transform;
        public void Reset()
        {
            var backgroundTransform = _transform ? _transform : transform;
            foreach (Transform child in backgroundTransform)
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
            backgroundContainers.Clear();
        }
        private void OnEnable()
        {
            perspectiveCamera ??= Camera.current;
            perspectiveCamera ??= Camera.main;
            if (perspectiveCamera == null)
            {
                enabled = false;
                return;
            }
            if (prefab == null)
            {
                enabled = false;
                return;
            }

            _transform = transform;
        }

        public void PopulateBackgroundContainers() => backgroundContainers.ForEach(PopulateBackgroundContainer);
        private void PopulateBackgroundContainer(Transform backgroundContainer)
        {
            if (prefab == null) return;

            var backgroundTransform = _transform ? _transform : transform;
            var containerCollider = backgroundContainer.GetComponent<Collider>();
            var bounds = containerCollider.bounds;
            for (var i = 0; i < numberOfPrefabInstances; i++)
            {
                var randomX = Random.Range(bounds.min.x, bounds.max.x);
                var randomY = Random.Range(bounds.min.y, bounds.max.y);
                var randomPointInside = new Vector3(randomX, randomY, backgroundTransform.position.z);
                var prefabInstance = Instantiate(prefab, backgroundContainer);
                prefabInstance.transform.position = randomPointInside;
            }
        }
        public void CreateBackgroundContainers()
        {
            Reset();

            var i = 0;

            var startingContainer = NewBackgroundContainer($"{i++}", Vector3.zero);
            _backgroundGeneratorQueue.Clear();
            _backgroundGeneratorQueue.Enqueue(startingContainer);

            while (_backgroundGeneratorQueue.Count > 0)
            {
                var container = _backgroundGeneratorQueue.Dequeue();
                var containerComponent = container.GetComponent<BackgroundContainer>();

                _directions.ForEach(direction =>
                {
                    if (!containerComponent.ContainerInDirection(direction))
                    {
                        var containerPosition =
                            container.position + new Vector3(containerSize.x * direction.x, containerSize.y * direction.y, 0);
                        containerPosition.z = 0;
                        var newContainer = NewBackgroundContainer($"{i++}", containerPosition);
                        if (IsSeenByCamera(newContainer.gameObject)) _backgroundGeneratorQueue.Enqueue(newContainer);
                    }
                });
                if (i > _limit) break;
            }
        }
        private Transform NewBackgroundContainer(string containerName, Vector3 position)
        {
            var backgroundTransform = _transform ? _transform : transform;
            var container = new GameObject
            {
                name = containerName,
                transform =
                {
                    position = backgroundTransform.position + position,
                    parent = backgroundTransform
                }
            };
            var colliderComponent = container.AddComponent<BoxCollider>();
            colliderComponent.size = containerSize;
            var backgroundContainerComponent = container.AddComponent<BackgroundContainer>();
            backgroundContainerComponent.boundingCollider = colliderComponent;
            backgroundContainers.Add(container.transform);
            return container.transform;
        }
        public bool IsSeenByCamera(GameObject newBackgroundContainer)
        {
            // is this box outside the camera frustum
            var containerCollider = newBackgroundContainer.GetComponent<Collider>();
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(perspectiveCamera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, containerCollider.bounds);
        }
    }
}
