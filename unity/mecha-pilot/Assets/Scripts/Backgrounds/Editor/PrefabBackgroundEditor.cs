#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Backgrounds.Editor
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
                _componentInstance.CreateBackgroundContainers();
            if (GUILayout.Button("Reset"))
                _componentInstance.Reset();
            base.OnInspectorGUI();
        }
    }
}
#endif
