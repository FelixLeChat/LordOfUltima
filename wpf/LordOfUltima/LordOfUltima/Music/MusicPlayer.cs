using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LordOfUltima.Music
{
    class MusicPlayer
    {
        private static MusicPlayer _instance;
        public static MusicPlayer Instance
        { get { return _instance ?? (_instance = new MusicPlayer()); } }

        private MusicPlayer()
        {
            Volume = 100;
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        
        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);


        private bool _isPlaying;
        public bool IsMuted { get; set; }
        public double Volume { get; set; }
        public string SongName { get; set; }

        private bool Open(string file)
        {
            if (File.Exists(file))
            {
                string command = "open \"" + file + "\" type MPEGVideo alias MyMp3";
                mciSendString(command, null, 0, 0);
                return true;
            }
            return false;
        }

        public void Play(string file)
        {
            if (Open(file))
            {
                const string command = "play MyMp3 REPEAT";
                mciSendString(command, null, 0, 0);
                _isPlaying = true;
            }
        }

        public void Stop()
        {
            if (_isPlaying)
            {
                 string command = "stop MyMp3";
                mciSendString(command, null, 0, 0);

                command = "close MyMp3";
                mciSendString(command, null, 0, 0);               
            }
        }

        public void UpdateSound(string filename, bool? muted, double volume)
        {
            // Mute handler
            IsMuted = (muted.HasValue) && muted.Value;
            string command = "setaudio MyMp3 " + ((IsMuted)? "off":"on");
            mciSendString(command, null, 0, 0);

            // volume handler
            Volume = volume;
            double newVolume = ushort.MaxValue * volume / 100.0;
            uint v = ((uint) newVolume) & 0xffff;
            uint vAll = v | (v << 16);
            WaveOutSetVolume(IntPtr.Zero, vAll);

            // music played handler
            if (filename == SongName) return;
            Stop();
            Play("Resources/audio/" + filename);
            SongName = filename;
        }
    }
}
