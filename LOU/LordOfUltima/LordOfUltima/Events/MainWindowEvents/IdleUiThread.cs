using System;
using System.Diagnostics;
using System.Threading;
using LordOfUltima.Error;
using LordOfUltima.RessourcesProduction;
using LordOfUltima.Score;

namespace LordOfUltima.Events
{
    class IdleUiThread
    {
        private static IdleUiThread _instance;
        public static IdleUiThread Instance
        {
            get { return _instance ?? (_instance = new IdleUiThread()); }
        }

        private readonly Stopwatch _stopwatch;
        private IdleUiThread()
        {
            // Start Stop watch
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void IdleThreadWork(object sender, EventArgs e)
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            //do your idle stuff here
            Thread.Sleep(10);
            // Show stopwatch in menu :D
            TimeSpan ts = _stopwatch.Elapsed;

            // Update Stop Watch
            string time = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            mainWindow.stop_watch.Content = time;

            // Update Chat
            //ChatEvents.Instance.ui_thread_updateChat();

            // Update ressources
            RessourcesManager.Instance.UpdateRessourceUi();

            // Update error
            ErrorManager.Instance.DispatchErrors();

            // Update Score
            ScoreManager.Instance.UpdateVisual();
        }
    }
}
