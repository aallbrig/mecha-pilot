using System;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class ScoreKeeper : MonoBehaviour, IScoreKeeper
    {
        public TextMeshProUGUI textMesh;
        public string beforeScoreText = "Score:";
        private int _currentScore;
        private void Start()
        {
            textMesh ??= GetComponent<TextMeshProUGUI>();
            SyncCurrentScore();
        }
        private void OnEnable() => _currentScore = 0;

        public event Action<ScoreAddedResult> ScoreAdded;

        public void AddToCurrentScore(int scoreIncrease)
        {
            var result = new ScoreAddedResult
            {
                PreviousScore = _currentScore, UpdatedScore = _currentScore + scoreIncrease, ScoreIncrease = scoreIncrease
            };
            _currentScore = result.UpdatedScore;
            SyncCurrentScore();
            ScoreAdded?.Invoke(result);
        }
        private void SyncCurrentScore() => textMesh.text = $"{beforeScoreText} {_currentScore}";
    }

    public class ScoreAddedResult
    {
        public int PreviousScore { get; set; }

        public int UpdatedScore { get; set; }

        public int ScoreIncrease { get; set; }
    }

    public interface IScoreKeeper
    {
        public event Action<ScoreAddedResult> ScoreAdded;

        public void AddToCurrentScore(int scoreIncrease);
    }
}
