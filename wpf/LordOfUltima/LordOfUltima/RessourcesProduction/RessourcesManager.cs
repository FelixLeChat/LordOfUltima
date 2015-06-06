using System.Threading;

namespace LordOfUltima.RessourcesProduction
{
    class RessourcesManager
    {
        private static RessourcesManager _ins;
        public static RessourcesManager Instance
        {
            get { return _ins ?? (_ins = new RessourcesManager()); }
        }

        private readonly Gameboard _gameboard;
        private Timer _ressourcesUpdateTimer;
        private RessourcesManager()
        {
            _gameboard = Gameboard.getInstance();
        }

        public void StartRessourcesManager()
        {
            // timer for ressources updates
            _ressourcesUpdateTimer = new Timer(obj => { CalculateRessources(); }, null, 0, 1000);
        }

        public void CalculateRessources()
        {
            Element[,] elementList = _gameboard.getMap();
        }
    }
}
