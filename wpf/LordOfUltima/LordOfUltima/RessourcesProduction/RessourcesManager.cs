using System.Threading;

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
        private Timer m_ressources_update_timer;
        private RessourcesManager()
        {
            m_gameboard = Gameboard.getInstance();

            // timer for ressources updates
            m_ressources_update_timer = new Timer(obj => { CalculateRessources(); }, null, 0, 1000);
        }

        public void CalculateRessources()
        {
            Element[,] elementList = m_gameboard.getMap();
        }
    }
}
