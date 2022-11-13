using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        public ScoreRecord currentScore;
        public List<ScoreRecord> scores;
        public void Reset() => currentScore = new ScoreRecord
        {
            playDate = DateTime.Today,
            playTime = 0,
            score = 0
        };

        private void OnEnable() => Reset();
        private void OnDisable() {}

        public event Action<ScoreAddedResult> ScoreAdded;

        public void AddToCurrentScore(int scoreIncrease)
        {
            var result = new ScoreAddedResult
            {
                PreviousScore = currentScore.score,
                UpdatedScore = currentScore.score + scoreIncrease,
                ScoreIncrease = scoreIncrease
            };
            currentScore.score = result.UpdatedScore;
            ScoreAdded?.Invoke(result);
        }
    }
}
