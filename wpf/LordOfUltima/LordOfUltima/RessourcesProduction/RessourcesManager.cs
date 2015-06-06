using System.Threading;
using LordOfUltima.MGameboard;

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
        private RessourcesProduction _ressourcesProduction;
        private RessourcesManager()
        {
            _gameboard = Gameboard.getInstance();
            _ressourcesProduction = RessourcesProduction.Instance;
            // Default time = 1 min
            _timeScale = 60;
        }

        public void StartRessourcesManager()
        {
            // timer for ressources updates
            //_ressourcesUpdateTimer = new Timer(obj => { CalculateRessources(); }, null, 0, 1000);
        }

        // time scale on wich we apply the points
        private int _timeScale;
        public int TimeScale
        {
            get { return _timeScale; }
            set
            {
                if (value > 0)
                {
                    _timeScale = value;
                }
            } 
        }

        public void CalculateRessources()
        {
            // Reinitialise ressources production
            _ressourcesProduction.InitRessourcesProduction();
            Element[,] elementList = _gameboard.getMap();

            foreach (var element in elementList)
            {
                if (element.HasElement && element.Level > 0)
                {
                    assignRessources(element);
                }
            }

            // update UI
            updateRessourceUI();
        }

        private void assignRessources(Element element)
        {      
            // check the Element Type
            IElementType elementType = element.GetElementType();
            if (elementType.IsRessources() == true)
                return;

            // Get the element Level
            int elementLevel = element.Level;
            if (elementLevel <= 0)
                return;

            // Get the base production
            ElementProduction elementProduction = elementType.GetElementProduction(elementLevel);
            if (elementProduction == null)
                return;

            _ressourcesProduction.WoodQty += calculateRessource(elementProduction.Wood);
            _ressourcesProduction.StoneQty += calculateRessource(elementProduction.Stone);
            _ressourcesProduction.IronQty += calculateRessource(elementProduction.Iron);
            _ressourcesProduction.FoodQty += calculateRessource(elementProduction.Food);
        }

        private int calculateRessource(
            int baseProduction, 
            int qtyNaturalRessources = 0, 
            int firstBonus = 0, 
            int secondBonus = 0,
            int buildingBonus = 0)
        {
            return baseProduction;
        }

        private void updateRessourceUI()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.qty_wood_incr.Content = _ressourcesProduction.WoodQty;
            mainWindow.qty_stone_incr.Content = _ressourcesProduction.StoneQty;
            mainWindow.qty_iron_incr.Content = _ressourcesProduction.IronQty;
            mainWindow.qty_grain_incr.Content = _ressourcesProduction.FoodQty;
        }
    }
}
