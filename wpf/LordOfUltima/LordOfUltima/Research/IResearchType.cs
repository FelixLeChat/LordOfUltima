namespace LordOfUltima.Research
{
    interface IResearchType
    {
        string GetElementPath(int level);

        ResearchCost GetResearchCost(int level);

        ResearchBonus GetResearchBonus(int level);

        int GetLevel();
        int SetLevel(int level);
    }
}
