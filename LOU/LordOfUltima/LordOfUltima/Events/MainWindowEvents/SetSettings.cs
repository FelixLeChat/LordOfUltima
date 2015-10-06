namespace LordOfUltima.Events.MainWindowEvents
{
    class SetSettings
    {
        private static SetSettings _instanceSettings;
        public static SetSettings Instance
        { get { return _instanceSettings ?? (_instanceSettings = new SetSettings()); } }

        public void Set()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if(mainWindow == null)
                return;

            mainWindow.trigger_dark.IsChecked = Properties.Settings.Default.IsDarkSkin;

            mainWindow.trigger_level.IsChecked = Properties.Settings.Default.IsBuildingLevelVisible;
            LevelIndicatorVisibility.Instance.HandleLevelIndicatorVisibility();
        }
    }
}
