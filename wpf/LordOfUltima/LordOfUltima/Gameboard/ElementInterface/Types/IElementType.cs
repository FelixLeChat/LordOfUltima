namespace LordOfUltima.MGameboard
{
    public interface IElementType
    {
        string GetImagePath();
        string GetDetailImagePath();
        string GetElementInfo();
        string Name();
        bool HasLevelEnable();
        bool IsRessources();
        ElementType.Type GetElementType();

        // Cost of upgrading the element
        ElementCost GetElementCost(int level);
        
        // Production of the element
        ElementProduction GetElementProduction(int level);

        // Bonus on ressources for the element
        ElementProductionBonus GetElementProductionBonus(int level);

        // Score value for element
        int GetScoreValue(int level);
    }
}
