using System;
using System.Windows;
using System.Windows.Controls;
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
            get
            {
                if (_instance == null)
                {
                    _instance = new UIImagesInit();
                }
                return _instance;
            }
        }

        private MainWindow _mainWindow;
        private UIImagesInit()
        {
            _mainWindow = MainWindow.m_ins;
        }

        public void InitImages()
        {
            _mainWindow = MainWindow.m_ins;
            if (_mainWindow == null)
            {
                return;
            }

            // Couleur du menu en haut
            initImageCanvas("Media/menu_repeat.png", _mainWindow.top_menu);
            initImageMenu("Media/menu_repeat.png", _mainWindow.menu1);

            // Couleur de fond de toute la fenetre
            _mainWindow.grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));

            // Map de la ville (background)
            initImageCanvas("Media/main.png", _mainWindow.canvas1);

            // Background pour le degrade (Fog)
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(0, 1);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, -0.03));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.10));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.90));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 1.03));
            _mainWindow.canvas_fog.Fill = myLinearGradientBrush;

            // Background pour Scroll View
            initImageScrollviewer("Media/menu.png", _mainWindow.scrollview);

            // Building Details Bottom seperator
            initImageRectangle("Media/menu_division.png", _mainWindow.building_detail_seperator);
            // Building Details Bottom cutoff rectangle
            initImageRectangle("Media/menu_bottom.png", _mainWindow.bottom_menu_rect);

            // Image pour le menu des ressources
            initImageCanvas("Media/ress_menu.png", _mainWindow.ress_menu);

            // Ressources images
            initImageCanvas("Media/ressource/icon/Lou_resource_wood.png", _mainWindow.ress_wood);
            initImageCanvas("Media/ressource/icon/Lou_resource_stone.png", _mainWindow.ress_stone);
            initImageCanvas("Media/ressource/icon/Lou_resource_grain.png", _mainWindow.ress_grain);
            initImageCanvas("Media/ressource/icon/Lou_resource_iron.png", _mainWindow.ress_iron);
            initImageCanvas("Media/ressource/icon/Lou_resource_gold.png", _mainWindow.ress_gold);
            initImageCanvas("Media/ressource/icon/research.png", _mainWindow.ress_research);

            // Building menu images
            initImageRectangle(new WoodcutterElementType().getDetailImagePath(), _mainWindow.building_woodcutter);
            initImageRectangle(new SawmillElementType().getDetailImagePath(), _mainWindow.building_sawmill);
            initImageRectangle(new QuarryElementType().getDetailImagePath(), _mainWindow.building_quarry);
            initImageRectangle(new StoneMasonElementType().getDetailImagePath(), _mainWindow.building_stonemason);
            initImageRectangle(new IronMineElementType().getDetailImagePath(), _mainWindow.building_ironmine);
            initImageRectangle(new FoundryElementType().getDetailImagePath(), _mainWindow.building_foundry);
            initImageRectangle(new FarmElementType().getDetailImagePath(), _mainWindow.building_farm);
            initImageRectangle(new MillElementType().getDetailImagePath(), _mainWindow.building_mill);
        }

        private void initImageRectangle(string imgUrl, Rectangle rectangle)
        {
            rectangle.Fill = getImageBrush(imgUrl);
        }

        private void initImageScrollviewer(string imgUrl, ScrollViewer scrollViewer)
        {
            scrollViewer.Background = getImageBrush(imgUrl);
        }

        private void initImageMenu(string imgUrl, Menu menu)
        {
            menu.Background = getImageBrush(imgUrl);
        }

        private void initImageCanvas(string imgUrl, Canvas canvas)
        {
            canvas.Background = getImageBrush(imgUrl);
        }

        private ImageBrush getImageBrush(string imgUrl)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.Relative));
            return imageBrush;
        }
    }
}
