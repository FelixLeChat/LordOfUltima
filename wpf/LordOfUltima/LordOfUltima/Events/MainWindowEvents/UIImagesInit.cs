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

            // Couleur du menu en haut
            InitImageCanvas("Media/menu_repeat.png", _mainWindow.top_menu);
            InitImageMenu("Media/menu_repeat.png", _mainWindow.menu1);

            // Couleur de fond de toute la fenetre
            _mainWindow.grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));

            // Map de la ville (background)
            InitImageCanvas("Media/main.png", _mainWindow.canvas1);

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

            // Background pour Scroll View
            InitImageScrollviewer("Media/menu.png", _mainWindow.scrollview);

            // Building Details Bottom seperator
            InitImageRectangle("Media/menu_division.png", _mainWindow.building_detail_seperator);
            // Building Details Bottom cutoff rectangle
            InitImageRectangle("Media/menu_bottom.png", _mainWindow.bottom_menu_rect);

            // Image pour le menu des ressources
            InitImageCanvas("Media/ress_menu.png", _mainWindow.ress_menu);

            // Ressources images
            InitImageCanvas("Media/ressource/icon/Lou_resource_wood.png", _mainWindow.ress_wood);
            InitImageCanvas("Media/ressource/icon/Lou_resource_stone.png", _mainWindow.ress_stone);
            InitImageCanvas("Media/ressource/icon/Lou_resource_grain.png", _mainWindow.ress_grain);
            InitImageCanvas("Media/ressource/icon/Lou_resource_iron.png", _mainWindow.ress_iron);
            InitImageCanvas("Media/ressource/icon/Lou_resource_gold.png", _mainWindow.ress_gold);
            InitImageCanvas("Media/ressource/icon/research.png", _mainWindow.ress_research);

            // Building menu images
            InitImageRectangle(new WoodcutterElementType().GetDetailImagePath(), _mainWindow.building_woodcutter);
            InitImageRectangle(new SawmillElementType().GetDetailImagePath(), _mainWindow.building_sawmill);
            InitImageRectangle(new QuarryElementType().GetDetailImagePath(), _mainWindow.building_quarry);
            InitImageRectangle(new StoneMasonElementType().GetDetailImagePath(), _mainWindow.building_stonemason);
            InitImageRectangle(new IronMineElementType().GetDetailImagePath(), _mainWindow.building_ironmine);
            InitImageRectangle(new FoundryElementType().GetDetailImagePath(), _mainWindow.building_foundry);
            InitImageRectangle(new FarmElementType().GetDetailImagePath(), _mainWindow.building_farm);
            InitImageRectangle(new MillElementType().GetDetailImagePath(), _mainWindow.building_mill);

            InitImageButton("Media/chat_button_up.png", _mainWindow.chat_button_open);
            InitImageButton("Media/chat_Button_down.png", _mainWindow.chat_button_down);
            InitImageButton("Media/delete_button.png", _mainWindow.delete_element_button);
        }

        private void InitImageRectangle(string imgUrl, Rectangle rectangle)
        {
            rectangle.Fill = getImageBrush(imgUrl);
        }

        private void InitImageScrollviewer(string imgUrl, ScrollViewer scrollViewer)
        {
            scrollViewer.Background = getImageBrush(imgUrl);
        }

        private void InitImageMenu(string imgUrl, Menu menu)
        {
            menu.Background = getImageBrush(imgUrl);
        }

        private void InitImageCanvas(string imgUrl, Canvas canvas)
        {
            canvas.Background = getImageBrush(imgUrl);
        }

        private void InitImageButton(string imgUrl, Button button)
        {
            button.Background = getImageBrush(imgUrl);
        }

        private ImageBrush getImageBrush(string imgUrl)
        {
            ImageBrush imageBrush = new ImageBrush {ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.Relative))};
            return imageBrush;
        }
    }
}
