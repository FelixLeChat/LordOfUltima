using System.Collections.Generic;
using LordOfUltima.Units.Units;

namespace LordOfUltima.Units
{
    class UnitManager
    {
        private UnitManager _ins;

        public UnitManager Instance
        {
            get { return _ins ?? (_ins = new UnitManager()); }
        }
    }
}
