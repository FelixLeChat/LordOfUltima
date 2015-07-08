namespace LordOfUltima.Research
{
    class ResearchBonus
    {
        public ResearchBonus(int wood, int stone, int iron, int food)
        {
            WoodBonus = wood;
            StoneBonus = stone;
            IronBonus = iron;
            FoodBonus = food;
        }

        public ResearchBonus(int gold, int research)
        {
            GoldBonus = gold;
            Research = research;
        }

        public int WoodBonus;
        public int StoneBonus;
        public int IronBonus;
        public int FoodBonus;

        public int GoldBonus;
        public int Research;
    }
}
