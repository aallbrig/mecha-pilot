using System.Linq;
using TMPro;
using UnityEngine;

namespace Scoring
{
    public class ScoresTable : MonoBehaviour
    {
        public ScoreManager scoreManager;
        public Transform scoreContainer;
        public GameObject scoreTableHeader;
        public GameObject scoreTableRow;
        public void OnEnable()
        {
            scoreContainer ??= transform;
            if (scoreContainer == null) enabled = false;
            RenderScores();
        }
        private void RenderScores()
        {
            foreach (Transform child in scoreContainer)
                Destroy(child.gameObject);
            Instantiate(scoreTableHeader, scoreContainer);
            if (scoreManager.scores.Count > 0)
            {
                foreach (var scoreRecord in scoreManager.scores.OrderByDescending(scoreRecord => scoreRecord.score))
                {
                    var instance = Instantiate(scoreTableRow, scoreContainer);
                    if (instance.TryGetComponent<ScoreTableRow>(out var scoreTableRowComponent))
                        scoreTableRowComponent.SetScore(scoreRecord);
                }
            }
            else
            {
                var temp = Instantiate(new GameObject(), scoreContainer);
                var textComponent = temp.AddComponent<TextMeshProUGUI>();
                textComponent.text = "No games have been played!";
                textComponent.alignment = TextAlignmentOptions.Midline;
            }
        }
    }
}
