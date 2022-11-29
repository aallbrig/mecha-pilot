using System;

namespace Scoring
{
    [Serializable]
    public struct ScoreRecord
    {
        public int score;
        public string initials;
        public DateTime playDate;
        public float startTime;
        public float endTime;

        public float PlayTime => endTime - startTime;
    }
}
