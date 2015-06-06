using System.Threading;
using System.Windows.Input;

namespace LordOfUltima.Events
{
    class ChatEvents
    {
        private static ChatEvents m_ins;
        public static ChatEvents Instance
        {
            get
            {
                if (m_ins == null)
                {
                    m_ins = new ChatEvents();
                }
                return m_ins;
            }
        }

        public void ChatKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               /* Thread thread = new Thread(new ParameterizedThreadStart(m_chat_ins.insertNewChatLine));
                thread.Start(chat_text.Text);
                chat_text.Text = "";*/
            }
        }
    }
}
