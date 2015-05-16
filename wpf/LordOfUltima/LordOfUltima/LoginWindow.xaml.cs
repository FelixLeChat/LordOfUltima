using System;
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
using System.Text.RegularExpressions;

using LordOfUltima.Web;
using LordOfUltima.Error;
using LordOfUltima.User;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private User.User m_user;
        public LoginWindow()
        {
            InitializeComponent();

            m_user = User.User.getInstance();

            // Hide both panel
            login_panel.Visibility = System.Windows.Visibility.Hidden;
            signup_panel.Visibility = System.Windows.Visibility.Hidden;

            // Background pour grid
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/login_background.jpg", UriKind.Relative));
            grid.Background = imageBrush;

            // Test
            login_password_textbox.Password = "1111AA";
            login_email_textbox.Text = "felix@felix.ca";
        }


        // Close login window and show game window
        private void good_login()
        {
            // get player name
            m_user.Name = Utility.getInstance().getPlayerName(m_user.Email);

            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            // Visibility for panels
            login_panel.Visibility = System.Windows.Visibility.Visible;
            signup_panel.Visibility = System.Windows.Visibility.Hidden;

            // Visibility for buttons
            login_button.Visibility = System.Windows.Visibility.Hidden;
            signup_button.Visibility = System.Windows.Visibility.Visible;
        }

        private void signup_button_Click(object sender, RoutedEventArgs e)
        {
            // Visibility for panels
            login_panel.Visibility = System.Windows.Visibility.Hidden;
            signup_panel.Visibility = System.Windows.Visibility.Visible;

            // Visibility for buttons
            signup_button.Visibility = System.Windows.Visibility.Hidden;
            login_button.Visibility = System.Windows.Visibility.Visible;
        }

        private void login_panel_button_Click(object sender, RoutedEventArgs e)
        {
            // Reset error message
            resetErrorMessage();

            // try to login with the informations
            string email = login_email_textbox.Text;
            string password = login_password_textbox.Password;

            if(validateLogin(email,password))
            {
                string loginRespond = Login.getInstance().login(login_email_textbox.Text, login_password_textbox.Password);

                if(checkLoginRespond(loginRespond))
                {
                    // Set the email for the only time
                    m_user.Email = email;
                    good_login();
                }
                else
                {
                    login_password_textbox.Password = "";
                }
            }
                
        }

        private void signup_panel_button_Click(object sender, RoutedEventArgs e)
        {
            // Reset error messages
            resetErrorMessage();

            string email = signup_email_textbox.Text;
            string username = signup_username_textbox.Text;
            string password = signup_password_textbox.Password;
            string password_check = signup_password_confirm_textbox.Password;

            if (validateSignup(username,email,password,password_check))
            {
                string signupRespond = Login.getInstance().register(email, username, password);

                // check if the signup is a sucess
                if(checkSignupRespond(signupRespond))
                {
                    // Visibility for panels
                    login_panel.Visibility = System.Windows.Visibility.Visible;
                    signup_panel.Visibility = System.Windows.Visibility.Hidden;

                    // Visibility for buttons
                    login_button.Visibility = System.Windows.Visibility.Hidden;
                    signup_button.Visibility = System.Windows.Visibility.Visible;

                    resetSignupFields();
                }
            }

        }

        private bool checkValidEmail(string email)
        {
            // check if email is valid 
            bool valid_email = false;
            try
            {
                valid_email = Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                valid_email = false;
            }

            return valid_email;
        }

        private void setSignupUsernameError(string error)
        {
            signup_username_error.Content = error;
        }
        private void setSignupEmailError(string error)
        {
            signup_email_error.Content = error;
        }
        private void setSignupPasswordError(string error)
        {
            signup_password_error.Content = error;
        }
        private void setSignupSuccess(string error)
        {
            signup_sucess.Content = error;
        }
        private void setLoginEmailError(string error)
        {
            login_email_error.Content = error;
        }
        private void setLoginPasswordError(string error)
        {
            login_password_error.Content = error;
        }
        private void setLoginFail(string error)
        {
            login_fail.Content = error;
        }
        private void resetErrorMessage()
        {
            // Signup page
            signup_username_error.Content = "";
            signup_email_error.Content = "";
            signup_password_error.Content = "";
            // Login page
            signup_sucess.Content = "";
            login_password_error.Content = "";
            login_email_error.Content = "";
            login_fail.Content = "";
        }

        private void resetSignupFields()
        {
            signup_email_textbox.Text = "";
            signup_username_textbox.Text = "";
            signup_password_textbox.Password = "";
            signup_password_confirm_textbox.Password = "";
        }

        private bool validateSignup(string username, string email, string password, string confirmation)
        {
            bool isValid = true;

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]*$"))
            {
                setSignupUsernameError(LoginError.getErrorValue(LoginError.errors.USERNAME_INVALID_FORMAT));
                isValid = false;
            }
            else if (username.Length < 5)
            {
                setSignupUsernameError(LoginError.getErrorValue(LoginError.errors.USERNAME_TOO_SHORT));
                isValid = false;
            }
            else if (username.Length > 30)
            {
                setSignupUsernameError(LoginError.getErrorValue(LoginError.errors.USERNAME_TOO_LONG));
                isValid = false;
            }

            if(password.Length < 6)
            {
                setSignupPasswordError(LoginError.getErrorValue(LoginError.errors.PASSWORD_TOO_SHORT));
                isValid = false;
            }
            else if (password != confirmation)
            {
                setSignupPasswordError(LoginError.getErrorValue(LoginError.errors.PASSWORD_DONT_MATCH));
                isValid = false;
            }
            else if(!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                setSignupPasswordError(LoginError.getErrorValue(LoginError.errors.PASSWORD_INVALID_FORMAT));
                isValid = false;
            }
                
            if(!checkValidEmail(email))
            {
                setSignupEmailError(LoginError.getErrorValue(LoginError.errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool validateLogin(string email, string password)
        {
            bool isValid = true;

            if(!checkValidEmail(email))
            {
                setLoginEmailError(LoginError.getErrorValue(LoginError.errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }
            else if (!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                setLoginPasswordError(LoginError.getErrorValue(LoginError.errors.PASSWORD_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool checkSignupRespond(string respond)
        {
            bool isSucess;
            if (respond == "Email already exist")
            {
                setSignupEmailError(LoginError.getErrorValue(LoginError.errors.EMAIL_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Username already exist")
            {
                setSignupUsernameError(LoginError.getErrorValue(LoginError.errors.USERNAME_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Sucess")
            {
                isSucess = true;
                setSignupSuccess(LoginError.getErrorValue(LoginError.errors.SIGNUP_SUCCESSFUL));
            }
            else
            {
                isSucess = false;
                setSignupUsernameError("Unknow error");
            }
            return isSucess;
        }

        private bool checkLoginRespond(string respond)
        {
            bool isSucess;

            if(respond == "LOGIN SUCESS")
            {
                isSucess = true;
            }
            else
            {
                setLoginFail(LoginError.getErrorValue(LoginError.errors.LOGIN_FAILED));
                isSucess = false;
            }
            return isSucess;
        }
    }
}
