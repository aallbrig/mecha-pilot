using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor.GamePlay
{
    [CustomEditor(typeof(GameStateTransitionReactor))]
    [CanEditMultipleObjects]
    public class GameStateTransitionReactorEditor : UnityEditor.Editor
    {
        private GameStateTransitionReactor _component;
        private void OnEnable() => _component = (GameStateTransitionReactor)target;
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Test on enter unity events"))
                _component.onEnter?.Invoke();
            if (GUILayout.Button("Test on exit unity events"))
                _component.onExit?.Invoke();
            base.OnInspectorGUI();
        }
    }
}
