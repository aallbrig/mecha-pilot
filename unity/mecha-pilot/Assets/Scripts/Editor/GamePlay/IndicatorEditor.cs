#if UNITY_EDITOR
using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor.GamePlay
{
    [CustomEditor(typeof(Indicator))]
    public class IndicatorEditor : UnityEditor.Editor
    {
        private Indicator _component;
        private GameObject _gameObject;
        private float _lineThickness = 3f;
        private Transform _transform;
        private void OnEnable()
        {
            _component = (Indicator)target;
            _gameObject = _component.gameObject;
            _transform = _gameObject.transform;
        }
        private void OnSceneGUI()
        {
            if (_component.follow)
            {
                // green line to player mecha (position constraint)
                Handles.color = Color.green;
                Handles.DrawLine(_transform.position, _component.follow.position, _lineThickness);
            }
            if (_component.track)
            {
                // red line to enemy (look at constraint)
                Handles.color = Color.red;
                Handles.DrawLine(_transform.position, _component.track.position, _lineThickness);
            }
        }
        public override void OnInspectorGUI()
        {
            _lineThickness = EditorGUILayout.FloatField("Line Thickness", _lineThickness);
            if (GUILayout.Button("Sync Position"))
            {
                if (_component.track == null || _component.follow == null) return;

                _transform.position = _component.CalculateIndicatorPosition();
            }
            base.OnInspectorGUI();
        }
    }
}
#endif
