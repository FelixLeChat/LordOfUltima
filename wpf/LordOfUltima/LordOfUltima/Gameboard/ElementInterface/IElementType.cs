using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LordOfUltima.MGameboard
{
    interface IElementType
    {
        string getImagePath();
        string Name();
        bool HasLevelEnable();
        bool IsRessources();
    }
}
