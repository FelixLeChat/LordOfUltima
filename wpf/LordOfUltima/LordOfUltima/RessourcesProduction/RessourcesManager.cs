using System;
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
        private readonly RessourcesProduction _ressourcesProduction;
        private RessourcesManager()
        {
            _gameboard = Gameboard.Instance;
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
            Element[,] elementList = _gameboard.GetMap();

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
            if (elementType.IsRessources())
                return;

            // Get the element Level
            int elementLevel = element.Level;
            if (elementLevel <= 0)
                return;

            // Get the base production
            ElementProduction elementProduction = elementType.GetElementProduction(elementLevel);
            if (elementProduction == null)
                return;

            updateElementTotalBonus(element);


            _ressourcesProduction.WoodQty += calculateRessource(elementProduction.Wood, element.TotalBonus);
            _ressourcesProduction.StoneQty += calculateRessource(elementProduction.Stone, element.TotalBonus);
            _ressourcesProduction.IronQty += calculateRessource(elementProduction.Iron, element.TotalBonus);
            _ressourcesProduction.FoodQty += calculateRessource(elementProduction.Food, element.TotalBonus);
        }

        private double calculateRessource(int baseProduction,double bonus = 0)
        {
            double production = baseProduction;
            return production*(bonus/100);
        }

        private void updateElementTotalBonus(Element element)
        {
            IElementType elementType = element.GetElementType();

            // Get number of natural ressources around
            int nbNaturalRessources = element.NbRessourcesAround;

            int firstBonus = 0;
            int secondBonus = 0;
            IElementType bonusRessource = ElementType.GetTypeObject(ElementType.GetBonusRessource(elementType.GetElementType()));
            if (bonusRessource != null)
            {
                firstBonus = bonusRessource.GetElementProductionBonus(0).FirstBonus;
                secondBonus = bonusRessource.GetElementProductionBonus(0).SecondBonus;
            }

            int buildingBonus = 0;
            Element bonusElement = element.BonusBuilding;
            if (bonusElement != null)
            {
                ElementProductionBonus elementProductionBonus =
                    bonusElement.GetElementType().GetElementProductionBonus(bonusElement.Level);
                if (elementProductionBonus != null)
                {
                    buildingBonus = elementProductionBonus.GetFirstNotNull();
                }
            }

            int fieldsCount = element.FieldsCount;
            ElementProductionBonus fieldBonus = new FieldsElementType().GetElementProductionBonus(0);
            

            double bonus = 100;

            // Natural ressources bonus
            if (nbNaturalRessources > 0)
            {
                bonus += firstBonus;
                if (nbNaturalRessources > 1)
                {
                    bonus += (nbNaturalRessources - 1)*secondBonus;
                }
            }
            // Production increase building bonus
            bonus += buildingBonus;

            // Fields bonus
            if (fieldsCount > 0)
            {
                bonus += fieldBonus.FirstBonus;
                if (fieldsCount > 1)
                {
                    bonus += (fieldsCount - 1) * fieldBonus.SecondBonus;
                }
            }

            // total bonus 

            element.TotalBonus = bonus;
        }

        private void updateRessourceUI()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.qty_wood_incr.Content = Math.Round(_ressourcesProduction.WoodQty);
            mainWindow.qty_stone_incr.Content = Math.Round(_ressourcesProduction.StoneQty);
            mainWindow.qty_iron_incr.Content = Math.Round(_ressourcesProduction.IronQty);
            mainWindow.qty_grain_incr.Content = Math.Round(_ressourcesProduction.FoodQty);
        }
    }
}
