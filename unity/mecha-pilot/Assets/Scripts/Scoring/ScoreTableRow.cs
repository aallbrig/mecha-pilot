using TMPro;
using UnityEngine;

namespace Scoring
{
    public class ScoreTableRow : MonoBehaviour
    {
        public ScoreRecord score;
        public TextMeshProUGUI initials;
        public TextMeshProUGUI gameScore;
        public TextMeshProUGUI playTime;
        public TextMeshProUGUI playDate;
        public void SetScore(ScoreRecord scoreRecord)
        {
            score = scoreRecord;
            SyncScore();
        }
        private void SyncScore()
        {
            if (initials) initials.text = score.initials;
            if (gameScore) gameScore.text = $"{score.score}";
            if (playTime) playTime.text = $"{score.PlayTime}";
            if (playDate) playDate.text = $"{score.month}/{score.day}/{score.year}";
        }
    }
}
