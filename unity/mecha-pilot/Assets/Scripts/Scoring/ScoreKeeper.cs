using TMPro;
using UnityEngine;

namespace Scoring
{
    [ExecuteInEditMode]
    public class ScoreKeeper : MonoBehaviour
    {
        public ScoreManager scoreManager;
        public TextMeshProUGUI textMesh;
        public string beforeScoreText = "Score:";

        public void Reset() => SyncCurrentScore();
        private void Start()
        {
            textMesh ??= GetComponent<TextMeshProUGUI>();
            Reset();
        }
        private void OnEnable() => scoreManager.ScoreAdded += OnScoreAdded;
        private void OnDisable() => scoreManager.ScoreAdded -= OnScoreAdded;
        private void OnScoreAdded(ScoreAddedResult scoreAddedResult) => SyncCurrentScore();
        private void SyncCurrentScore() => textMesh.text = $"{beforeScoreText} {scoreManager.currentScore.score}";
    }

}
