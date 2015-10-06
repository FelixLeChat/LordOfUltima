namespace LordOfUltima.Error
{
    class LoginError
    {
        public enum Errors
        {
            USERNAME_INVALID_FORMAT = 0,
            USERNAME_ALREADY_EXIST,
            USERNAME_TOO_SHORT,
            USERNAME_TOO_LONG,

            // We permit special char in password, must at least contain:
            // - One upper case
            // - One number

            PASSWORD_INVALID_FORMAT,
            PASSWORD_TOO_SHORT,
            PASSWORD_DONT_MATCH,

            EMAIL_INVALID_FORMAT,
            EMAIL_ALREADY_EXIST,

            LOGIN_FAILED,
            SIGNUP_SUCCESSFUL
        };

        public static string GetErrorValue(Errors error)
        {
            string errorValue = "";

            switch(error)
            {
                case Errors.USERNAME_INVALID_FORMAT:
                    errorValue = "Username has special chars";
                    break;
                case Errors.USERNAME_ALREADY_EXIST:
                    errorValue = "Username already exist";
                    break;
                case Errors.USERNAME_TOO_SHORT:
                    errorValue = "Must be longer than 5 chars";
                    break;
                case Errors.USERNAME_TOO_LONG:
                    errorValue = "Max 30 characters";
                    break;
                case Errors.PASSWORD_INVALID_FORMAT:
                    errorValue = "One Upper case & one letter";
                    break;
                case Errors.PASSWORD_TOO_SHORT:
                    errorValue = "Must be longer than 6 chars";
                    break;
                case Errors.PASSWORD_DONT_MATCH:
                    errorValue = "Passwords don't match";
                    break;
                case Errors.EMAIL_INVALID_FORMAT:
                    errorValue = "Email is invalid";
                    break;
                case Errors.EMAIL_ALREADY_EXIST:
                    errorValue = "Email is already being used";
                    break;
                case Errors.SIGNUP_SUCCESSFUL:
                    errorValue = "Signup successful";
                    break;
                case Errors.LOGIN_FAILED:
                    errorValue = "Login Failed";
                    break;
                default:
                    errorValue = "Error not found";
                    break;
            }

            return errorValue;
        }
    }
}
