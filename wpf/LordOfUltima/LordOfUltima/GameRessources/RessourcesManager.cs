using System;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using LordOfUltima.Error;
using LordOfUltima.MGameboard;
using LordOfUltima.Research;
using LordOfUltima.RessourcesStorage;

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
            _ressourcesUpdateTimer = new Timer(obj => { updateRessourcesValues(); }, null, 0, 1000);
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

        private void updateRessourcesValues()
        {
            if (_timeScale <= 0)
                return;

            Ressources ressources = Ressources.Instance;

            ressources.WoodQty += _ressourcesProduction.WoodQty/_timeScale;
            ressources.StoneQty += _ressourcesProduction.StoneQty/_timeScale;
            ressources.IronQty += _ressourcesProduction.IronQty/_timeScale;
            ressources.FoodQty += _ressourcesProduction.FoodQty/_timeScale;
            ressources.GoldQty += _ressourcesProduction.GoldQty/_timeScale;
            ressources.ResearchQty += _ressourcesProduction.ResearchQty/_timeScale;

            checkStorage();
        }

        private void checkStorage()
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
                    assignRessources(element);
                }
            }

            // update UI
            updateRessourceProductionUI();
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


            _ressourcesProduction.WoodQty += calculateRessource(elementProduction.Wood, element.TotalBonus, woodBonus);
            _ressourcesProduction.StoneQty += calculateRessource(elementProduction.Stone, element.TotalBonus, stoneBonus);
            _ressourcesProduction.IronQty += calculateRessource(elementProduction.Iron, element.TotalBonus, ironBonus);
            _ressourcesProduction.FoodQty += calculateRessource(elementProduction.Food, element.TotalBonus, foodBonus);
            _ressourcesProduction.GoldQty += calculateRessource(elementProduction.Gold, element.TotalBonus, goldBonus);
            _ressourcesProduction.ResearchQty += calculateRessource(elementProduction.Research, element.TotalBonus);
        }

        private double calculateRessource(int baseProduction, double bonus = 0, int researchBonus = 0)
        {
            double production = baseProduction;
            return production*((bonus + researchBonus)/100);
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
            if (bonusElement != null && bonusElement.GetElementType() != null)
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

        private void updateRessourceProductionUI()
        {
            MainWindow mainWindow = MainWindow.MIns;
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
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            Ressources ressources = Ressources.Instance;
            Storage storage = Storage.Instance;

            int wood = (int)Math.Round(ressources.WoodQty);
            int stone = (int)Math.Round(ressources.StoneQty);
            int iron = (int)Math.Round(ressources.IronQty);
            int food = (int)Math.Round(ressources.FoodQty);

            mainWindow.qty_wood.Content = wood;
            mainWindow.qty_stone.Content = stone;
            mainWindow.qty_iron.Content = iron;
            mainWindow.qty_grain.Content = food;
            mainWindow.qty_gold.Content = Math.Round(ressources.GoldQty);
            mainWindow.qty_research.Content = Math.Round(ressources.ResearchQty);

            // update visual for storage qty
            updateStorageUI(wood, (int)storage.WoodStorage, mainWindow.qty_wood);
            updateStorageUI(stone, (int)storage.StoneStorage, mainWindow.qty_stone);
            updateStorageUI(iron, (int)storage.IronStorage, mainWindow.qty_iron);
            updateStorageUI(food, (int)storage.FoodStorage, mainWindow.qty_grain);
        }

        private void updateStorageUI(int currentQty, int max, System.Windows.Controls.Label label)
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
    }
}
