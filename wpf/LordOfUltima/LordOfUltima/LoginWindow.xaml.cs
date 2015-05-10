﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Background pour grid
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/login_background.jpg", UriKind.Relative));
            grid.Background = imageBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // Close login window and show game window
        private void good_login()
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
