using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LordOfUltima.Research.Element;
using Label = System.Windows.Controls.Label;

namespace LordOfUltima.Research
{
    class ResearchHandler
    {
        private static ResearchHandler _instanceHandler;
        public static ResearchHandler Instance => _instanceHandler ?? (_instanceHandler = new ResearchHandler());

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

            // Set Wood research informations
            updateResearchInformations(WoodResearchType);
        }

        public void UpdateResearch(IResearchType researchType)
        {
            // If you have enough ressources
            researchType.SetLevel(researchType.GetLevel() + 1);

            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            updateResearchInformations(WoodResearchType);
        }

        private void updateResearchInformations(IResearchType researchType)
        {
            setPath(researchType.GetImageRectangle(), researchType, researchType.GetLevel() + 1);
            setBonus(researchType.GetCurrentBonusLabel(), researchType, researchType.GetLevel());
            setNextBonus(researchType.GetNextBonusLabel(), researchType, researchType.GetLevel());

            if(researchType.GetLevel() == researchType.GetMaxLevel())
                researchType.GetResearchButton().Visibility = Visibility.Hidden;
        }


        private void setPath(Rectangle rectangle, IResearchType researchType, int level)
        {
            if (level <= 0 || level > researchType.GetMaxLevel())
                return;

            var path = researchType.GetElementPath(level);
            rectangle.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(@path, UriKind.RelativeOrAbsolute)) };
        }

        private void setBonus(Label label, IResearchType researchType, int level)
        {
            if (level > 0 && level <= researchType.GetMaxLevel())
            {
                label.Content = researchType.GetResearchBonus(level).GetFirstBonus() + " %";
            }
        }

        private void setNextBonus(Label label, IResearchType researchType, int level)
        {
            if (level >= 0 && level < researchType.GetMaxLevel())
            {
                label.Content = researchType.GetResearchBonus(level + 1).GetFirstBonus() + " %";
            }
            else
            {
                label.Content = "No research bonus next";
            }
        }

    }
}
