using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima.Error
{
    class LoginError
    {
        public enum errors
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

        public static string getErrorValue(errors error)
        {
            string errorValue = "";

            switch(error)
            {
                case errors.USERNAME_INVALID_FORMAT:
                    errorValue = "Username has special chars";
                    break;
                case errors.USERNAME_ALREADY_EXIST:
                    errorValue = "Username already exist";
                    break;
                case errors.USERNAME_TOO_SHORT:
                    errorValue = "Must be longer than 5 chars";
                    break;
                case errors.USERNAME_TOO_LONG:
                    errorValue = "Max 30 characters";
                    break;
                case errors.PASSWORD_INVALID_FORMAT:
                    errorValue = "One Upper case & one letter";
                    break;
                case errors.PASSWORD_TOO_SHORT:
                    errorValue = "Must be longer than 6 chars";
                    break;
                case errors.PASSWORD_DONT_MATCH:
                    errorValue = "Passwords don't match";
                    break;
                case errors.EMAIL_INVALID_FORMAT:
                    errorValue = "Email is invalid";
                    break;
                case errors.EMAIL_ALREADY_EXIST:
                    errorValue = "Email is already being used";
                    break;
                case errors.SIGNUP_SUCCESSFUL:
                    errorValue = "Signup successful";
                    break;
                case errors.LOGIN_FAILED:
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
