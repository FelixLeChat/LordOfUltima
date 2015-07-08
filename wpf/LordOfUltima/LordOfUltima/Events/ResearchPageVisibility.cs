using System.Windows;

namespace LordOfUltima.Events
{
    class ResearchPageVisibility
    {
        private static ResearchPageVisibility _instance;

        public static ResearchPageVisibility Instance
        {
            get { return _instance ?? (_instance = new ResearchPageVisibility()); }
        }
        private static bool _isVisible { get; set; }

        public static void HideResearchPage()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.research_page.Visibility = Visibility.Hidden;
            _isVisible = false;
        }

        public static void ShowResearchPage()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.research_page.Visibility = Visibility.Visible;
            _isVisible = true;
        }

        public static void InvertResearchPageVisibility()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            if (_isVisible)
            {
                mainWindow.research_page.Visibility = Visibility.Hidden;
                _isVisible = false;
            }
            else
            {
                mainWindow.research_page.Visibility = Visibility.Visible;
                _isVisible = true;
            }

        }
    }
}
