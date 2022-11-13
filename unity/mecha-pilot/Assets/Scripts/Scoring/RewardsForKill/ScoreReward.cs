using System;
using Combat;
using UnityEngine;

namespace Scoring.RewardsForKill
{
    public class ScoreReward : MonoBehaviour
    {
        public int scoreReward;
        private ICanDie _ableToDie;
        private ScoreManager _scoreManager;
        private void Start()
        {
            _scoreManager ??= FindObjectOfType<ScoreManager>(true);
            // ScoreReward deactivates self if it can't find a score keeper in the scene
            if (_scoreManager == null)
            {
                enabled = false;
                throw new NullReferenceException("score keeper is required");
            }
            _ableToDie ??= GetComponent<ICanDie>();
            if (_ableToDie == null)
            {
                enabled = false;
                throw new NullReferenceException("need to be attached to a game object that can die");
            }
            _ableToDie.Died += RewardScoreOnDeath;
        }
        private void RewardScoreOnDeath(GameObject deadGameObject) => _scoreManager.AddToCurrentScore(scoreReward);
    }
}
