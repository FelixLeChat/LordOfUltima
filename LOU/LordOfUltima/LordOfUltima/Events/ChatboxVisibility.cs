using System.Windows;

namespace LordOfUltima.Events
{
    class ChatboxVisibility
    {
        private static ChatboxVisibility _instance;

        public static ChatboxVisibility Instance
        { get{return _instance ?? (_instance = new ChatboxVisibility()); } }

        public void MinimizeChatbox()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.chat_box.Visibility = Visibility.Collapsed;
            mainWindow.chat_box_open.Visibility = Visibility.Visible;
            Properties.Settings.Default.ChatMinimized = true;
        }

        public void OpenChatbox()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.chat_box.Visibility = Visibility.Visible;
            mainWindow.chat_box_open.Visibility = Visibility.Collapsed;
            Properties.Settings.Default.ChatMinimized = false;
        }

        public void HandleChatboxVisibility()
        {
            if(Properties.Settings.Default.ChatMinimized)
                MinimizeChatbox();
            else
                OpenChatbox();
        }
    }
}