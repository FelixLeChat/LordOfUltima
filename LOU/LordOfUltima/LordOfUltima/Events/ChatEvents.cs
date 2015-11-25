using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using LordOfUltima.Web;

namespace LordOfUltima.Events
{
    class ChatEvents
    {
        private static ChatEvents _chatEvents;
        public static ChatEvents Instance
        {
            get { return _chatEvents ?? (_chatEvents = new ChatEvents()); }
        }

        public const int ChatboxMaxItems = 30;
        private bool _stopChatUpdate = false;
        private readonly Chat _chatIns = Chat.Instance;
        private List<string> _newChatLines = new List<string>();
        private readonly Object _lockChat = new Object();
        private readonly List<string> _chatText = new List<string>();

        public void ChatKeyDown(object sender, KeyEventArgs e)
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            if (e.Key == Key.Enter)
            {
                string chatContent = mainWindow.chat_text.Text;

                if (chatContent.Length == 0)
                    return;

                // run "InsertNewChatLine" on a new thread
                Thread thread = new Thread(_chatIns.InsertNewChatLine);
                // Send the chat line that is on the chat textbox
                thread.Start(chatContent);
                mainWindow.chat_text.Text = "";
            }
        }

        public void UpdateChat(object sender, DoWorkEventArgs e)
        {
            while (!_stopChatUpdate)
            {
                // update each 1 seconds
                Thread.Sleep(1000);

                if (!_chatIns.IsInit)
                {
                    _chatIns.InitChat();
                }
                else
                {
                    lock (_lockChat)
                    {
                        _newChatLines = _chatIns.GetLastChatString();
                    }
                }
            }
        }

        public void ui_thread_updateChat()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            if (_newChatLines != null && _newChatLines.Count != 0)
            {
                lock (_lockChat)
                {
                    _newChatLines.ForEach(delegate(String chatText)
                    {
                        _chatText.Add(chatText);
                    });
                    _newChatLines.Clear();
                }
                mainWindow.textbox_scroll_viewer.ScrollToBottom();

                // limit the number in the chat box
                if (_chatText.Count > ChatboxMaxItems)
                {
                    _chatText.RemoveAt(0);
                }
                string text = String.Join(Environment.NewLine, _chatText);
                mainWindow.chat_textbox.Text = text;
            }
        }
    }
}
