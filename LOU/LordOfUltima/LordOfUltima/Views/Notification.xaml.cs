using System;
using System.Windows;
using System.Windows.Threading;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour Notification.xaml
    /// </summary>
    public partial class Notification
    {
        private static Window _instance;
        public static Window Instance
        { get { return _instance;} }


        public Notification()
        {
            InitializeComponent();
            _instance = this;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = SystemParameters.WorkArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth - 100;
                Top = corner.Y - ActualHeight;
            }));
  
        }

        private void DoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _instance = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
