using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using LordOfUltima.Error;
using LordOfUltima.Events;
using LordOfUltima.Events.MainWindowEvents;
using LordOfUltima.MGameboard;
using LordOfUltima.Music;
using LordOfUltima.Research;
using LordOfUltima.RessourcesProduction;
using LordOfUltima.RessourcesStorage;
using LordOfUltima.Units;
using LordOfUltima.Units.Units;
using LordOfUltima.Views;
using LordOfUltima.XML;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Gameboard _gameboard;
        private readonly LevelIndicatorVisibility _levelIndicatorVisibility;
        private readonly BuildingMenuVisibility _buildingMenuVisibility;
        private readonly BuildingDetailsVisibility _buildingDetailsVisibility;
        private readonly ChatEvents _chatEvents;
        private readonly MusicPlayer _musicPlayer;
        private readonly ResearchHandler _researchHandler;
        private readonly RecruitmentManager _recruitmentManager;

        public static MainWindow MIns;
        public MainWindow()
        {
            MIns = this;
            InitializeComponent();

            // Set the music to play
            _musicPlayer = MusicPlayer.Instance;
            _musicPlayer.Play("Resources/Audio/main_theme.mp3");

            // Set the gameboard Instance
            _gameboard = Gameboard.Instance;
            ResetMap.Instance.VerifyMap();

            // Set the user instance
            label_player_name.Content = User.User.Instance.Name;

            // Dispatcher pour programme Idle
            ComponentDispatcher.ThreadIdle += IdleUiThread.Instance.IdleThreadWork;

            // Insert images in UI
            UIImagesInit.Instance.InitImages();

            // Insertion des elements dans la carte
            GameboardInit.Instance.InsertMap();

            // Level indicators
            _levelIndicatorVisibility = LevelIndicatorVisibility.Instance;

            // Hide building menu
            _buildingMenuVisibility = BuildingMenuVisibility.Instance;
            _buildingMenuVisibility.HideBuildingMenu();

            // Hide Building Details menu
            _buildingDetailsVisibility = BuildingDetailsVisibility.Instance;
            _buildingDetailsVisibility.HideBuildingDetails();

            // Initialise Research
            _researchHandler = ResearchHandler.Instance;
            _researchHandler.Initialise();

            // Disable Unit recruitment
            UnitRecruitmentVisibility.Instance.DisableAllUnits();
            
            // load game
            if (!SaveGame.Instance.Load())
            {
                // If no game is found, load a new one
                ResetMap.Instance.InitialiseNewGame();

                // Set default ressources
                Ressources.Instance.Initialise();
                Ressources.Instance.SetDefault();

                // Count Units
                UnitManager.Instance.UpdateUnitCount();
            }

            // Start Error dispatching
            ErrorManager.Instance.StartErrorDispatch();

            // First update of ressources
            RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

            // Check Building count with townhall
            BuildingCount.Instance.CountBuildings();

            // Update Storage
            Storage.Instance.UpdateStorageCapacity();

            // Start Ressource management
            RessourcesManager.Instance.StartRessourcesManager();
            RessourcesManager.Instance.TimeScale = LordOfUltima.Properties.Settings.Default.UpdateTime;

            // Settings initialisation
            SetSettings.Instance.Set();

            // Hide research
            ResearchPageVisibility.HideResearchPage();

            // Hide recruitment
            RecruitmentPageVisibility.HideRecruitmentPage();

            // Chatbox state
            ChatboxVisibility.Instance.HandleChatboxVisibility();

            // Start Recruitment Manager
            _recruitmentManager = RecruitmentManager.Instance;

            // Update recruitment window visibility
            UnitRecruitmentVisibility.Instance.UpdateVisibility();

            // Start Chat thread
            /*_chatEvents = ChatEvents.Instance;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += _chatEvents.UpdateChat;
            bw.RunWorkerAsync();*/
        }

        #region Map Mouvement
        const double ScaleRate = 1.05;
        const double ScaleMin = 1.0;
        const double ScaleMax = 2.5;
        private Point _point = new Point(0, 0);
        private double _scale = 1;
        private bool _isset;
        /*
         * Gestion du scroll Wheel dans le canvas
        */
        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && !_isset)
            {
                _isset = true;
                _point = e.MouseDevice.GetPosition(canvas1);
            }

            Matrix m = canvas1.RenderTransform.Value;
            if (e.Delta > 0)
            {
                if(_scale * ScaleRate > ScaleMax)
                {
                    return;
                }
                m.ScaleAtPrepend(ScaleRate, ScaleRate, _point.X, _point.Y);
                _scale *= ScaleRate;
            }
            else
            {
                if (_scale / ScaleRate < ScaleMin)
                {
                    _isset = false;
                    return;
                }
                m.ScaleAtPrepend(1 / ScaleRate, 1 / ScaleRate, _point.X, _point.Y);
                _scale /= ScaleRate;
            }
            canvas1.RenderTransform = new MatrixTransform(m);
        }

        /*
         * Gestion du drag
        */
        private bool _isMouseDown;
        private static bool _isMouseMove;
        private Point _startMousePos;
        private Point _oldPos = new Point(0, 0);
        private void canvas1_leftMouseDown(object sender, RoutedEventArgs e)
        {
            _isMouseDown = true;
            // Reinitialiser le mouve a false au click down
            _isMouseMove = false;
            _totalMove = 0.0;

            _startMousePos = Mouse.GetPosition(canvas_mouse_pos);
            canvas1.CaptureMouse();
        }

        private void canvas1_leftMouseUp(object sender, RoutedEventArgs e)
        {
            _isMouseDown = false;
            canvas1.ReleaseMouseCapture();
            _oldPos.X = Canvas.GetLeft(canvas1);
            _oldPos.Y = Canvas.GetTop(canvas1);
        }

        public static bool GetIsMouseMove()
        {
            return _isMouseMove;
        }
        /*
         * Gestion du deplacement de la souris sur la carte
        */
        private double _totalMove;
        private void canvas1_mouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown) return;
            
            // Si on se deplace de plus d'un certain nombre de pixel, on ignore le click up pour la selection d'un batiment
            if(_totalMove > 5)
            {
                _isMouseMove = true;
            }

            Point currentMousePos = Mouse.GetPosition(canvas_mouse_pos);

            double maxDeplacement = 100 * Math.Pow(_scale,3);
            double dx = _oldPos.X + (currentMousePos.X - _startMousePos.X);
            double dy = _oldPos.Y + (currentMousePos.Y - _startMousePos.Y);

            // Calcul du mouse move totale
            _totalMove += dx + dy;

            if (Math.Abs(dx) < maxDeplacement )
                Canvas.SetLeft(canvas1, dx);

            if (Math.Abs(dy) < maxDeplacement)
                Canvas.SetTop(canvas1, dy);
        }
        #endregion

        /*
         * Gestion entrer clavier pour le chat text input
        */
        private void chat_text_KeyDown(object sender, KeyEventArgs e)
        {
            _chatEvents.ChatKeyDown(sender,e);

        }

        #region Menu element click
        /*
         * Trigger building level indicator on map
        */
        private void trigger_level_Click(object sender, RoutedEventArgs e)
        {
            _levelIndicatorVisibility.HandleLevelIndicatorVisibility();
            Properties.Settings.Default.IsBuildingLevelVisible = trigger_level.IsChecked;
        }


        private void trigger_dark_Click(object sender, RoutedEventArgs e)
        {
            UIImagesInit.Instance.TriggerDarkTheme();
        }

        /*
         * Reset the current map
        */
        private void reset_map_Click(object sender, RoutedEventArgs e)
        {
            // Show messagebox for confirmation
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to reset map?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ResetMapElements.Instance.ResetMap();
                // Init a new game
                ResetMap.Instance.InitialiseNewGame();

                // set count
                BuildingCount.Instance.CountBuildings();

                _buildingMenuVisibility.HideBuildingMenu();
                _buildingDetailsVisibility.HideBuildingDetails();

                _levelIndicatorVisibility.HideLevelIndicator();
                _levelIndicatorVisibility.HandleLevelIndicatorVisibility();
            }
        }

        /*
         * Logout of the game
        */
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            foreach (Element element in _gameboard.GetMap())
            {
                canvas1.Children.Remove(element.GetElement());
                canvas1.Children.Remove(element.GetLevelElement());
                canvas1.Children.Remove(element.GetLevelLabel());
                canvas1.Children.Remove(element.GetSelectElement());
            }
            ResetMapElements.Instance.ResetMap();

            LoginWindow window = new LoginWindow();
            window.Show();
            Close();
        }

        /*
         * Save Game
        */
        private void game_save_Click(object sender, RoutedEventArgs e)
        {
            SaveGame.Instance.Save();
        }

        /*
         * Load the last saved game
        */
        private void game_load_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to load the last save?", "Load Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (SaveGame.Instance.SaveExist())
                {
                    ResetMapElements.Instance.ResetMap();
                    SaveGame.Instance.Load();
                }
                else
                {
                    ErrorManager.Instance.AddError(new Error.Error(){Description = Error.Error.Type.SAVED_GAME_NOT_FOUND});
                }
            }
        }

        /*
         * Configuration box for ressources update time
        */
        private void ressource_update_time_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateTimeOption.Instance == null)
            {
                UpdateTimeOption timeOption = new UpdateTimeOption();
                timeOption.Show();
            }
            else
            {
                UpdateTimeOption.Instance.Activate();
            }
        }

        private void music_options_Click(object sender, RoutedEventArgs e)
        {
            if (MusicOption.Instance == null)
            {
                MusicOption musicOption = new MusicOption();
                musicOption.Show();
            }
            else
            {
                MusicOption.Instance.Activate();
            }
        }
        #endregion

        #region SideMenu building Click
        private readonly BuildEvent _buildEvent = BuildEvent.Instance;
        private void building_woodcutter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new WoodcutterElementType());
        }

        private void building_sawmill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new SawmillElementType());
        }

        private void building_quarry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new QuarryElementType());
        }

        private void building_stonemason_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new StoneMasonElementType());
        }

        private void building_ironmine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new IronMineElementType());
        }

        private void building_foundry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new FoundryElementType());
        }

        private void building_farm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new FarmElementType());
        }

        private void building_mill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new MillElementType());
        }

        private void building_townhouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new TownhouseElementType());
        }

        private void building_marketplace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new MarketplaceElementType());
        }

        private void building_research_center_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new ResearchCenterElementType());
        }

        private void building_storage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new WarehouseElementType());
        }

        private void BuildElement(IElementType element)
        {
            _buildEvent.SetTypeToBuild(element);
            if (_buildEvent.BuildElement())
            {
                _buildEvent.SetElementToBuild(null);
                BuildingMenuVisibility.Instance.HideBuildingMenu();                
            }
        }
        #endregion

        // update instance on windows closing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MIns = null;
            // Stop music player
            _musicPlayer.Stop();

            // close music window
            if (MusicOption.Instance != null)
            {
                MusicOption.Instance.Close();
            }

            // Save settings
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Upgrade button click
            if (UpgradeElement.Instance.Upgrade())
            {
                BuildingDetailsVisibility.Instance.ShowBuildingDetails();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DeleteElement.Instance.Delete();
            }
        }

        /*
         * Minimise chatbox
         */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ChatboxVisibility.Instance.MinimizeChatbox();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ChatboxVisibility.Instance.OpenChatbox();
        }

        // Close element description window
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            _buildingDetailsVisibility.HideBuildingDetails();
            ResetMapElementBorder.Instance.ResetSelectionBorder();
        }

        #region Research page visibility
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            RecruitmentPageVisibility.HideRecruitmentPage();
            ResearchPageVisibility.InvertResearchPageVisibility();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ResearchPageVisibility.HideResearchPage();
        }
        #endregion

        #region Unit Recruitment page visibility
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            ResearchPageVisibility.HideResearchPage();
            RecruitmentPageVisibility.InvertRecruitmentPageVisibility();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            RecruitmentPageVisibility.HideRecruitmentPage();
        }
        #endregion

        #region Research upgrade buttons
        private void research_wood_button_Click(object sender, RoutedEventArgs e)
        {
            _researchHandler.UpdateResearch(_researchHandler.WoodResearchType);
        }

        private void research_stone_button_Click(object sender, RoutedEventArgs e)
        {
            _researchHandler.UpdateResearch(_researchHandler.StoneResearchType);
        }

        private void research_iron_button_Click(object sender, RoutedEventArgs e)
        {
            _researchHandler.UpdateResearch(_researchHandler.IronResearchType);
        }

        private void research_food_button_Click(object sender, RoutedEventArgs e)
        {
            _researchHandler.UpdateResearch(_researchHandler.FoodResearchType);
        }

        private void research_gold_button_Click(object sender, RoutedEventArgs e)
        {
            _researchHandler.UpdateResearch(_researchHandler.GoldResearchType);
        }
        #endregion

        #region Dungeon Visibility
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            DungeonVisibility.Instance.ShowDungeon();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            DungeonVisibility.Instance.HideDungeon();
        }
        #endregion

        #region Units/Ressources building visibility
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            BuildingMenuVisibility.Instance.ShowBuildingRessourcesMenu();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            BuildingMenuVisibility.Instance.ShowBuildingUnitsMenu();
        }
        #endregion

        #region Military Building click
        private void building_barrack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new BarrackBuilding());
        }

        private void building_cityguard_house_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new CityguardBuilding());
        }

        private void building_training_ground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new TrainingGroundBuilding());
        }

        private void building_stable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new StableBuilding());
        }

        private void building_moonglow_tower_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new MoonglowTowerBuilding());
        }
        #endregion

        #region Recruitment Window
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            RecruitmentManager.Instance.CheckAllFields();
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            var match = !regex.IsMatch(text);
            return match;
        }

        // Incr recruitment cityguard
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(cityguard_recruitment_count.Text);
            _recruitmentManager.IncrCount(cityguard_recruitment_count, UnitEntity.Cityguard, count);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(cityguard_recruitment_count.Text);
            _recruitmentManager.DecrCount(cityguard_recruitment_count, UnitEntity.Cityguard, count);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(berserker_recruitment_count.Text);
            _recruitmentManager.IncrCount(berserker_recruitment_count, UnitEntity.Berserker, count);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(berserker_recruitment_count.Text);
            _recruitmentManager.DecrCount(berserker_recruitment_count, UnitEntity.Berserker, count);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(crossbow_recruitment_count.Text);
            _recruitmentManager.IncrCount(crossbow_recruitment_count, UnitEntity.Crossbow, count);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(crossbow_recruitment_count.Text);
            _recruitmentManager.DecrCount(crossbow_recruitment_count, UnitEntity.Crossbow, count);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(guardian_recruitment_count.Text);
            _recruitmentManager.IncrCount(guardian_recruitment_count, UnitEntity.Guardian, count);
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(guardian_recruitment_count.Text);
            _recruitmentManager.DecrCount(guardian_recruitment_count, UnitEntity.Guardian, count);
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(guardian_recruitment_count.Text);
            _recruitmentManager.IncrCount(guardian_recruitment_count, UnitEntity.Guardian, count);
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(guardian_recruitment_count.Text);
            _recruitmentManager.DecrCount(guardian_recruitment_count, UnitEntity.Guardian, count);
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(paladin_recruitment_count.Text);
            _recruitmentManager.IncrCount(paladin_recruitment_count, UnitEntity.Paladin, count);
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(paladin_recruitment_count.Text);
            _recruitmentManager.DecrCount(paladin_recruitment_count, UnitEntity.Paladin, count);
        }

        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(templar_recruitment_count.Text);
            _recruitmentManager.IncrCount(templar_recruitment_count, UnitEntity.Templar, count);
        }

        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(templar_recruitment_count.Text);
            _recruitmentManager.DecrCount(templar_recruitment_count, UnitEntity.Templar, count);
        }

        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(ranger_recruitment_count.Text);
            _recruitmentManager.IncrCount(ranger_recruitment_count, UnitEntity.Ranger, count);
        }

        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(ranger_recruitment_count.Text);
            _recruitmentManager.DecrCount(ranger_recruitment_count, UnitEntity.Ranger, count);
        }

        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(scout_recruitment_count.Text);
            _recruitmentManager.IncrCount(scout_recruitment_count, UnitEntity.Scout, count);
        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(scout_recruitment_count.Text);
            _recruitmentManager.DecrCount(scout_recruitment_count, UnitEntity.Scout, count);
        }

        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(mage_recruitment_count.Text);
            _recruitmentManager.IncrCount(mage_recruitment_count, UnitEntity.Mage, count);
        }

        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(mage_recruitment_count.Text);
            _recruitmentManager.DecrCount(mage_recruitment_count, UnitEntity.Mage, count);
        }

        private void Button_Click_33(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(warlock_recruitment_count.Text);
            _recruitmentManager.IncrCount(warlock_recruitment_count, UnitEntity.Warlock, count);
        }

        private void Button_Click_34(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(warlock_recruitment_count.Text);
            _recruitmentManager.DecrCount(warlock_recruitment_count, UnitEntity.Warlock, count);
        }
        #endregion
    }
}
