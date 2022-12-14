using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Scoring
{
    [Serializable]
    public class ScoreSave
    {
        public List<ScoreRecord> scores;
        public ScoreSave(List<ScoreRecord> scores) => this.scores = scores;
    }

    public class ScoreManager : MonoBehaviour
    {
        public InitialLetterInputForm form;
        public ScoreRecord currentScore;
        public List<ScoreRecord> scores;
        public UnityEvent<ScoreAddedResult> onScoreAdded;
        private readonly string _saveKey = "MF_Scores";

        private void Awake()
        {
            if (PlayerPrefs.HasKey(_saveKey))
            {
                var value = JsonUtility.FromJson<ScoreSave>(PlayerPrefs.GetString(_saveKey));
                scores = value.scores;
            }
        }
        public void Reset()
        {
            currentScore = new ScoreRecord
            {
                PlayDate = DateTime.Today,
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
        public void SaveScore()
        {
            if (form) currentScore.initials = form.GetInitials();
            scores.Add(currentScore);
            var jsonString = JsonUtility.ToJson(new ScoreSave(scores));
            PlayerPrefs.SetString(_saveKey, jsonString);
            PlayerPrefs.Save();
        }
    }
}
