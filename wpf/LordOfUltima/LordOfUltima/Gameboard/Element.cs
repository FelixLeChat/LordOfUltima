using System;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using LordOfUltima.Events;
using LordOfUltima.MGameboard;

namespace LordOfUltima
{
    public class Element
    {
        private int _neighbourRessources;
        public Element BonusBuilding { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        private IElementType _elementType;
        public bool HasElement { get; set; }
        public Element()
        {
            // Caracteristiques pour le rectangle
            _rectangle = new Rectangle
            {
                Width = Width,
                Height = Height
            };

            // Images
            _imageBrush = new ImageBrush();
            _rectangle.Fill = _imageBrush;
            _rectangle.IsEnabled = true;
            _rectangle.PreviewMouseLeftButtonDown += leftButtonDown;
            _rectangle.MouseLeftButtonUp += leftButtonUp;


            // Level Rectangle
            _levelRectangle = new Rectangle
            {
                Width = 8,
                Height = 8
            };
            ImageBrush imageLvl = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"Media/level_rect.png", UriKind.Relative))
            };
            _levelRectangle.Fill = imageLvl;
            _levelRectangle.IsHitTestVisible = false;

            // Level Label
            _levelLabel = new Label
            {
                Width = 20,
                Height = 20,
                FontSize = 8,
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xB3, 0x7B)),
                Content = _level.ToString(),
                IsHitTestVisible = false
            };

            // Click rect
            _clickBorder = new Border
            {
                Width = Width,
                Height = Height - 10
            };
            int borderThickness = 2;
            _clickBorder.BorderThickness = new Thickness(borderThickness, borderThickness, borderThickness, borderThickness);
            _clickBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xC9, 0xD6, 0x3A));
            _clickBorder.Visibility = Visibility.Hidden;
            _clickBorder.IsHitTestVisible = false;
        }

        /*
         * Attribuer une nouvelle image pour l'element
        */
        public void SetPath(string path)
        {
            _path = path;

            if (File.Exists(path))
            {
                _imageBrush.ImageSource = new BitmapImage(new Uri(@path, UriKind.Relative));
                HasElement = true;
            }
        }

        public Rectangle GetElement()
        {
            return _rectangle;
        }
        public Rectangle GetLevelElement()
        {
            return _levelRectangle;
        }
        public Label GetLevelLabel()
        {
            return _levelLabel;
        }
        public Border GetSelectElement()
        {
            return _clickBorder;
        }

        /*
         * Met invalide l'element (ne sera pas affiche)
        */
        public bool IsValid
        {
            get { return _isValid;}
            set
            {
                if (value == false)
                {
                    _isValid = false;
                    _rectangle.Opacity = 0;
                    _levelRectangle.Opacity = 0;
                    _levelLabel.Opacity = 0;                    
                }
            }
        }


        /*
         * Ajout d'un evenement sur le clic de l'item
        */
        private bool _isClicked;
        private void leftButtonDown(object sender, RoutedEventArgs e)
        {
            _isClicked = true;
        }
        private void leftButtonUp(object sender, RoutedEventArgs e)
        {
            if(_isClicked && !MainWindow.GetIsMouseMove())
            {
                // reset all select borders
                _isClicked = false;
                ResetMapElementBorder.Instance.ResetSelectionBorder();

                // add the select border
                if(_isValid)
                    ShowSelectBorder();

                // Trigger menu
                if (_isValid && !HasElement)
                {
                    BuildingMenuVisibility.Instance.ShowBuildingMenu();
                    BuildEvent.Instance.SetElementToBuild(this);
                }
                else
                {
                    BuildingMenuVisibility.Instance.HideBuildingMenu();
                }


                if (_isValid && HasElement)
                {
                    BuildingDetailsVisibility.Instance.SetElementMenuDetail(this);
                    BuildingDetailsVisibility.Instance.ShowBuildingDetails();
                    UpgradeElement.Instance.ElementToUpgrade = this;
                    DeleteElement.Instance.ElementToDelete = this;
                }
                else
                {
                    BuildingDetailsVisibility.Instance.HideBuildingDetails();
                }
            }
        }

        // Elements graphiques lie au building
        private readonly Rectangle _rectangle;
        private readonly Rectangle _levelRectangle;
        private readonly Border _clickBorder;
        private readonly Label _levelLabel;


        private ImageBrush _imageBrush;
        public ImageBrush GetImageBrush() { return _imageBrush;}
        private string _path = "";
        public int Width = 40;
        public int Height = 40;
        private bool _isValid = true;
        private int _level;
        public int Level
        {
            get { return _level; }
            set
            {
                if (value < 0 || value > 10) return;
                _level = value;
                _levelLabel.Content = _level;
            }
        }

        /*
         * Methode pour la gestion de la presence de l'indicateur de niveau
        */
        private bool _levelIndicatorVisibility;
        public void HideLevelIndicator()
        {
            _levelRectangle.Opacity = 0;
            _levelLabel.Opacity = 0;
            _levelIndicatorVisibility = false;
        }
        public void ShowLevelIndicator()
        {
            // Verifier si l'objet est valide (dois etre afficher) avant de l'afficher
            if(_isValid && _level > 0)
            {
                _levelRectangle.Opacity = 1;
                _levelLabel.Opacity = 1;
            }
            _levelIndicatorVisibility = true;
        }

        public bool HasSelectBorder = false;
        public void HideSelectBorder()
        {
            _clickBorder.Visibility = Visibility.Hidden;
        }
        public void ShowSelectBorder()
        {
            _clickBorder.Visibility = Visibility.Visible;
        }

        public void SetElementType(IElementType type)
        {
            if (type == null)
            {
                return;
            }
            _elementType = type;

            // Set new image
            SetPath(type.GetImagePath());

            if (_levelIndicatorVisibility)
            {
                ShowLevelIndicator();
            }
        }

        public IElementType GetElementType()
        {
            return _elementType;
        }

        public void Initialise()
        {
            HideSelectBorder();
            HasElement = false;
            _elementType = null;
            _imageBrush = new ImageBrush();
            _rectangle.Fill = _imageBrush;
            _level = 0;

            // Natural ressources near
            _neighbourRessources = 0;
            _fieldsCount = 0;

            // Initialise bonus building
            BonusBuilding = null;
            TotalBonus = 0;
        }

        public int NbRessourcesAround 
        {
            get { return _neighbourRessources;}
            set { if (value < 9) _neighbourRessources = value; }
        }

        private int _fieldsCount;
        public int FieldsCount
        {
            get { return _fieldsCount; }
            set
            {
                if (value < 9 && value >= 0)
                {
                    _fieldsCount = value;
                }
            }
        }

        public double TotalBonus { get; set; }
    }
}
