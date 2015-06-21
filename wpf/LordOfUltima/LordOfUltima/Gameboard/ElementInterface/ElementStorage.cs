namespace LordOfUltima.MGameboard
{
    public class ElementStorage
    {
        public ElementStorage(int wood, int stone, int iron, int food)
        {
            WoodStorage = wood;
            StoneStorage = stone;
            IronStorage = iron;
            FoodStorage = food;
        }

        public ElementStorage(int value)
        {
            WoodStorage = value;
            StoneStorage = value;
            IronStorage = value;
            FoodStorage = value;
        }

        public double WoodStorage { get; set; }
        public double StoneStorage { get; set; }
        public double IronStorage { get; set; }
        public double FoodStorage { get; set; }
    }
}
