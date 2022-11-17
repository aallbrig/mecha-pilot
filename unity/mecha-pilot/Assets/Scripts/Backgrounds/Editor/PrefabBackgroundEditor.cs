#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Backgrounds.Editor
{
    [CustomEditor(typeof(PrefabBackground))]
    public class PrefabBackgroundEditor : UnityEditor.Editor
    {
        private readonly Queue<Transform> _backgroundGeneratorQueue = new();
        private readonly int _limit = 25;

        private PrefabBackground _componentInstance;
        private void OnEnable() => _componentInstance = (PrefabBackground)target;
        private void OnSceneGUI()
        {

            var frustumPlanes = _componentInstance.CalculateFrustumPlanes();
            _componentInstance.allBackgroundContainers.ForEach(backgroundContainer =>
            {
                if (backgroundContainer == null) return;

                Handles.color = _componentInstance.IsSeenByCamera(backgroundContainer.gameObject, frustumPlanes)
                    ? Color.green
                    : Color.magenta;
                var backgroundContainerPosition = backgroundContainer.position;
                Handles.DrawWireCube(backgroundContainerPosition, _componentInstance.containerSize);
                Handles.Label(backgroundContainerPosition, backgroundContainer.name);
            });
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Draw background containers"))
                CreateBackgroundContainers();
            if (GUILayout.Button("Reset"))
                _componentInstance.Reset();
            base.OnInspectorGUI();
        }

        private void CreateBackgroundContainers()
        {
            var startingContainer =
                _componentInstance.NewBackgroundContainer($"{_componentInstance.backgroundContainerCount++}", Vector3.zero);
            _componentInstance.activeBackgroundContainers.Add(startingContainer.transform);
            _backgroundGeneratorQueue.Clear();
            _backgroundGeneratorQueue.Enqueue(startingContainer);

            var frustumPlanes = _componentInstance.CalculateFrustumPlanes();
            while (_backgroundGeneratorQueue.Count > 0)
            {
                var container = _backgroundGeneratorQueue.Dequeue();
                var containerComponent = container.GetComponent<BackgroundContainer>();

                _componentInstance.Directions.ForEach(direction =>
                {
                    if (!containerComponent.ContainerInDirection(direction))
                    {
                        var containerPosition =
                            container.position + new Vector3(_componentInstance.containerSize.x * direction.x,
                                _componentInstance.containerSize.y * direction.y, 0);
                        containerPosition.z = 0;
                        var newContainer =
                            _componentInstance.NewBackgroundContainer($"{_componentInstance.backgroundContainerCount++}",
                                containerPosition);
                        if (_componentInstance.IsSeenByCamera(newContainer.gameObject, frustumPlanes))
                        {
                            _componentInstance.activeBackgroundContainers.Add(newContainer.transform);
                            _backgroundGeneratorQueue.Enqueue(newContainer);
                        }
                    }
                });
                if (_componentInstance.backgroundContainerCount > _limit) break;
            }
        }
    }
}
#endif
