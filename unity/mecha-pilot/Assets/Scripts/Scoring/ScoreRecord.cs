using System;

namespace Scoring
{
    [Serializable]
    public struct ScoreRecord
    {
        public int score;
        public string initials;
        public float playTime;
        public DateTime playDate;
    }
}
