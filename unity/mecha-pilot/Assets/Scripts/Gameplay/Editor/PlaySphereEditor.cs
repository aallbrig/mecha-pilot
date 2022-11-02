#if UNITY_EDITOR
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Editor
{
    [CustomEditor(typeof(PlaySphere))]
    public class PlaySphereEditor : UnityEditor.Editor
    {
        private GameObject _gameObject;
        private GUIStyle _labelStyle;
        private PlaySphere _playSphere;
        private SphereCollider _sphereCollider;
        private Transform _transform;
        private void OnEnable()
        {
            _labelStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter };
            _playSphere = (PlaySphere)target;
            _gameObject = _playSphere.gameObject;
            _transform = _gameObject.transform;
            _sphereCollider = _gameObject.GetComponent<SphereCollider>();
        }
        private void OnSceneGUI()
        {
            var position = _transform.position;
            var strBuilder = new StringBuilder();
            strBuilder.Append($"Play Sphere {_gameObject.name}");

            Handles.Label(
                position + Vector3.up * 2,
                strBuilder.ToString(),
                _labelStyle
            );
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(position, Vector3.forward, _sphereCollider.radius);
        }
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
#endif
