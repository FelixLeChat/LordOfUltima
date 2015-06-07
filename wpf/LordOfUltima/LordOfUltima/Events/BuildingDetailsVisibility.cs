using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class BuildingDetailsVisibility
    {
       private static BuildingDetailsVisibility _instance;
        public static BuildingDetailsVisibility Instance
        {
            get { return _instance ?? (_instance = new BuildingDetailsVisibility()); }
        }

        private MainWindow _mainWindow;
        private BuildingDetailsVisibility()
        {
            _mainWindow = MainWindow.MIns;
        }

        private Element _elementMenuDetail;
        public void SetElementMenuDetail(Element element)
        {
            if (element != null)
            {
                _elementMenuDetail = element;
            }
        }

        public void HideBuildingDetails()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            _mainWindow.building_details.Visibility = Visibility.Hidden;
        }

        public void ShowBuildingDetails()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            // Building menu visibility
            _mainWindow.building_menu_englob.Height = 400;
            _mainWindow.building_details.Visibility = Visibility.Visible;

            // if anything is not defined, return
            if (_elementMenuDetail == null || _elementMenuDetail.GetElementType() == null)
                return;

            // Add visual elements
            ImageBrush image = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(_elementMenuDetail.GetElementType().GetDetailImagePath(), UriKind.Relative))
            };
            _mainWindow.building_details_img.Fill = image;

            // set the elementInfo
            _mainWindow.building_detail_info.Text = _elementMenuDetail.GetElementType().GetElementInfo();

            int elementLevel = _elementMenuDetail.Level;
            if (elementLevel > 0 && elementLevel < 10)
            {
                // it's a building, show level info
                _mainWindow.building_detail_level_info.Visibility = Visibility.Visible;
                _mainWindow.building_detail_level.Content = _elementMenuDetail.Level;

                IElementType elementType = _elementMenuDetail.GetElementType();
                if (elementType == null)
                    return;
                ElementCost elementCost = elementType.GetElementCost(elementLevel + 1);

                // Show cost for upgrade
                _mainWindow.building_detail_wood_cost.Content = elementCost.Wood;
                _mainWindow.building_detail_stone_cost.Content = elementCost.Stone;
                _mainWindow.building_detail_iron_cost.Content = elementCost.Iron;

                // Show/Hide production label
                #region Production Label handle

                ElementProduction elementProduction =
                    _elementMenuDetail.GetElementType().GetElementProduction(elementLevel);
                if (elementProduction != null)
                {
                    _mainWindow.building_production_label.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_production.Content = elementProduction.GetFirstNotNull();
                }
                else
                {
                    // hide production label
                    _mainWindow.building_detail_production.Content = "";
                    _mainWindow.building_production_label.Visibility = Visibility.Hidden;
                }

                elementProduction =
                    _elementMenuDetail.GetElementType().GetElementProduction(elementLevel + 1);
                if (elementProduction != null)
                {
                    _mainWindow.production_dockpanel.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_production_next.Content = elementProduction.GetFirstNotNull();
                }
                else
                {
                    // hide production label
                    _mainWindow.building_detail_production_next.Content = "";
                    _mainWindow.production_dockpanel.Visibility = Visibility.Collapsed;
                }

                #endregion

                // Show/Hide Bonus label
                #region Bonus Label handle

                ElementProductionBonus elementProductionBonus =
                    _elementMenuDetail.GetElementType().GetElementProductionBonus(elementLevel);
                if (elementProductionBonus != null)
                {
                    _mainWindow.building_bonus_label.Visibility = Visibility.Visible;

                    if (!elementProductionBonus.IsRessourcesBonus)
                    {
                        _mainWindow.building_detail_bonus.Content = String.Format("{0}%",
                            elementProductionBonus.GetFirstNotNull());
                    }
                }
                else
                {
                    _mainWindow.building_bonus_label.Visibility = Visibility.Hidden;
                    _mainWindow.building_detail_bonus.Content = "";
                }

                elementProductionBonus =
                    _elementMenuDetail.GetElementType().GetElementProductionBonus(elementLevel + 1);
                if (elementProductionBonus != null)
                {

                    _mainWindow.bonus_dockpanel.Visibility = Visibility.Visible;

                    if (!elementProductionBonus.IsRessourcesBonus)
                    {
                        _mainWindow.building_detail_bonus_next.Content = String.Format("{0}%",
                            elementProductionBonus.GetFirstNotNull());
                    }
                }
                else
                {
                    _mainWindow.bonus_dockpanel.Visibility = Visibility.Collapsed;
                    _mainWindow.building_detail_bonus_next.Content = "";
                }

                #endregion

                // Show/Hide # ressources around element
                #region Label for # ressources around

                if (_elementMenuDetail.NbRessourcesAround > 0)
                {
                    _mainWindow.ressources_bonus_dockpanel.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_ressource_bonus.Content =
                        _elementMenuDetail.NbRessourcesAround;
                }
                else
                {
                    _mainWindow.ressources_bonus_dockpanel.Visibility = Visibility.Collapsed;
                }

                #endregion

                // Show/Hide # fields count
                #region Fields Count
                if (_elementMenuDetail.FieldsCount > 0)
                {
                    _mainWindow.fields_count_dockpanel.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_fields_count.Content = _elementMenuDetail.FieldsCount;
                }
                else
                {
                    _mainWindow.fields_count_dockpanel.Visibility = Visibility.Collapsed;
                }
                #endregion

                // Show/Hide total bonus on element
                #region Total bonus
                if (_elementMenuDetail.TotalBonus > 99)
                {
                    _mainWindow.total_bonus_dockpanel.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_total_bonus.Content = (_elementMenuDetail.TotalBonus - 100) + "%";
                }
                else
                {
                    _mainWindow.total_bonus_dockpanel.Visibility = Visibility.Collapsed;
                } 
                #endregion

                // Show/hide delete button
                _mainWindow.delete_element_button.Visibility = (elementType.GetElementType() == ElementType.Type.BUILDING_TOWNHALL) 
                    ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                // it's not a building (no level)
                _mainWindow.building_detail_level_info.Visibility = Visibility.Collapsed;
            }
        }

    }
}
