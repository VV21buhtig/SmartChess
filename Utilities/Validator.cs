using System;

namespace SmartChess.Utilities
{
    public static class Validator
    {
        public static bool IsValidLogin(string login)
        {
            return !string.IsNullOrWhiteSpace(login) && login.Length >= 3 && login.Length <= 50;
        }

        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }

        public static bool ArePasswordsEqual(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }
    }
}