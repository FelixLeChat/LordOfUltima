using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LordOfUltima.Error;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Units.Units
{
    class RecruitmentManager
    {
        private static RecruitmentManager _ins;
        private Dictionary<UnitEntity, int> _recruitmentCount = new Dictionary<UnitEntity, int>();
        private UnitCost totalUnitCost = new UnitCost();
        private bool _canRecruit = true;

        public static RecruitmentManager Instance
        {
            get { return _ins ?? (_ins = new RecruitmentManager());}
        }

        private RecruitmentManager()
        {
            // All unit that exist
            var units = UnitManager.Instance.Units;

            foreach (var unit in units)
            {
                _recruitmentCount.Add(unit.Key, 0);
            }
        }

        /// <summary>
        /// Increase count of selected units to recruit and update visual to recruit
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="entity"></param>
        /// <param name="count"></param>
        public void IncrCount(TextBox textbox, UnitEntity entity, int count)
        {
            var unit = UnitManager.Instance.Units[entity];
            var space = unit.GetUnitStats().Space;
            var max = UnitManager.Instance.TotalUnits/space;

            _recruitmentCount[entity]++;
            if (_recruitmentCount[entity] > max)
                _recruitmentCount[entity] = max;

            textbox.Text = _recruitmentCount[entity].ToString();
            UpdateTotalTroupsCost();
        }

        /// <summary>
        /// Decrease count for selected units to recruit and update visual to recruit
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="entity"></param>
        /// <param name="count"></param>
        public void DecrCount(TextBox textbox, UnitEntity entity, int count)
        {
            if (count <= 0)
                return;

            _recruitmentCount[entity]--;
            textbox.Text = _recruitmentCount[entity].ToString();

            UpdateTotalTroupsCost();
        }

        /// <summary>
        /// Update Unit recruited
        /// </summary>
        public void UpdateCurrentUnitCount()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            var unitCounts = UnitManager.Instance.UnitsAvailables;

            // CityGuard
            UpdateTroop(mainWindow.cityguard_recruitment_limit, unitCounts[UnitEntity.Cityguard]);

            // Berserker
            UpdateTroop(mainWindow.berserker_recruitment_limit, unitCounts[UnitEntity.Berserker]);

            // Crossbow
            UpdateTroop(mainWindow.crossbow_recruitment_limit, unitCounts[UnitEntity.Crossbow]);

            // Guardian
            UpdateTroop(mainWindow.guardian_recruitment_limit, unitCounts[UnitEntity.Guardian]);

            // knight
            UpdateTroop(mainWindow.knight_recruitment_limit, unitCounts[UnitEntity.Knight]);

            // Mage
            UpdateTroop(mainWindow.mage_recruitment_limit, unitCounts[UnitEntity.Mage]);

            // Paladin
            UpdateTroop(mainWindow.paladin_recruitment_limit, unitCounts[UnitEntity.Paladin]);

            // Ranger
            UpdateTroop(mainWindow.ranger_recruitment_limit, unitCounts[UnitEntity.Ranger]);

            // Scout
            UpdateTroop(mainWindow.scout_recruitment_limit, unitCounts[UnitEntity.Scout]);

            // Templar
            UpdateTroop(mainWindow.templar_recruitment_limit, unitCounts[UnitEntity.Templar]);

            // Warlock
            UpdateTroop(mainWindow.warlock_recruitment_limit, unitCounts[UnitEntity.Warlock]);
        }
        private void UpdateTroop(Label label, int count)
        {
            label.Content = count;
        }

        /// <summary>
        /// Recruit selected units if they don't exceed ressources
        /// </summary>
        public void Recruit()
        {
            if (!_canRecruit)
            {
                ErrorManager.Instance.AddError(new Error.Error()
                {
                    Description = Error.Error.Type.RECRUITMENT_RESSOURCES_CAP
                });
                return;
            }
            foreach (var recruitmentInfo in _recruitmentCount)
            {
                Buy(recruitmentInfo.Key, recruitmentInfo.Value);
            }

            // reset field
            foreach (var i in _recruitmentCount.Keys.ToList())
            {
                _recruitmentCount[i] = 0;
            }
            ResetAllFields();
            UpdateTotalTroupsCost();

            UpdateCurrentUnitCount();
        }

        /// <summary>
        /// Buy selected units (increment count and decrement ressources)
        /// </summary>
        /// <param name="unitEntity"></param>
        /// <param name="count"></param>
        private void Buy(UnitEntity unitEntity, int count)
        {
            var cost = UnitManager.Instance.Units[unitEntity].GetUnitCost();

            for (int i = 0; i < count; i++)
            {
                // when no more ressources
                if (!CheckRessourcesAvailability(cost))
                {
                    ErrorManager.Instance.AddError(new Error.Error()
                    {
                        Description = Error.Error.Type.RECRUITMENT_RESSOURCES_CAP
                    });
                    return;
                }
                UnitManager.Instance.UnitsAvailables[unitEntity]++;
            }
        }

        private bool CheckRessourcesAvailability(UnitCost cost)
        {
            Ressources ressources = Ressources.Instance;

            if (ressources.WoodQty >= cost.Wood &&
                ressources.StoneQty >= cost.Stone &&
                ressources.IronQty >= cost.Iron &&
                ressources.FoodQty >= cost.Food &&
                ressources.GoldQty >= cost.Gold)
            {
                ressources.WoodQty -= (cost.Wood > 0) ? cost.Wood : 0;
                ressources.StoneQty -= (cost.Stone > 0) ? cost.Stone : 0;
                ressources.IronQty -= (cost.Iron > 0) ? cost.Iron : 0;
                ressources.FoodQty -= (cost.Food > 0) ? cost.Food : 0;
                ressources.GoldQty -= (cost.Gold > 0) ? cost.Gold : 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check fields when a value was entered to see if they are more than the maximum allowed
        /// </summary>
        public void CheckAllFields()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            CheckField(UnitEntity.Cityguard, mainWindow.cityguard_recruitment_count);
            CheckField(UnitEntity.Berserker, mainWindow.berserker_recruitment_count);
            CheckField(UnitEntity.Crossbow, mainWindow.crossbow_recruitment_count);
            CheckField(UnitEntity.Guardian, mainWindow.guardian_recruitment_count);
            CheckField(UnitEntity.Knight, mainWindow.knight_recruitment_count);
            CheckField(UnitEntity.Mage, mainWindow.mage_recruitment_count);
            CheckField(UnitEntity.Paladin, mainWindow.paladin_recruitment_count);
            CheckField(UnitEntity.Ranger, mainWindow.ranger_recruitment_count);
            CheckField(UnitEntity.Scout, mainWindow.scout_recruitment_count);
            CheckField(UnitEntity.Templar, mainWindow.templar_recruitment_count);
            CheckField(UnitEntity.Warlock, mainWindow.warlock_recruitment_count);

            UpdateTotalTroupsCost();
        }
        private void CheckField(UnitEntity unitEntity, TextBox textBox)
        {
            var unit = UnitManager.Instance.Units[unitEntity];
            var space = unit.GetUnitStats().Space;
            var max = UnitManager.Instance.TotalUnits / space;

            int value = int.Parse(textBox.Text);

            _recruitmentCount[unitEntity] = value;

            if (value > max)
            {
                _recruitmentCount[unitEntity] = max;
            }

            textBox.Text = _recruitmentCount[unitEntity].ToString();
        }

        /// <summary>
        /// Set all fields for recruitment at 0
        /// </summary>
        private void ResetAllFields()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.cityguard_recruitment_count.Text = "0";
            mainWindow.berserker_recruitment_count.Text = "0";
            mainWindow.crossbow_recruitment_count.Text = "0";
            mainWindow.guardian_recruitment_count.Text = "0";
            mainWindow.knight_recruitment_count.Text = "0";
            mainWindow.mage_recruitment_count.Text = "0";
            mainWindow.paladin_recruitment_count.Text = "0";
            mainWindow.ranger_recruitment_count.Text = "0";
            mainWindow.scout_recruitment_count.Text = "0";
            mainWindow.templar_recruitment_count.Text = "0";
            mainWindow.warlock_recruitment_count.Text = "0";
        }

        /// <summary>
        /// Update visual for cost of recruiting current selected units
        /// </summary>
        private void UpdateTotalTroupsCost()
        {
            totalUnitCost = new UnitCost();
            var totalSpaceNeeded = 0;
            _canRecruit = true;

            var remaining = UnitManager.Instance.TotalUnits;
            var unitCounts = UnitManager.Instance.UnitsAvailables;

            // Get remaining units to recruit
            remaining = unitCounts.Aggregate(remaining, (current, unitCount) => current - unitCount.Value);

            foreach (var recruitment in _recruitmentCount)
            {
                var unit = UnitManager.Instance.Units[recruitment.Key];
                var unitCost = unit.GetUnitCost();
                var count = recruitment.Value;

                totalUnitCost.Gold += unitCost.Gold*count;
                totalUnitCost.Iron += unitCost.Iron*count;
                totalUnitCost.Wood += unitCost.Wood*count;

                totalSpaceNeeded += unit.GetUnitStats().Space*count;
            }

            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            var totalRessources = Ressources.Instance;
            var redbrush = new SolidColorBrush(Colors.Red);
            var defaultBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#443025"));

            mainWindow.total_wood_recruitment.Content = totalUnitCost.Wood;
            mainWindow.total_wood_recruitment.Foreground = defaultBrush;
            if (totalUnitCost.Wood > totalRessources.WoodQty)
            {
                _canRecruit = false;
                mainWindow.total_wood_recruitment.Foreground = redbrush;
            }      

            mainWindow.total_iron_recruitment.Content = totalUnitCost.Iron;
            mainWindow.total_iron_recruitment.Foreground = defaultBrush;
            if (totalUnitCost.Iron > totalRessources.IronQty)
            {
                _canRecruit = false;
                mainWindow.total_iron_recruitment.Foreground = redbrush;
            } 

            mainWindow.total_gold_recruitment.Content = totalUnitCost.Gold;
            mainWindow.total_gold_recruitment.Foreground = defaultBrush;
            if (totalUnitCost.Gold > totalRessources.GoldQty)
            {
                _canRecruit = false;
                mainWindow.total_gold_recruitment.Foreground = redbrush;
            }  

            mainWindow.total_space_recruitment.Content = totalSpaceNeeded + "/" + remaining;
            mainWindow.total_space_recruitment.Foreground = defaultBrush;
            if (totalSpaceNeeded > remaining)
            {
                _canRecruit = false;
                mainWindow.total_space_recruitment.Foreground = redbrush;
            }
        }
    }
}