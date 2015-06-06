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

        private Gameboard _gameboard;
        private Timer _ressources_update_timer;
        private RessourcesManager()
        {
            _gameboard = Gameboard.getInstance();
        }

        public void StartRessourcesManager()
        {
            // timer for ressources updates
            _ressources_update_timer = new Timer(obj => { CalculateRessources(); }, null, 0, 1000);
        }

        public void CalculateRessources()
        {
            Element[,] elementList = _gameboard.getMap();
        }
    }
}
