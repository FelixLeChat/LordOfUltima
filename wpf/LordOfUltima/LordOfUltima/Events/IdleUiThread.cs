using System;
using System.Diagnostics;
using System.Threading;

namespace LordOfUltima.Events
{
    class IdleUiThread
    {
        private static IdleUiThread _instance;
        public static IdleUiThread Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IdleUiThread();
                }
                return _instance;
            }
        }

        private Stopwatch m_watch;
        private IdleUiThread()
        {
            // Start Stop watch
            m_watch = new Stopwatch();
            m_watch.Start();
        }

        public void IdleThreadWork(object sender, EventArgs e)
        {
            //do your idle stuff here
            Thread.Sleep(10);
            // Show stopwatch in menu :D
            TimeSpan ts = m_watch.Elapsed;

            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            if (MainWindow.m_ins != null)
            {
                // Update Stop Watch
                MainWindow.m_ins.InsertContentInStopWatch(time);

                // Update Chat
                MainWindow.m_ins.ui_thread_updateChat();
            }
        }
    }
}
