using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scoring
{
    public class ScoreManager : MonoBehaviour
    {
        public ScoreRecord currentScore;
        public List<ScoreRecord> scores;
        public UnityEvent<ScoreAddedResult> onScoreAdded;

        public void Reset()
        {
            currentScore = new ScoreRecord
            {
                playDate = DateTime.Today,
                startTime = Time.time,
                score = 0
            };
            var result = new ScoreAddedResult
            {
                PreviousScore = currentScore.score,
                UpdatedScore = 0,
                ScoreIncrease = -currentScore.score
            };
            onScoreAdded?.Invoke(result);
        }

        private void OnEnable() => Reset();

        public void AddToCurrentScore(int scoreIncrease)
        {
            var result = new ScoreAddedResult
            {
                PreviousScore = currentScore.score,
                UpdatedScore = currentScore.score + scoreIncrease,
                ScoreIncrease = scoreIncrease
            };
            currentScore.score = result.UpdatedScore;
            onScoreAdded?.Invoke(result);
        }

        public void SetEndTime() => currentScore.endTime = Time.time;
    }
}
