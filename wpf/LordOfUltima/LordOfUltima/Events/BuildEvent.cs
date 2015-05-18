using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class BuildEvent
    {
        private static IElementType typeToBuild = null;
        private static Element elementToBuild = null;
        private static BuildEvent m_ins = null;
        private static Gameboard m_gameboard = Gameboard.getInstance();

        public static BuildEvent getInstance()
        {
            if (m_ins == null)
            {
                m_ins = new BuildEvent();
            }
            return m_ins;
        }

        public void setTypeToBuild(IElementType elementType)
        {
            typeToBuild = elementType;
        }

        public void setElementToBuild(Element element)
        {
            elementToBuild = element;
        }

        public void buildElement()
        {
            if (elementToBuild != null && typeToBuild != null)
            {
                // build the building
                elementToBuild.setElementType(typeToBuild);
                // check neighbours for ressources
                m_gameboard.checkNeignbourRessources(elementToBuild);
            }     
        }

    }
}
