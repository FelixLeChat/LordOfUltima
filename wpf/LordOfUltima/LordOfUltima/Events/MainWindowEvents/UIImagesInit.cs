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
            _mainWindow.grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));

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

        /*private ImageBrush getImageBrushResource(object value)
        {
            var resourceConverter = new WPFBitmapConverter();
            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = (ImageSource) resourceConverter.Convert(value, null, null, null)
            };
            return imageBrush;
        }*/
    }
}
