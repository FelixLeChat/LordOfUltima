namespace LordOfUltima.Research
{
    interface IUnitBuilding
    {
        int GetArmySize(int level);
        bool IsBarrack();
        int GetUnitBonus(int level);
    }
}

