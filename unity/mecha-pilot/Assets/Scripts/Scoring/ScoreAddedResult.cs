using System;

namespace Scoring
{
    [Serializable]
    public class ScoreAddedResult
    {
        public int PreviousScore { get; set; }

        public int UpdatedScore { get; set; }

        public int ScoreIncrease { get; set; }
    }
}
