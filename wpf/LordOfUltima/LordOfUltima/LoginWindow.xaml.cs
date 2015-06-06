using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using LordOfUltima.Web;
using LordOfUltima.Error;

namespace LordOfUltima
{
    public partial class LoginWindow
    {
        private readonly User.User _user;
        public LoginWindow()
        {
            InitializeComponent();

            _user = User.User.Instance;

            // Hide both panel
            login_panel.Visibility = Visibility.Hidden;
            signup_panel.Visibility = Visibility.Hidden;

            // Background pour grid
            ImageBrush imageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"Media/login_background.jpg", UriKind.Relative))
            };
            grid.Background = imageBrush;

            // Test
            login_password_textbox.Password = "1111AA";
            login_email_textbox.Text = "felix@felix.ca";
        }


        // Close login window and show game window
        private void good_login()
        {
            // get player name
            _user.Name = Utility.Instance.GetPlayerName(_user.Email);

            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            // Visibility for panels
            login_panel.Visibility = Visibility.Visible;
            signup_panel.Visibility = Visibility.Hidden;

            // Visibility for buttons
            login_button.Visibility = Visibility.Hidden;
            signup_button.Visibility = Visibility.Visible;
        }

        private void signup_button_Click(object sender, RoutedEventArgs e)
        {
            // Visibility for panels
            login_panel.Visibility = Visibility.Hidden;
            signup_panel.Visibility = Visibility.Visible;

            // Visibility for buttons
            signup_button.Visibility = Visibility.Hidden;
            login_button.Visibility = Visibility.Visible;
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
                string loginRespond = Login.Instance.TryLogin(login_email_textbox.Text, login_password_textbox.Password);

                if(checkLoginRespond(loginRespond))
                {
                    // Set the email for the only time
                    _user.Email = email;
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
            string passwordCheck = signup_password_confirm_textbox.Password;

            if (validateSignup(username,email,password,passwordCheck))
            {
                string signupRespond = Login.Instance.Register(email, username, password);

                // check if the signup is a sucess
                if(checkSignupRespond(signupRespond))
                {
                    // Visibility for panels
                    login_panel.Visibility = Visibility.Visible;
                    signup_panel.Visibility = Visibility.Hidden;

                    // Visibility for buttons
                    login_button.Visibility = Visibility.Hidden;
                    signup_button.Visibility = Visibility.Visible;

                    resetSignupFields();
                }
            }

        }

        private bool checkValidEmail(string email)
        {
            // check if email is valid 
            bool validEmail;
            try
            {
                validEmail = Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                validEmail = false;
            }

            return validEmail;
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
                setSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_INVALID_FORMAT));
                isValid = false;
            }
            else if (username.Length < 5)
            {
                setSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_TOO_SHORT));
                isValid = false;
            }
            else if (username.Length > 30)
            {
                setSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_TOO_LONG));
                isValid = false;
            }

            if(password.Length < 6)
            {
                setSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_TOO_SHORT));
                isValid = false;
            }
            else if (password != confirmation)
            {
                setSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_DONT_MATCH));
                isValid = false;
            }
            else if(!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                setSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_INVALID_FORMAT));
                isValid = false;
            }
                
            if(!checkValidEmail(email))
            {
                setSignupEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool validateLogin(string email, string password)
        {
            bool isValid = true;

            if(!checkValidEmail(email))
            {
                setLoginEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }
            else if (!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                setLoginPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool checkSignupRespond(string respond)
        {
            bool isSucess;
            if (respond == "Email already exist")
            {
                setSignupEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Username already exist")
            {
                setSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Sucess")
            {
                isSucess = true;
                setSignupSuccess(LoginError.GetErrorValue(LoginError.Errors.SIGNUP_SUCCESSFUL));
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
                setLoginFail(LoginError.GetErrorValue(LoginError.Errors.LOGIN_FAILED));
                isSucess = false;
            }
            return isSucess;
        }
    }
}
