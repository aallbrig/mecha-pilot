using System.Text;
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
            var strBuilder = new StringBuilder();
            strBuilder.Append($"Player {_playerGameObject.name}");
            strBuilder.Append($"\nMove input {_playerController.MoveInput}");
            strBuilder.Append($"\nMove vector {_playerController.MoveVector}");
            strBuilder.Append($"\nFire input {_playerController.FireInput}");
            strBuilder.Append($"\nFire vector {_playerController.FireVector}");

            Handles.Label(
                position + Vector3.up * 2,
                strBuilder.ToString(),
                _labelStyle
            );
            Handles.color = Color.green;
            Handles.DrawWireDisc(position, Vector3.forward, 1f);
        }
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
