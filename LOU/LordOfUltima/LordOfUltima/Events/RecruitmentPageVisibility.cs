using System.Windows;

namespace LordOfUltima.Events
{
    class RecruitmentPageVisibility
    {
        private static RecruitmentPageVisibility _instance;

        public static RecruitmentPageVisibility Instance
        {
            get { return _instance ?? (_instance = new RecruitmentPageVisibility()); }
        }
        private static bool _isVisible { get; set; }

        public static void HideRecruitmentPage()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.recruitment_page.Visibility = Visibility.Hidden;
            _isVisible = false;
        }

        public static void ShowRecruitmentPage()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.recruitment_page.Visibility = Visibility.Visible;
            _isVisible = true;
        }

        public static void InvertRecruitmentPageVisibility()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            if (_isVisible)
            {
                mainWindow.recruitment_page.Visibility = Visibility.Hidden;
                _isVisible = false;
            }
            else
            {
                mainWindow.recruitment_page.Visibility = Visibility.Visible;
                _isVisible = true;
            }

        }
    }
}
