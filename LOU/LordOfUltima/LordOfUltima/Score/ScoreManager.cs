namespace LordOfUltima.Score
{
    class ScoreManager
    {
        private static ScoreManager _instanceScoreManager;
        public static ScoreManager Instance
        {
            get { return _instanceScoreManager ?? (_instanceScoreManager = new ScoreManager()); }
        }

        public void UpdateVisual()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.label_player_score.Content = Score.Instance.ScoreValue;
            mainWindow.label_player_title.Content = Score.Instance.GetTitle();
        }
    }
}
