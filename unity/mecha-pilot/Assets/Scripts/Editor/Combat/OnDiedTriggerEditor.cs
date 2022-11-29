using Combat;
using UnityEditor;
using UnityEngine;

namespace Editor.Combat
{
    [CustomEditor(typeof(OnDiedTrigger))]
    public class OnDiedTriggerEditor : UnityEditor.Editor
    {
        private OnDiedTrigger _componentInstance;
        private void OnEnable() => _componentInstance = (OnDiedTrigger)target;
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Trigger Death Event"))
                _componentInstance.HandleOnDied(_componentInstance.gameObjectAbleToDie);
            base.OnInspectorGUI();
        }
    }
}
