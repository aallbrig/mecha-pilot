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
        public float secondsUntilBackgroundRefresh = 2f;
        [SerializeField] public int backgroundContainerCount;
        public List<Transform> allBackgroundContainers = new();
        public List<Transform> activeBackgroundContainers = new();

        public List<Vector3> Directions = new()
        {
            Vector3.up, Vector3.right, Vector3.down, Vector3.left
        };

        private Plane[] _frustumPlanes;
        private float _timeOfLastBackgroundRefresh;

        private Transform _transform;
        public void Reset()
        {
            var backgroundTransform = _transform ? _transform : transform;
            foreach (Transform child in backgroundTransform)
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
            allBackgroundContainers.Clear();
            activeBackgroundContainers.Clear();
            backgroundContainerCount = 0;
        }
        private void Update()
        {
            if (Time.time - _timeOfLastBackgroundRefresh > secondsUntilBackgroundRefresh)
            {
                var frustumPlanes = CalculateFrustumPlanes();
                activeBackgroundContainers.Clear();
                allBackgroundContainers.ForEach(container =>
                {
                    if (IsSeenByCamera(container.gameObject, frustumPlanes)) activeBackgroundContainers.Add(container);
                });
                activeBackgroundContainers.ForEach(container =>
                {
                    var containerComponent = container.GetComponent<BackgroundContainer>();
                    Directions.ForEach(direction =>
                    {
                        if (!containerComponent.ContainerInDirection(direction))
                        {
                            var newContainerPosition =
                                container.position + new Vector3(containerSize.x * direction.x, containerSize.y * direction.y,
                                    0);
                            newContainerPosition.z = 0;
                            var newContainer = NewBackgroundContainer($"{backgroundContainerCount++}", newContainerPosition);
                        }
                    });
                });
                _timeOfLastBackgroundRefresh = Time.time;
            }
        }
        private void OnEnable()
        {
            _timeOfLastBackgroundRefresh = Time.time - secondsUntilBackgroundRefresh;
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

        public Plane[] CalculateFrustumPlanes() => GeometryUtility.CalculateFrustumPlanes(perspectiveCamera);

        private void PopulateBackgroundContainer(Transform backgroundContainer)
        {
            if (prefab == null || backgroundContainer == null) return;

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
        public Transform NewBackgroundContainer(string containerName, Vector3 position)
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
            allBackgroundContainers.Add(container.transform);
            PopulateBackgroundContainer(container.transform);
            return container.transform;
        }
        public bool IsSeenByCamera(GameObject newBackgroundContainer, Plane[] frustumPlanes)
        {
            // is this box outside the camera frustum
            var containerCollider = newBackgroundContainer.GetComponent<Collider>();
            return GeometryUtility.TestPlanesAABB(frustumPlanes, containerCollider.bounds);
        }
    }
}
