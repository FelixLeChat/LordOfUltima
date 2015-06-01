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
        ElementCost GetElementCost(int level);
    }
}
