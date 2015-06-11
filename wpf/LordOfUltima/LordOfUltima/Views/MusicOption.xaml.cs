using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LordOfUltima.Music;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MusicOption.xaml
    /// </summary>
    public partial class MusicOption
    {
        public MusicOption()
        {
            InitializeComponent();

            // Set instance
            Instance = this;

            window_background.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Media/options.png", UriKind.Relative)) };

            // load music from directory
            string[] filePaths = Directory.GetFiles(@"Media/audio");
            foreach (var file in filePaths)
            {
                if (file.Contains(".mp3"))
                {
                    var split = file.Split('\\');
                    music_choices.Items.Add(split[split.Length-1]);
                }
            }

            // update mutes checkbox
            mute.IsChecked = MusicPlayer.Instance.IsMuted;

            // Set volume value to current value
            volume.Maximum = 100;
            volume.Value = Math.Round(MusicPlayer.Instance.Volume);

            music_choices.Text = MusicPlayer.Instance.SongName;
            // disable apply button
            apply_button.IsEnabled = false;


        }

        public static MusicOption Instance { get; private set; }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instance = null;
        }

        private void music_choices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            apply_button.IsEnabled = true;
        }

        private void mute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            apply_button.IsEnabled = true;
        }

        private void cancel_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void volume_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            apply_button.IsEnabled = true;
            volume_value.Content = Math.Round(volume.Value);
        }
        /*
         * Apply the selected settings to the game sound
         */
        private void apply_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MusicPlayer.Instance.UpdateSound(music_choices.Text, mute.IsChecked, volume.Value);
            apply_button.IsEnabled = false;
        }
    }
}
