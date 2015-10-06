using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using LordOfUltima.Error;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Views
{
    /// <summary>
    /// Logique d'interaction pour UpdateTimeOption.xaml
    /// </summary>
    public partial class UpdateTimeOption
    {
        public static UpdateTimeOption Instance;
        public UpdateTimeOption()
        {
            InitializeComponent();

            // Initialise singleton
            Instance = this;

            // Initialise time
            time_choice.Text = RessourcesManager.Instance.TimeScale.ToString();
        }

        private void apply_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(time_choice.Text.Trim());
                RessourcesManager.Instance.TimeScale = value;
                LordOfUltima.Properties.Settings.Default.UpdateTime = value;
                apply_button.IsEnabled = false;
                Close();
            }
            catch (Exception)
            {
                ErrorManager.Instance.AddError(new Error.Error(){Description = Error.Error.Type.INVALID_TIME});
                time_choice.Text = "";
                apply_button.IsEnabled = false;
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void time_choice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instance = null;
        }

        private void one_second_Click(object sender, RoutedEventArgs e)
        {
            time_choice.Text = "1";
        }

        private void one_minute_Click(object sender, RoutedEventArgs e)
        {
            time_choice.Text = "60";
        }

        private void one_hour_Click(object sender, RoutedEventArgs e)
        {
            time_choice.Text = "3600";
        }

        private void time_choice_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            apply_button.IsEnabled = true;
        }

    }
}
