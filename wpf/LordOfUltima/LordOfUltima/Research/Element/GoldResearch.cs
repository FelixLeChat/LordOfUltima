using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LordOfUltima.MGameboard;
using Label = System.Windows.Controls.Label;
using System.Windows.Shapes;

namespace LordOfUltima.Research.Element
{
    class GoldResearch : IResearchType
    {
        private readonly Dictionary<int, string> _imagePathDictionary = new Dictionary<int, string>();
        private readonly Dictionary<int, ResearchCost> _researchCosts = new Dictionary<int, ResearchCost>();
        private readonly Dictionary<int, ResearchBonus> _researchBonuses = new Dictionary<int, ResearchBonus>();

        private int _level;

        public GoldResearch()
        {
            // Image Path
            _imagePathDictionary.Add(1, "pack://application:,,,/Resources/Images/Research/Lou_artifact_copper_chest.png");
            _imagePathDictionary.Add(2, "pack://application:,,,/Resources/Images/Research/Lou_artifact_bronze_chest.png");
            _imagePathDictionary.Add(3, "pack://application:,,,/Resources/Images/Research/Lou_artifact_steel_chest.png");
            _imagePathDictionary.Add(4, "pack://application:,,,/Resources/Images/Research/Lou_artifact_silver_chest.png");
            _imagePathDictionary.Add(5, "pack://application:,,,/Resources/Images/Research/Lou_artifact_gold_chest.png");
            _imagePathDictionary.Add(6, "pack://application:,,,/Resources/Images/Research/Lou_artifact_platinum_chest.png");
            _imagePathDictionary.Add(7, "pack://application:,,,/Resources/Images/Research/Lou_artifact_verite_chest.png");
            _imagePathDictionary.Add(8, "pack://application:,,,/Resources/Images/Research/Lou_artifact_valorite_chest.png");

            // Research Cost
            _researchCosts.Add(1, new ResearchCost(200, 100, new ElementCost(500, 0, 0)));
            _researchCosts.Add(2, new ResearchCost(1000, 200, new ElementCost(1500, 0, 0)));
            _researchCosts.Add(3, new ResearchCost(4000, 500, new ElementCost(2500, 0, 0)));
            _researchCosts.Add(4, new ResearchCost(10000, 1000, new ElementCost(5000, 0, 0)));
            _researchCosts.Add(5, new ResearchCost(20000, 2000, new ElementCost(10000, 0, 0)));
            _researchCosts.Add(6, new ResearchCost(30000, 5000, new ElementCost(17500, 0, 0)));
            _researchCosts.Add(7, new ResearchCost(45000, 10000, new ElementCost(25000, 0, 0)));
            _researchCosts.Add(8, new ResearchCost(60000, 15000, new ElementCost(32500, 0, 0)));

            // Research bonus
            _researchBonuses.Add(1, new ResearchBonus(10, 0));
            _researchBonuses.Add(2, new ResearchBonus(20, 0));
            _researchBonuses.Add(3, new ResearchBonus(35, 0));
            _researchBonuses.Add(4, new ResearchBonus(50, 0));
            _researchBonuses.Add(5, new ResearchBonus(70, 0));
            _researchBonuses.Add(6, new ResearchBonus(90, 0));
            _researchBonuses.Add(7, new ResearchBonus(120, 0));
            _researchBonuses.Add(8, new ResearchBonus(150, 0));
        }

        public string GetElementPath(int level)
        {
            if (level > 0 && level <= 8)
            {
                return _imagePathDictionary[level];
            }
            return null;
        }

        public ResearchCost GetResearchCost(int level)
        {
            if (level > 0 && level <= 8)
            {
                return _researchCosts[level];
            }
            return null;
        }

        public ResearchBonus GetResearchBonus(int level)
        {
            if (level > 0 && level <= 8)
            {
                return _researchBonuses[level];
            }
            return null;
        }

        public int GetLevel()
        {
            return _level;
        }

        public int SetLevel(int level)
        {
            if (level > 0 && level <= GetMaxLevel())
            {
                _level = level;
            }
            return _level;
        }

        public int GetMaxLevel()
        {
            return _researchBonuses.Count;
        }

        public Label GetCurrentBonusLabel()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_current_bonus;
        }

        public Label GetNextBonusLabel()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_next_bonus;
        }

        public Rectangle GetImageRectangle()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_image;
        }

        public Button GetResearchButton()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_button;
        }

        public Canvas GetCostCanvas()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_cost;
        }

        public Label GetResearchCostLabel()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_research_cost;
        }

        public Label GetGoldCostLabel()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_gold_cost;
        }

        public Label GetRessourceCostLabel()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                throw new Exception("Invalid Main Window");

            return mainWindow.research_gold_wood_cost;
        }
    }
}
