using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LordOfUltima.Score
{
    class Score
    {
        private static Score _instanceScore;
        public static Score Instance
        {
            get { return _instanceScore ?? (_instanceScore = new Score()); }
        }

        public int ScoreValue { get; set; }

        public string GetTitle()
        {
            if (ScoreValue < 0)
                return "";
            if (ScoreValue < 100)
                return "Sir";
            if (ScoreValue < 200)
                return "Knight";
            if (ScoreValue < 500)
                return "Baron";
            if (ScoreValue < 2000)
                return "Earl";
            if (ScoreValue < 5000)
                return "Marquess";
            if (ScoreValue < 10000)
                return "Prince";
            if (ScoreValue < 20000)
                return "Duke";
            if (ScoreValue < 50000)
                return "King";

            return "Emperor";
        }

        public ImageBrush GetCity()
        {
            var imgUrl = "pack://application:,,,/Resources/Images/Town/town_";

            if (ScoreValue < 100)
                imgUrl += "1.png";
            else if (ScoreValue < 200)
                imgUrl += "2.png";
            else if (ScoreValue < 400)
                imgUrl += "3.png";
            else if (ScoreValue < 800)
                imgUrl += "4.png";
            else if (ScoreValue < 1500)
                imgUrl += "5.png";
            else if (ScoreValue < 2000)
                imgUrl += "6.png";
            else if (ScoreValue < 3000)
                imgUrl += "7.png";
            else
                imgUrl += "8.png";

            return new ImageBrush { ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.RelativeOrAbsolute)) };
        }
    }
}
