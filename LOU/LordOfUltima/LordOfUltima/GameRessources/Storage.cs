using LordOfUltima.MGameboard;

namespace LordOfUltima.RessourcesStorage
{
    class Storage
    {
        private static Storage _instanceStorage;
        public static Storage Instance
        {
            get { return _instanceStorage ?? (_instanceStorage = new Storage()); }
        }

        public double WoodStorage { get; set; }
        public double StoneStorage { get; set; }
        public double IronStorage { get; set; }
        public double FoodStorage { get; set; }

        public void Initialise()
        {
            WoodStorage = 0;
            StoneStorage = 0;
            IronStorage = 0;
            FoodStorage = 0;
        }

        // TODO : Maybe not needed
        public void SetDefault()
        {
            IElementType townHallElementType = new TownHallElementType();
            ElementStorage townHallStorage = townHallElementType.GetElementStorage(1);

            WoodStorage = townHallStorage.WoodStorage;
            StoneStorage = townHallStorage.StoneStorage;
            IronStorage = townHallStorage.IronStorage;
            FoodStorage = townHallStorage.FoodStorage;
        }

        public void AddToStorage(ElementStorage elementStorage)
        {
            if (elementStorage == null)
                return;

            WoodStorage += elementStorage.WoodStorage;
            StoneStorage += elementStorage.StoneStorage;
            IronStorage += elementStorage.IronStorage;
            FoodStorage += elementStorage.FoodStorage;
        }

        public void UpdateStorageCapacity()
        {
            SetDefault();
            Element[,] elementList = Gameboard.Instance.GetMap();

            foreach (var element in elementList)
            {
                if (element.HasElement && element.Level > 0)
                {
                    assingStorage(element);
                }
            }
        }

        private void assingStorage(Element element)
        {
            if (element.GetElementType() != null)
            {
                ElementStorage elementStorage = element.GetElementType().GetElementStorage(element.Level);

                if (elementStorage != null)
                {
                    AddToStorage(elementStorage);
                }
            }
        }
    }
}
