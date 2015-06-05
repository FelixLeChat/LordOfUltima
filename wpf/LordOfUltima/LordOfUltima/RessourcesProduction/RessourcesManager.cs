using System.Windows.Documents;

namespace LordOfUltima.RessourcesProduction
{
    class RessourcesManager
    {
        private static RessourcesManager m_ins;
        public static RessourcesManager Instance
        {
            get
            {
                if (m_ins == null)
                {
                    m_ins = new RessourcesManager();
                }
                return m_ins;
            }
        }

        private Gameboard m_gameboard;
        private RessourcesManager()
        {
            m_gameboard = Gameboard.getInstance();
        }

        public void CalculateRessources()
        {
            Element[,] elementList = m_gameboard.getMap();
        }
    }
}
