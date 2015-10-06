using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class UIImagesInit
    {
        private static UIImagesInit _instance;
        public static UIImagesInit Instance
        {
            get { return _instance ?? (_instance = new UIImagesInit()); }
        }

        private MainWindow _mainWindow;
        private UIImagesInit()
        {
            _mainWindow = MainWindow.MIns;
        }

        public void InitImages()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
            {
                return;
            }

            // Couleur de fond de toute la fenetre
            _mainWindow.grid.Background = Properties.Settings.Default.IsDarkSkin ? new SolidColorBrush(Color.FromRgb(0x2B, 0x2E, 0x25)) : new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));
            

            // Background pour le degrade (Fog)
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, -0.03));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.10));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.90));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 1.03));
            _mainWindow.canvas_fog.Fill = myLinearGradientBrush;

            // Building menu images
            InitImageRectangle(new WoodcutterElementType().GetDetailImagePath(), _mainWindow.building_woodcutter);
            InitImageRectangle(new SawmillElementType().GetDetailImagePath(), _mainWindow.building_sawmill);
            InitImageRectangle(new QuarryElementType().GetDetailImagePath(), _mainWindow.building_quarry);
            InitImageRectangle(new StoneMasonElementType().GetDetailImagePath(), _mainWindow.building_stonemason);
            InitImageRectangle(new IronMineElementType().GetDetailImagePath(), _mainWindow.building_ironmine);
            InitImageRectangle(new FoundryElementType().GetDetailImagePath(), _mainWindow.building_foundry);
            InitImageRectangle(new FarmElementType().GetDetailImagePath(), _mainWindow.building_farm);
            InitImageRectangle(new MillElementType().GetDetailImagePath(), _mainWindow.building_mill);
            InitImageRectangle(new TownhouseElementType().GetDetailImagePath(), _mainWindow.building_townhouse);
            InitImageRectangle(new MarketplaceElementType().GetDetailImagePath(), _mainWindow.building_marketplace);
            InitImageRectangle(new ResearchCenterElementType().GetDetailImagePath(), _mainWindow.building_research_center);
            InitImageRectangle(new WarehouseElementType().GetDetailImagePath(), _mainWindow.building_storage);

            // Military Building init
            InitImageRectangle(new BarrackBuilding().GetDetailImagePath(), _mainWindow.building_barrack);
            InitImageRectangle(new CityguardBuilding().GetDetailImagePath(), _mainWindow.building_cityguard_house);
            InitImageRectangle(new TrainingGroundBuilding().GetDetailImagePath(), _mainWindow.building_training_ground);
            InitImageRectangle(new StableBuilding().GetDetailImagePath(), _mainWindow.building_stable);
            InitImageRectangle(new MoonglowTowerBuilding().GetDetailImagePath(), _mainWindow.building_moonglow_tower);
        }

        private void InitImageRectangle(string imgUrl, Rectangle rectangle)
        {
            rectangle.Fill = getImageBrush(imgUrl);
        }

        private ImageBrush getImageBrush(string imgUrl)
        {
            ImageBrush imageBrush = new ImageBrush {ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.RelativeOrAbsolute))};
            return imageBrush;
        }
        
        public void TriggerDarkTheme()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            _mainWindow.grid.Background =
                new SolidColorBrush((_mainWindow.trigger_dark.IsChecked)
                    ? Color.FromRgb(0x2B, 0x2E, 0x25)
                    : Color.FromRgb(0xE9, 0xD3, 0xAE));

            Properties.Settings.Default.IsDarkSkin = _mainWindow.trigger_dark.IsChecked;
        }
    }
}
