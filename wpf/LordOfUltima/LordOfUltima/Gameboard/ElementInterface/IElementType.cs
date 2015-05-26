using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LordOfUltima.MGameboard
{
    public interface IElementType
    {
        string getImagePath();
        string Name();
        bool HasLevelEnable();
        bool IsRessources();
        ElementType.type GetElementType();
        ElementCost GetElementCost(int level);
    }
}
