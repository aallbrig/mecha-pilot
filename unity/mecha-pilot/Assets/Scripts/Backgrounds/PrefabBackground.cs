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
        public List<BackgroundContainer> allBackgroundContainers = new();
        public List<BackgroundContainer> activeBackgroundContainers = new();
        private readonly Queue<BackgroundContainer> _backgroundGeneratorQueue = new();
        private readonly int _limit = 25;

        private Plane[] _frustumPlanes;
        private float _timeOfLastBackgroundRefresh;

        private Transform _transform;
        private void Awake()
        {
            Reset();
            CreateBackgroundContainers();
        }
        public void Reset()
        {
            var backgroundTransform = _transform ? _transform : transform;
            while (backgroundTransform.childCount > 0)
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
                    var missingContainerReports = container.AuditMissingContainers();
                    if (missingContainerReports.Length > 0)
                        foreach (var missingContainerReport in missingContainerReports)
                        {
                            var newContainer = NewBackgroundContainer(missingContainerReport);
                            container.RegisterContainer(missingContainerReport.RelativeDirection, newContainer);
                        }
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
        public void CreateBackgroundContainers()
        {
            var startingContainer =
                NewBackgroundContainer(new MissingBackgroundContainerReport
                {
                    ExpectedCoordinates = Vector3Int.zero,
                    ExpectedLocation = transform.position
                });
            activeBackgroundContainers.Add(startingContainer);
            _backgroundGeneratorQueue.Clear();
            _backgroundGeneratorQueue.Enqueue(startingContainer);

            var frustumPlanes = CalculateFrustumPlanes();
            while (_backgroundGeneratorQueue.Count > 0)
            {
                var container = _backgroundGeneratorQueue.Dequeue();
                var containerComponent = container.GetComponent<BackgroundContainer>();
                var missingContainerReports = containerComponent.AuditMissingContainers();
                if (missingContainerReports.Length > 0)
                    foreach (var missingContainerReport in missingContainerReports)
                    {
                        var newContainer = NewBackgroundContainer(missingContainerReport);
                        containerComponent.RegisterContainer(missingContainerReport.RelativeDirection, newContainer);

                        if (IsSeenByCamera(newContainer.gameObject, frustumPlanes))
                        {
                            activeBackgroundContainers.Add(newContainer);
                            _backgroundGeneratorQueue.Enqueue(newContainer);
                        }
                    }

                if (backgroundContainerCount > _limit) break;
            }
        }

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
        public BackgroundContainer NewBackgroundContainer(MissingBackgroundContainerReport missingReport)
        {
            var backgroundTransform = _transform ? _transform : transform;
            var container = new GameObject
            {
                name = $"{NewBackgroundContainerId()} {missingReport.ExpectedCoordinates}",
                transform =
                {
                    position = missingReport.ExpectedLocation,
                    parent = backgroundTransform
                }
            };
            var colliderComponent = container.AddComponent<BoxCollider>();
            colliderComponent.size = containerSize;
            var backgroundContainerComponent = container.AddComponent<BackgroundContainer>();
            backgroundContainerComponent.coordinates = missingReport.ExpectedCoordinates;
            backgroundContainerComponent.boundingCollider = colliderComponent;
            allBackgroundContainers.Add(backgroundContainerComponent);
            PopulateBackgroundContainer(container.transform);
            return backgroundContainerComponent;
        }
        private int NewBackgroundContainerId() => backgroundContainerCount++;
        public bool IsSeenByCamera(GameObject newBackgroundContainer, Plane[] frustumPlanes)
        {
            // is this box outside the camera frustum
            var containerCollider = newBackgroundContainer.GetComponent<Collider>();
            return GeometryUtility.TestPlanesAABB(frustumPlanes, containerCollider.bounds);
        }
    }
}
