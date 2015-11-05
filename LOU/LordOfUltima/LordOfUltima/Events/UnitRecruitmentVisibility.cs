using System.Windows.Controls;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class UnitRecruitmentVisibility
    {
        private static UnitRecruitmentVisibility _ins;
        public static UnitRecruitmentVisibility Instance
        { get { return _ins ?? (_ins = new UnitRecruitmentVisibility()); } }

        public void DisableAllUnits()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            DisableCanvas(mainWindow.recruit_cityguard);
            DisableCanvas(mainWindow.recruit_berserker);
            DisableCanvas(mainWindow.recruit_crossbow);
            DisableCanvas(mainWindow.recruit_guardian);
            DisableCanvas(mainWindow.recruit_knight);
            DisableCanvas(mainWindow.recruit_paladin);
            DisableCanvas(mainWindow.recruit_templar);
            DisableCanvas(mainWindow.recruit_ranger);
            DisableCanvas(mainWindow.recruit_scout);
            DisableCanvas(mainWindow.recruit_mage);
            DisableCanvas(mainWindow.recruit_warlock);
        }

        private void DisableCanvas(Canvas canvas)
        {
            canvas.IsEnabled = false;
            canvas.Background.Opacity = 0.5;
        }

        private void EnableCanvas(Canvas canvas)
        {
            canvas.IsEnabled = true;
            canvas.Background.Opacity = 1;
        }

        public void UpdateVisibility()
        {
            Element[,] elementMap = Gameboard.Instance.GetMap();

            foreach (var element in elementMap)
            {
                UpdateVisibility(element);
            }
        }

        public void UpdateVisibility(Element element)
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            if (element?.GetElementType() == null)
                return;

            switch (element.GetElementType().GetElementType())
            {
                case ElementType.Type.BUILDING_CITYGUARD_HOUSE:
                    EnableCanvas(mainWindow.recruit_cityguard);
                    break;

                case ElementType.Type.BUILDING_MOONGLOW_TOWER:
                    EnableCanvas(mainWindow.recruit_mage);
                    EnableCanvas(mainWindow.recruit_warlock);
                    break;
                
                case ElementType.Type.BUILDING_STABLE:
                    EnableCanvas(mainWindow.recruit_crossbow);
                    EnableCanvas(mainWindow.recruit_knight);
                    EnableCanvas(mainWindow.recruit_paladin);
                    break;

                case ElementType.Type.BUILDING_TRAINING_GROUND:
                    EnableCanvas(mainWindow.recruit_berserker);
                    EnableCanvas(mainWindow.recruit_guardian);
                    EnableCanvas(mainWindow.recruit_ranger);
                    EnableCanvas(mainWindow.recruit_templar);
                    break;
            }
        }
    }
}
