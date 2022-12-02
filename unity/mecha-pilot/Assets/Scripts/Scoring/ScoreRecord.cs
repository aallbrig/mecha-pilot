using System;

namespace Scoring
{
    [Serializable]
    public struct ScoreRecord
    {
        public int score;
        public string initials;

        public int month;
        public int day;
        public int year;
        public float startTime;
        public float endTime;

        private DateTime _playDate;

        public DateTime PlayDate
        {
            get => _playDate;
            set
            {
                _playDate = value;
                month = _playDate.Month;
                day = _playDate.Day;
                year = _playDate.Year;
            }
        }

        public float PlayTime => endTime - startTime;
    }
}
