using LordOfUltima.MGameboard;

namespace LordOfUltima.Units.Units
{
    abstract class Unit
    {
        protected UnitStats UnitStats;
        protected string ImagePath;
        protected bool Available;
        protected string Name;
        protected UnitType UnitType;
        protected UnitEntity UnitEntity;
        protected UnitCost UnitCost;

        public UnitStats GetUnitStats()
        {
            return UnitStats;
        }

        public string GetImagePath()
        {
            return ImagePath;
        }

        public string GetName()
        {
            return Name;
        }

        public bool GetAvailable()
        {
            return Available;
        }

        public void SetAvailability(bool availability)
        {
            Available = availability;
        }

        public UnitType GetUnitType()
        {
            return UnitType;
        }

        public UnitEntity GetUnitEntity()
        {
            return UnitEntity;
        }

        public UnitCost GetUnitCost()
        {
            return UnitCost;
        }
    }
}
