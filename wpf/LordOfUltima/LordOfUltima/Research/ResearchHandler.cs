using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LordOfUltima.Research.Element;

namespace LordOfUltima.Research
{
    class ResearchHandler
    {
        private static ResearchHandler _instanceHandler;
        public static ResearchHandler Instance
        {
            get { return _instanceHandler ?? (_instanceHandler = new ResearchHandler()); }
        }

        public readonly IResearchType WoodResearchType;
        public ResearchHandler()
        {
            WoodResearchType = new WoodResearch();
        }

        public void Initialise()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;
            setPath(mainWindow.research_wood_image, WoodResearchType, 1);
        }

        public void UpdateResearch(IResearchType researchType)
        {
            // If you have enough ressources
            researchType.SetLevel(researchType.GetLevel() + 1);
            //setPath();
        }

        private void setPath(Rectangle rectangle, IResearchType researchType, int level)
        {
            var path = researchType.GetElementPath(level);
            rectangle.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(@path, UriKind.RelativeOrAbsolute)) };
        }
    }
}
