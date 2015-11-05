using System.Collections.Generic;
using System.Windows.Controls;

namespace LordOfUltima.Units.Units
{
    class RecruitmentManager
    {
        private static RecruitmentManager _ins;

        public static RecruitmentManager Instance
        {
            get { return _ins ?? (_ins = new RecruitmentManager());}
        }

        private readonly Dictionary<UnitEntity, Unit> _units = new Dictionary<UnitEntity, Unit>();
        private readonly Dictionary<UnitEntity, RecruitmentInfo> _recruitmentCount = new Dictionary<UnitEntity, RecruitmentInfo>();

        RecruitmentManager()
        {
            _units.Add(UnitEntity.Berserker, new BerserkerUnit());
            _units.Add(UnitEntity.Cityguard, new CityguardUnit());
            _units.Add(UnitEntity.Crossbow, new CrossbowUnit());
            _units.Add(UnitEntity.Guardian, new GuardianUnit());
            _units.Add(UnitEntity.Knight, new KnightUnit());
            _units.Add(UnitEntity.Mage, new MageUnit());
            _units.Add(UnitEntity.Paladin, new PaladinUnit());
            _units.Add(UnitEntity.Ranger, new RangerUnit());
            _units.Add(UnitEntity.Scout, new ScoutUnit());
            _units.Add(UnitEntity.Templar, new TemplarUnit());
            _units.Add(UnitEntity.Warlock, new WarlockUnit());

            foreach (var unit in _units)
            {
                _recruitmentCount.Add(unit.Key, new RecruitmentInfo());
            }
        }

        public void IncrCount(TextBox textbox, UnitEntity entity, int count)
        {
            var info = _recruitmentCount[entity];
            var max = info.MaxSelected;

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
    }
}