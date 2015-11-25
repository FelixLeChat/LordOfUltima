using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using LordOfUltima.Error;
using LordOfUltima.MGameboard;
using LordOfUltima.Research;
using LordOfUltima.RessourcesStorage;
using LordOfUltima.Units;
using LordOfUltima.Units.Units;

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
        private readonly ResearchHandler _researchHandler;

        private Timer _ressourcesUpdateTimer;
        private readonly RessourcesProduction _ressourcesProduction;
        private RessourcesManager()
        {
            _gameboard = Gameboard.Instance;
            _ressourcesProduction = RessourcesProduction.Instance;
            _researchHandler = ResearchHandler.Instance;

            // Default time = 1 min
            _timeScale = 60;
        }

        public void StartRessourcesManager()
        {
            // timer for ressources updates
            _ressourcesUpdateTimer = new Timer(obj => { UpdateRessourcesValues(); }, null, 0, 1000);
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

        private void UpdateRessourcesValues()
        {
            if (_timeScale <= 0)
                return;

            var ressources = Ressources.Instance;

            ressources.WoodQty += _ressourcesProduction.WoodQty/_timeScale;
            ressources.StoneQty += _ressourcesProduction.StoneQty/_timeScale;
            ressources.IronQty += _ressourcesProduction.IronQty/_timeScale;
            ressources.FoodQty += _ressourcesProduction.FoodQty/_timeScale;
            ressources.GoldQty += _ressourcesProduction.GoldQty/_timeScale;
            ressources.ResearchQty += _ressourcesProduction.ResearchQty/_timeScale;

            // verify if storage is enough
            CheckStorage();

            // verify if unit survives
            CheckUnitFoodCost();
        }

        private static void CheckStorage()
        {
            var storage = Storage.Instance;
            var ressources = Ressources.Instance;

            // Check wood ressources
            if (ressources.WoodQty > storage.WoodStorage)
            {
                ressources.WoodQty = storage.WoodStorage;
                //ErrorManager.Instance.AddError(new Error.Error(){Description = Error.Error.Type.WOOD_STORAGE_FULL});
            }

            // Check stone ressources
            if (ressources.StoneQty > storage.StoneStorage)
            {
                ressources.StoneQty = storage.StoneStorage;
                //ErrorManager.Instance.AddError(new Error.Error() { Description = Error.Error.Type.STONE_STORAGE_FULL });
            }

            // Check iron ressources
            if (ressources.IronQty > storage.IronStorage)
            {
                ressources.IronQty = storage.IronStorage;
                //ErrorManager.Instance.AddError(new Error.Error() { Description = Error.Error.Type.IRON_STORAGE_FULL });
            }

            // check food ressources
            if (ressources.FoodQty > storage.FoodStorage)
            {
                ressources.FoodQty = storage.FoodStorage;
                //ErrorManager.Instance.AddError(new Error.Error() { Description = Error.Error.Type.FOOD_STORAGE_FULL });
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
                    AssignRessources(element);
                }
            }

            // Add the unit food cost
            UpdateUnitsFoodCost();

            // update UI
            UpdateRessourceProductionUI();
        }

        private void AssignRessources(Element element)
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

            UpdateElementTotalBonus(element);

            // Get Wood bonus
            var woodResearch = _researchHandler.WoodResearchType;
            var woodBonus = 0;

            if(woodResearch .GetLevel() > 0)
                woodBonus = woodResearch.GetResearchBonus(woodResearch.GetLevel()).WoodBonus;

            // Get Stone bonus
            var stoneResearch = _researchHandler.StoneResearchType;
            var stoneBonus = 0;
            if (stoneResearch.GetLevel() > 0)
                stoneBonus = stoneResearch.GetResearchBonus(stoneResearch.GetLevel()).StoneBonus;

            // Get Iron bonus
            var ironResearch = _researchHandler.IronResearchType;
            var ironBonus = 0;
            if (ironResearch.GetLevel() > 0)
                ironBonus = ironResearch.GetResearchBonus(ironResearch.GetLevel()).IronBonus;

            // Get Food Bonus
            var foodResearch = _researchHandler.FoodResearchType;
            var foodBonus = 0;
            if (foodResearch.GetLevel() > 0)
                foodBonus = foodResearch.GetResearchBonus(foodResearch.GetLevel()).FoodBonus;

            // Get Gold bonus
            var goldResearch = _researchHandler.GoldResearchType;
            var goldBonus = 0;
            if (goldResearch.GetLevel() > 0)
                goldBonus = goldResearch.GetResearchBonus(goldResearch.GetLevel()).GoldBonus;


            _ressourcesProduction.WoodQty += CalculateRessource(elementProduction.Wood, element.TotalBonus, woodBonus);
            _ressourcesProduction.StoneQty += CalculateRessource(elementProduction.Stone, element.TotalBonus, stoneBonus);
            _ressourcesProduction.IronQty += CalculateRessource(elementProduction.Iron, element.TotalBonus, ironBonus);
            _ressourcesProduction.FoodQty += CalculateRessource(elementProduction.Food, element.TotalBonus, foodBonus);
            _ressourcesProduction.GoldQty += CalculateRessource(elementProduction.Gold, element.TotalBonus, goldBonus);
            _ressourcesProduction.ResearchQty += CalculateRessource(elementProduction.Research, element.TotalBonus);
        }

        private static double CalculateRessource(int baseProduction, double bonus = 0, int researchBonus = 0)
        {
            double production = baseProduction;
            return production*((bonus + researchBonus)/100);
        }

        private static void UpdateElementTotalBonus(Element element)
        {
            var elementType = element.GetElementType();

            // Get number of natural ressources around
            var nbNaturalRessources = element.NbRessourcesAround;

            var firstBonus = 0;
            var secondBonus = 0;
            var bonusRessource = ElementType.GetTypeObject(ElementType.GetBonusRessource(elementType.GetElementType()));
            if (bonusRessource != null)
            {
                firstBonus = bonusRessource.GetElementProductionBonus(0).FirstBonus;
                secondBonus = bonusRessource.GetElementProductionBonus(0).SecondBonus;
            }

            var buildingBonus = 0;
            var bonusElement = element.BonusBuilding;
            if (bonusElement?.GetElementType() != null)
            {
                var elementProductionBonus =
                    bonusElement.GetElementType().GetElementProductionBonus(bonusElement.Level);
                if (elementProductionBonus != null)
                {
                    buildingBonus = elementProductionBonus.GetFirstNotNull();
                }
            }

            var fieldsCount = element.FieldsCount;
            var fieldBonus = new FieldsElementType().GetElementProductionBonus(0);
            

            var bonus = 100;

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

        public void UpdateRessourceProductionUI()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.qty_wood_incr.Content = Math.Round(_ressourcesProduction.WoodQty);
            mainWindow.qty_stone_incr.Content = Math.Round(_ressourcesProduction.StoneQty);
            mainWindow.qty_iron_incr.Content = Math.Round(_ressourcesProduction.IronQty);
            mainWindow.qty_grain_incr.Content = Math.Round(_ressourcesProduction.FoodQty);
            mainWindow.qty_gold_incr.Text = "+ " + Math.Round(_ressourcesProduction.GoldQty) + " ";
            mainWindow.qty_research_incr.Text = "+ " + Math.Round(_ressourcesProduction.ResearchQty) + " ";
        }

        public void UpdateRessourceUi()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            var ressources = Ressources.Instance;
            var storage = Storage.Instance;

            var wood = (int)Math.Round(ressources.WoodQty);
            var stone = (int)Math.Round(ressources.StoneQty);
            var iron = (int)Math.Round(ressources.IronQty);
            var food = (int)Math.Round(ressources.FoodQty);

            mainWindow.qty_wood.Content = wood;
            mainWindow.qty_stone.Content = stone;
            mainWindow.qty_iron.Content = iron;
            mainWindow.qty_grain.Content = food;
            mainWindow.qty_gold.Content = Math.Round(ressources.GoldQty);
            mainWindow.qty_research.Content = Math.Round(ressources.ResearchQty);

            // update visual for storage qty
            UpdateStorageUI(wood, (int)storage.WoodStorage, mainWindow.qty_wood);
            UpdateStorageUI(stone, (int)storage.StoneStorage, mainWindow.qty_stone);
            UpdateStorageUI(iron, (int)storage.IronStorage, mainWindow.qty_iron);
            UpdateStorageUI(food, (int)storage.FoodStorage, mainWindow.qty_grain);
        }

        private static void UpdateStorageUI(int currentQty, int max, Label label)
        {
            if (currentQty == max)
            {
                label.Foreground = new SolidColorBrush(Color.FromRgb(0xEE,0x10,0x10));
            }
            else if (currentQty > 0.75*max)
            {
                label.Foreground = new SolidColorBrush(Color.FromRgb(0xFF,0x66,0x00));
            }
            else
            {
                label.Foreground = new SolidColorBrush(Color.FromRgb(0x24, 0x76, 0x24));
            }

            label.ToolTip = new ToolTip() {Content = "Storage max : " + max};
        }

        private void UpdateUnitsFoodCost()
        {
            var units = UnitManager.Instance.UnitsAvailables;
            var unitDescription = UnitManager.Instance.Units;

            var totalFoodCost = (from unit in units let foodCost = unitDescription[unit.Key].GetUnitStats().FoodUsage select foodCost*unit.Value).Sum();

            _ressourcesProduction.FoodQty -= totalFoodCost;
        }

        private void CheckUnitFoodCost()
        {
            if (Ressources.Instance.FoodQty < 0)
            {
                // missing food error
                ErrorManager.Instance.AddError(new Error.Error() {Description = Error.Error.Type.RECRUITMENT_FOOD_MISSING});
                var foodProduction = _ressourcesProduction.FoodQty;

                while (foodProduction < 0)
                {
                    // Kill random units to make for need of foods
                    var units = UnitManager.Instance.UnitsAvailables;
                    var unitDescription = UnitManager.Instance.Units;

                    //var key = units.FirstOrDefault(x => x.Value > 0).Key;
                    var random = new Random();
                    var unitsRecruited = units.Where(x => x.Value > 0);
                    var key = unitsRecruited.ElementAt(random.Next(0, unitsRecruited.Count()-1)).Key;
                    
                    // Kill
                    units[key]--;
                    var foodUsage = unitDescription[key].GetUnitStats().FoodUsage;
                    foodProduction += foodUsage;
                    _ressourcesProduction.FoodQty = foodProduction;
                }
                Ressources.Instance.FoodQty = 0;
                RecruitmentManager.Instance.UnitKilled = true;
            }
        }
    }
}
