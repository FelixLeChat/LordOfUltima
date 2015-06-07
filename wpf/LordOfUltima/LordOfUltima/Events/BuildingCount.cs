namespace LordOfUltima.Events
{
    class BuildingCount
    {
        private static BuildingCount _instance;
        public static BuildingCount Instance
        {
            get { return _instance ?? (_instance = new BuildingCount()); }
        }

        /*
         * Represente le compte maximale de building dans la map
         */
        private int _maxCount;
        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _maxCount = value;
                }
            }
        }

        /*
         * Represente le compte courant de buildings dans la map (exluant le town Hall)
         */
        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _count = value;
                }
            }
        }

        public void CountBuildings()
        {
            int count = 0;
            foreach (var element in Gameboard.Instance.GetMap())
            {
                if (element != null && element.HasElement && element.Level > 0)
                {
                    count++;
                }
            }
            Count = (count - 1);
            MaxCount = Gameboard.Instance.GetMap()[9, 9].Level*10;

            UpdateUI();
        }

        private void UpdateUI()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.building_count.Content = _count + " / " + _maxCount;
        }
    }
}
