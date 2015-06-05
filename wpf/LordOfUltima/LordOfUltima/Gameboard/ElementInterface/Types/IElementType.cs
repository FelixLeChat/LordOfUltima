namespace LordOfUltima.MGameboard
{
    public interface IElementType
    {
        string getImagePath();
        string getDetailImagePath();
        string GetElementInfo();
        string Name();
        bool HasLevelEnable();
        bool IsRessources();
        ElementType.type GetElementType();

        // Cost of upgrading the element
        ElementCost GetElementCost(int level);
        
        // Production of the element
        ElementProduction GetElementProduction(int level);

        // Bonus on ressources for the element
        ElementProductionBonus GetElementProductionBonus(int level);
    }
}
