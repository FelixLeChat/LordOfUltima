using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using LordOfUltima.Error;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Units.Units
{
    class RecruitmentManager
    {
        private static RecruitmentManager _ins;
        private readonly Dictionary<UnitEntity, RecruitmentInfo> _recruitmentCount = new Dictionary<UnitEntity, RecruitmentInfo>();

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
                _recruitmentCount.Add(unit.Key, new RecruitmentInfo());
            }
        }

        public void IncrCount(TextBox textbox, UnitEntity entity, int count)
        {
            var info = _recruitmentCount[entity];
            var unit = UnitManager.Instance.Units[entity];
            var space = unit.GetUnitStats().Space;
            var max = UnitManager.Instance.TotalUnits/space;

            info.SelectedCount = count + 1;
            if (info.SelectedCount > max)
                info.SelectedCount = max;

            _recruitmentCount[entity] = info;

            textbox.Text = info.SelectedCount.ToString();
        }

        public void DecrCount(TextBox textbox, UnitEntity entity, int count)
        {
            if (count <= 0)
                count = 1;

            var info = _recruitmentCount[entity];
            info.SelectedCount = count - 1;
            _recruitmentCount[entity] = info;

            textbox.Text = info.SelectedCount.ToString();
        }

        public void UpdateCurrentUnitCount()
        {
            var remaining = UnitManager.Instance.TotalUnits;
            var unitCounts = UnitManager.Instance.UnitsAvailables;

            // Get remaining units to recruit
            remaining = unitCounts.Aggregate(remaining, (current, unitCount) => current - unitCount.Value);

            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            // CityGuard
            UpdateTroop(mainWindow.cityguard_recruitment_limit, UnitEntity.Cityguard, unitCounts[UnitEntity.Cityguard], remaining);

            // Berserker
            UpdateTroop(mainWindow.berserker_recruitment_limit, UnitEntity.Berserker, unitCounts[UnitEntity.Berserker], remaining);

            // Crossbow
            UpdateTroop(mainWindow.crossbow_recruitment_limit, UnitEntity.Crossbow, unitCounts[UnitEntity.Crossbow], remaining);

            // Guardian
            UpdateTroop(mainWindow.guardian_recruitment_limit, UnitEntity.Guardian, unitCounts[UnitEntity.Guardian], remaining);

            // knight
            UpdateTroop(mainWindow.knight_recruitment_limit, UnitEntity.Knight, unitCounts[UnitEntity.Knight], remaining);

            // Mage
            UpdateTroop(mainWindow.mage_recruitment_limit, UnitEntity.Mage, unitCounts[UnitEntity.Mage], remaining);

            // Paladin
            UpdateTroop(mainWindow.paladin_recruitment_limit, UnitEntity.Paladin, unitCounts[UnitEntity.Paladin], remaining);

            // Ranger
            UpdateTroop(mainWindow.ranger_recruitment_limit, UnitEntity.Ranger, unitCounts[UnitEntity.Ranger], remaining);

            // Scout
            UpdateTroop(mainWindow.scout_recruitment_limit, UnitEntity.Scout, unitCounts[UnitEntity.Scout], remaining);

            // Templar
            UpdateTroop(mainWindow.templar_recruitment_limit, UnitEntity.Templar, unitCounts[UnitEntity.Templar], remaining);

            // Warlock
            UpdateTroop(mainWindow.warlock_recruitment_limit, UnitEntity.Warlock, unitCounts[UnitEntity.Warlock], remaining);
        }

        public void Recruit()
        {
            foreach (var recruitmentInfo in _recruitmentCount)
            {
                Buy(recruitmentInfo.Key, recruitmentInfo.Value.SelectedCount);
            }
        }

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

        private void UpdateTroop(Label label, UnitEntity unitEntity, int count, int remaining)
        {
            var unit = UnitManager.Instance.Units[unitEntity];
            var space = unit.GetUnitStats().Space;

            label.Content = count + "/" + (remaining/space);
        }

        public void CheckAllFields()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            CheckField(UnitEntity.Cityguard, mainWindow.cityguard_recruitment_count);
        }

        private void CheckField(UnitEntity unitEntity, TextBox textBox)
        {
            var unit = UnitManager.Instance.Units[unitEntity];
            var space = unit.GetUnitStats().Space;
            var max = UnitManager.Instance.TotalUnits / space;

            int value = int.Parse(textBox.Text);

            if (value > max)
            {
                textBox.Text = max.ToString();
                return;
            }

            textBox.Text = value.ToString();
        }
    }
}