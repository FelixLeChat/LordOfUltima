namespace LordOfUltima.Units.Units
{
    interface IUnit
    {
        UnitStats GetUnitStats();
        string GetImagePath();
        string GetName();
        bool GetAvailable();
        void SetAvailability(bool availability);
    }
}
