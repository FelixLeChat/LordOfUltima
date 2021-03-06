﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LordOfUltima.MGameboard;
using LordOfUltima.Research;

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
                ImageSource = new BitmapImage(new Uri(_elementMenuDetail.GetElementType().GetDetailImagePath(), UriKind.RelativeOrAbsolute))
            };
            _mainWindow.building_details_img.Fill = image;

            // set the elementInfo
            _mainWindow.building_detail_info.Text = _elementMenuDetail.GetElementType().GetElementInfo();

            int elementLevel = _elementMenuDetail.Level;
            if (elementLevel > 0 && elementLevel <= 10)
            {
                // hide second ressource delete button
                _mainWindow.delete_ressource_button.Visibility = Visibility.Hidden;

                // it's a building, show level info
                _mainWindow.building_detail_level_info.Visibility = Visibility.Visible;
                _mainWindow.building_detail_level.Content = _elementMenuDetail.Level;

                IElementType elementType = _elementMenuDetail.GetElementType();
                if (elementType == null)
                    return;

                if (elementLevel < 10)
                {
                    ElementCost elementCost = elementType.GetElementCost(elementLevel + 1);

                    // Show upgrade button
                    _mainWindow.upgrade_element_button.Visibility = Visibility.Visible;

                    // Show infos
                     _mainWindow.wood_dockpanel.Visibility = Visibility.Visible;
                     _mainWindow.stone_dockpanel.Visibility = Visibility.Visible;
                     _mainWindow.iron_dockpanel.Visibility = Visibility.Visible;

                    // Show cost for upgrade
                    _mainWindow.building_detail_wood_cost.Content = elementCost.Wood;
                    _mainWindow.building_detail_stone_cost.Content = elementCost.Stone;
                    _mainWindow.building_detail_iron_cost.Content = elementCost.Iron;            
                }
                else
                {
                    // hide upgrade button
                    _mainWindow.upgrade_element_button.Visibility = Visibility.Hidden;

                    // hide upgrade qty
                    _mainWindow.wood_dockpanel.Visibility = Visibility.Hidden;
                    _mainWindow.stone_dockpanel.Visibility = Visibility.Hidden;
                    _mainWindow.iron_dockpanel.Visibility = Visibility.Hidden;
                }

                #region Storage Label handle
                ElementStorage elementStorage = elementType.GetElementStorage(elementLevel + 1);
                if (elementStorage != null)
                {
                    _mainWindow.storage_dockpanel.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_storage_next.Content = elementStorage.BaseStorage;
                }
                else
                {
                    _mainWindow.storage_dockpanel.Visibility = Visibility.Hidden;
                }

                elementStorage = elementType.GetElementStorage(elementLevel);
                if (elementStorage != null && elementType.GetElementType() != ElementType.Type.BUILDING_TOWNHALL)
                {
                    _mainWindow.building_storage_label.Visibility = Visibility.Visible;
                    _mainWindow.building_detail_storage.Content = elementStorage.BaseStorage;
                }
                else
                {
                    _mainWindow.building_detail_storage.Content = "";
                    _mainWindow.building_storage_label.Visibility = Visibility.Hidden;
                }
                #endregion

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

                _mainWindow.building_units_label.Visibility = Visibility.Hidden;
                _mainWindow.building_detail_units.Content = "";

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
                else if (_elementMenuDetail.GetElementType().IsMilitary())
                {
                    var militaryElement = (IUnitBuilding) _elementMenuDetail.GetElementType();

                    if (!militaryElement.IsBarrack())
                    {
                        _mainWindow.building_bonus_label.Visibility = Visibility.Visible;
                        _mainWindow.building_detail_bonus.Content = String.Format("{0}%",
                            militaryElement.GetUnitBonus(_elementMenuDetail.Level));
                    }
                    else
                    {
                        _mainWindow.building_units_label.Visibility = Visibility.Visible;
                        _mainWindow.building_detail_units.Content = militaryElement.GetArmySize(_elementMenuDetail.Level);

                        _mainWindow.building_bonus_label.Visibility = Visibility.Hidden;
                        _mainWindow.building_detail_bonus.Content = "";
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
                if (_elementMenuDetail.TotalBonus > 100)
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
               

                if (_elementMenuDetail.GetElementType() != null &&
                    _elementMenuDetail.GetElementType().GetElementType() != ElementType.Type.BUILDING_TOWNHALL)
                {
                    _mainWindow.delete_ressource_button.Visibility = Visibility.Visible;
                }
            }
        }

    }
}
