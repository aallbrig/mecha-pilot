using System;
using Combat;
using Gameplay;
using UnityEngine;

namespace Economy.RewardsForKill
{
    public class ScoreReward : MonoBehaviour
    {
        public int scoreReward;
        private ICanDie _ableToDie;
        private ScoreKeeper _scoreKeeper;
        private void Start()
        {
            _scoreKeeper ??= FindObjectOfType<ScoreKeeper>();
            // ScoreReward deactivates self if it can't find a score keeper in the scene
            if (_scoreKeeper == null)
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
        private void RewardScoreOnDeath(GameObject obj) => _scoreKeeper.AddToCurrentScore(scoreReward);
    }
}
