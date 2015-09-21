using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LordOfUltima.Error;
using LordOfUltima.Events;
using LordOfUltima.Research.Element;
using LordOfUltima.RessourcesProduction;
using Label = System.Windows.Controls.Label;

namespace LordOfUltima.Research
{
    class ResearchHandler
    {
        private static ResearchHandler _instanceHandler;
        public static ResearchHandler Instance
        { get{return _instanceHandler ?? (_instanceHandler = new ResearchHandler()); } }

        public readonly IResearchType WoodResearchType;
        public readonly IResearchType StoneResearchType;
        public readonly IResearchType IronResearchType;
        public readonly IResearchType FoodResearchType;
        public readonly IResearchType GoldResearchType;

        public ResearchHandler()
        {
            WoodResearchType = new WoodResearch();
            StoneResearchType = new StoneResearch();
            IronResearchType = new IronResearch();
            FoodResearchType = new FoodResearch();
            GoldResearchType = new GoldResearch();
        }

        public void Initialise()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            // Set Wood research informations
            updateResearchInformations(WoodResearchType);

            // Set Stone research informations
            updateResearchInformations(StoneResearchType);

            // Set Iron research informations
            updateResearchInformations(IronResearchType);

            // Set Food research informations
            updateResearchInformations(FoodResearchType);

            // Set Gold research informations
            updateResearchInformations(GoldResearchType);
        }

        public void UpdateResearch(IResearchType researchType)
        {
            // If you have enough ressources
            if (!BuyResearch.Instance.Buy(researchType))
            {
                ErrorManager.Instance.AddError(new Error.Error() { Description = Error.Error.Type.NOT_ENOUGH_RESSOURCES_RESEARCH });
                return;
            } 

            // Buy sucessfull
            researchType.SetLevel(researchType.GetLevel() + 1);

            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            updateResearchInformations(researchType);

            // update ressource production with new research
            RessourcesManager.Instance.CalculateRessources();
        }

        private void updateResearchInformations(IResearchType researchType)
        {
            setPath(researchType.GetImageRectangle(), researchType, researchType.GetLevel() + 1);
            setBonus(researchType.GetCurrentBonusLabel(), researchType, researchType.GetLevel());
            setNextBonus(researchType.GetNextBonusLabel(), researchType, researchType.GetLevel());
            setResearchCost(researchType);

            if (researchType.GetLevel() == researchType.GetMaxLevel())
            {
                researchType.GetResearchButton().Visibility = Visibility.Hidden;
                researchType.GetCostCanvas().Visibility = Visibility.Hidden;
            }    
        }

        private void setPath(Rectangle rectangle, IResearchType researchType, int level)
        {
            // max level
            if (level == researchType.GetMaxLevel() + 1)
                level--;

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

        private void setResearchCost(IResearchType researchType)
        {
            var level = researchType.GetLevel();

            if (level >= 0 && level < researchType.GetMaxLevel())
            {
                var researchCost = researchType.GetResearchCost(level + 1);
                researchType.GetResearchCostLabel().Content = researchCost.Research;
                researchType.GetGoldCostLabel().Content = researchCost.Gold;
                researchType.GetRessourceCostLabel().Content = researchCost.GetFirstCost();
            }
        }

    }
}
