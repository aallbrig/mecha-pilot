using UnityEditor;
using UnityEngine;

namespace Character.Editor
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        private GUIStyle _labelStyle;
        private PlayerController _playerController;
        private GameObject _playerGameObject;
        private Transform _playerTransform;
        private void OnEnable()
        {
            _labelStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter };
            _playerController = (PlayerController)target;
            _playerGameObject = _playerController.gameObject;
            _playerTransform = _playerGameObject.transform;
        }
        private void OnSceneGUI()
        {
            var position = _playerTransform.position;
            Handles.Label(
                position + Vector3.up * 2,
                $"Player {_playerGameObject.name}\nMove Input {_playerController.MoveInput}\nFire Input {_playerController.FireInput}",
                _labelStyle
            );
            Handles.color = Color.green;
            Handles.DrawWireDisc(position, Vector3.forward, 1f);
        }
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
