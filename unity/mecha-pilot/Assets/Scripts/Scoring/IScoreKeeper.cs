using System;

namespace Scoring
{
    public interface IScoreKeeper
    {
        public event Action<ScoreAddedResult> ScoreAdded;

        public void AddToCurrentScore(int scoreIncrease);
        public void Reset();
    }
}