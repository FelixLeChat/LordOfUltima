using System.Windows.Controls;
using Label = System.Windows.Controls.Label;
using System.Windows.Shapes;

namespace LordOfUltima.Research
{
    interface IResearchType
    {
        string GetElementPath(int level);

        ResearchCost GetResearchCost(int level);

        ResearchBonus GetResearchBonus(int level);

        int GetLevel();
        int SetLevel(int level);
        int GetMaxLevel();

        Label GetCurrentBonusLabel();
        Label GetNextBonusLabel();
        Rectangle GetImageRectangle();
        Button GetResearchButton();

        Canvas GetCostCanvas();
        Label GetResearchCostLabel();
        Label GetGoldCostLabel();
        Label GetRessourceCostLabel();
    }
}
