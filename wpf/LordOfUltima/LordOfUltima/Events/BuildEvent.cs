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

        // Here is the place where we build the element for the first time(on map)
        public void buildElement()
        {
            if (elementToBuild != null && typeToBuild != null)
            {
                // Increase Level of building
                elementToBuild.Level = 1;
                // build the building
                elementToBuild.setElementType(typeToBuild);
                // check neighbours for ressources
                m_gameboard.checkNeignbourRessources(elementToBuild);

                // if we build a farm, spawn fields around it
                if (typeToBuild.GetElementType() == ElementType.type.BUILDING_FARM)
                {
                    m_gameboard.spawnFields(elementToBuild);
                }

                //if build sucessfull, show in side menu
                MainWindow.m_ins.setElementMeduDetail(elementToBuild);
                MainWindow.m_ins.setVisibleBuildingDetails(true);
            }     
        }

    }
}
