#if UNITY_EDITOR
using System.Collections.Generic;
using Backgrounds;
using UnityEditor;
using UnityEngine;

namespace Editor.Backgrounds
{
    [CustomEditor(typeof(PrefabBackground))]
    public class PrefabBackgroundEditor : UnityEditor.Editor
    {
        private readonly Queue<BackgroundContainer> _backgroundGeneratorQueue = new();

        private PrefabBackground _componentInstance;
        private int _limit = 25;
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
                var backgroundContainerPosition = backgroundContainer.transform.position;
                Handles.DrawWireCube(backgroundContainerPosition, _componentInstance.containerSize);
                Handles.Label(backgroundContainerPosition, backgroundContainer.name);
            });
        }
        public override void OnInspectorGUI()
        {
            _limit = EditorGUILayout.IntField("Max generated containers", _limit);
            if (GUILayout.Button("Draw background containers"))
                CreateBackgroundContainers();
            if (GUILayout.Button("Reset"))
                _componentInstance.Reset();
            base.OnInspectorGUI();
        }
        public void CreateBackgroundContainers()
        {
            var startingContainer =
                _componentInstance.NewBackgroundContainer(new MissingBackgroundContainerReport
                {
                    ExpectedCoordinates = Vector3Int.zero,
                    ExpectedLocation = _componentInstance.transform.position
                });
            _componentInstance.activeBackgroundContainers.Add(startingContainer);
            _backgroundGeneratorQueue.Clear();
            _backgroundGeneratorQueue.Enqueue(startingContainer);

            var frustumPlanes = _componentInstance.CalculateFrustumPlanes();
            while (_backgroundGeneratorQueue.Count > 0)
            {
                var container = _backgroundGeneratorQueue.Dequeue();
                var containerComponent = container.GetComponent<BackgroundContainer>();
                var missingContainerReports = containerComponent.AuditMissingContainers();
                if (missingContainerReports.Length > 0)
                    foreach (var missingContainerReport in missingContainerReports)
                    {
                        var newContainer = _componentInstance.NewBackgroundContainer(missingContainerReport);
                        containerComponent.RegisterContainer(missingContainerReport.RelativeDirection, newContainer);

                        if (_componentInstance.IsSeenByCamera(newContainer.gameObject, frustumPlanes))
                        {
                            _componentInstance.activeBackgroundContainers.Add(newContainer);
                            _backgroundGeneratorQueue.Enqueue(newContainer);
                        }
                    }

                if (_componentInstance.ContainerCount > _limit) break;
            }
        }
    }
}
#endif
