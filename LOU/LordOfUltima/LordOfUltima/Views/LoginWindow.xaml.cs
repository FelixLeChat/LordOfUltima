using System;
using System.Windows;
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
            ResetErrorMessage();

            // try to login with the informations
            string email = login_email_textbox.Text;
            string password = login_password_textbox.Password;

            if(ValidateLogin(email,password))
            {
                string loginRespond = Login.Instance.TryLogin(login_email_textbox.Text, login_password_textbox.Password);

                if(CheckLoginRespond(loginRespond))
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
            else
            {
                login_password_textbox.Password = "";
            }
                
        }

        private void signup_panel_button_Click(object sender, RoutedEventArgs e)
        {
            // Reset error messages
            ResetErrorMessage();

            string email = signup_email_textbox.Text;
            string username = signup_username_textbox.Text;
            string password = signup_password_textbox.Password;
            string passwordCheck = signup_password_confirm_textbox.Password;

            if (ValidateSignup(username,email,password,passwordCheck))
            {
                string signupRespond = Login.Instance.Register(email, username, password);

                // check if the signup is a sucess
                if(CheckSignupRespond(signupRespond))
                {
                    // Visibility for panels
                    login_panel.Visibility = Visibility.Visible;
                    signup_panel.Visibility = Visibility.Hidden;

                    // Visibility for buttons
                    login_button.Visibility = Visibility.Hidden;
                    signup_button.Visibility = Visibility.Visible;

                    ResetSignupFields();
                }
            }

        }

        private bool CheckValidEmail(string email)
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

        private void SetSignupUsernameError(string error)
        {
            signup_username_error.Content = error;
        }
        private void SetSignupEmailError(string error)
        {
            signup_email_error.Content = error;
        }
        private void SetSignupPasswordError(string error)
        {
            signup_password_error.Content = error;
        }
        private void SetSignupSuccess(string error)
        {
            signup_sucess.Content = error;
        }
        private void SetLoginEmailError(string error)
        {
            login_email_error.Content = error;
        }
        private void SetLoginPasswordError(string error)
        {
            login_password_error.Content = error;
        }
        private void SetLoginFail(string error)
        {
            login_fail.Content = error;
        }
        private void ResetErrorMessage()
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

        private void ResetSignupFields()
        {
            signup_email_textbox.Text = "";
            signup_username_textbox.Text = "";
            signup_password_textbox.Password = "";
            signup_password_confirm_textbox.Password = "";
        }

        private void ResetPasswordFields()
        {
            signup_password_textbox.Password = "";
            signup_password_confirm_textbox.Password = "";
        }

        private bool ValidateSignup(string username, string email, string password, string confirmation)
        {
            bool isValid = true;

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]*$"))
            {
                SetSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_INVALID_FORMAT));
                isValid = false;
            }
            else if (username.Length < 5)
            {
                SetSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_TOO_SHORT));
                isValid = false;
            }
            else if (username.Length > 30)
            {
                SetSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_TOO_LONG));
                isValid = false;
            }

            if(password.Length < 6)
            {
                SetSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_TOO_SHORT));
                ResetPasswordFields();
                isValid = false;
            }
            else if (password != confirmation)
            {
                SetSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_DONT_MATCH));
                ResetPasswordFields();
                isValid = false;
            }
            else if(!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                SetSignupPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_INVALID_FORMAT));
                ResetPasswordFields();
                isValid = false;
            }
                
            if(!CheckValidEmail(email))
            {
                SetSignupEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateLogin(string email, string password)
        {
            bool isValid = true;

            if(!CheckValidEmail(email))
            {
                SetLoginEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_INVALID_FORMAT));
                isValid = false;
            }
            else if (!Regex.IsMatch(password, "[0-9]{1}") || !Regex.IsMatch(password, "[A-Z]{1}"))
            {
                SetLoginPasswordError(LoginError.GetErrorValue(LoginError.Errors.PASSWORD_INVALID_FORMAT));
                isValid = false;
            }

            return isValid;
        }

        private bool CheckSignupRespond(string respond)
        {
            bool isSucess;
            if (respond == "Email already exist")
            {
                SetSignupEmailError(LoginError.GetErrorValue(LoginError.Errors.EMAIL_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Username already exist")
            {
                SetSignupUsernameError(LoginError.GetErrorValue(LoginError.Errors.USERNAME_ALREADY_EXIST));
                isSucess = false;
            }
            else if (respond == "Sucess")
            {
                isSucess = true;
                SetSignupSuccess(LoginError.GetErrorValue(LoginError.Errors.SIGNUP_SUCCESSFUL));
            }
            else
            {
                isSucess = false;
                SetSignupUsernameError("Unknow error");
            }
            return isSucess;
        }

        private bool CheckLoginRespond(string respond)
        {
            bool isSucess;

            if(respond == "LOGIN SUCESS")
            {
                isSucess = true;
            }
            else
            {
                SetLoginFail(LoginError.GetErrorValue(LoginError.Errors.LOGIN_FAILED));
                isSucess = false;
            }
            return isSucess;
        }

        private void offline_Click(object sender, RoutedEventArgs e)
        {
            User.User.Instance.Email = "offline@offline.com";
            User.User.Instance.Name = "Offline";
            good_login();
        }
    }
}
