namespace LordOfUltima.Score
{
    class Score
    {
        private static Score _instanceScore;
        public static Score Instance
        {
            get { return _instanceScore ?? (_instanceScore = new Score()); }
        }

        public int ScoreValue { get; set; }

        public string GetTitle()
        {
            if (ScoreValue < 0)
                return "";
            if (ScoreValue < 100)
                return "Sir";
            if (ScoreValue < 200)
                return "Knight";
            if (ScoreValue < 500)
                return "Baron";
            if (ScoreValue < 2000)
                return "Earl";
            if (ScoreValue < 5000)
                return "Marquess";
            if (ScoreValue < 10000)
                return "Prince";
            if (ScoreValue < 20000)
                return "Duke";
            if (ScoreValue < 50000)
                return "King";

            return "Emperor";
        }
    }
}
