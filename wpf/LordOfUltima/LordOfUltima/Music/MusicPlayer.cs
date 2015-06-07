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

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);

        public bool Open(string file)
        {
            if (File.Exists(file))
            {
                string command = "open \"" + file + "\" type MPEGVideo alias MyMp3";
                mciSendString(command, null, 0, 0);
                return true;
            }
            return false;
        }

        public void Play()
        {
            const string command = "play MyMp3 REPEAT";
            mciSendString(command, null, 0, 0);
        }

        public void Stop()
        {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, 0);

            command = "close MyMp3";
            mciSendString(command, null, 0, 0);
        }
    }
}
