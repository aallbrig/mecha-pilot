using Scoring;
using UnityEditor;
using UnityEngine;

namespace Editor.Scoring
{
    [CustomEditor(typeof(ScoreManager))]
    public class ScoreManagerEditor : UnityEditor.Editor
    {
        private ScoreManager _componentInstance;
        private void OnEnable() => _componentInstance = (ScoreManager)target;
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Clear saved scores"))
                Debug.Log("Score is cleared");
            base.OnInspectorGUI();
        }
    }
}
