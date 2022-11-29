using TMPro;
using UnityEngine;

namespace Scoring
{
    [ExecuteInEditMode]
    public class ScoreKeeper : MonoBehaviour
    {
        public TextMeshProUGUI textMesh;
        public string beforeScoreText = "Score:";
        private int _currentScore;

        public void Reset()
        {
            _currentScore = 0;
            SyncCurrentScore();
        }
        private void Start()
        {
            textMesh ??= GetComponent<TextMeshProUGUI>();
            Reset();
        }
        public void OnScoreAdded(ScoreAddedResult scoreAddedResult)
        {
            _currentScore = scoreAddedResult.UpdatedScore;
            SyncCurrentScore();
        }
        private void SyncCurrentScore() => textMesh.text = $"{beforeScoreText} {_currentScore}";
    }

}
