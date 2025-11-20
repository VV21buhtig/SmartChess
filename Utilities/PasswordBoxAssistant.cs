// Utilities/PasswordBoxAssistant.cs
using System.Windows;
using System.Windows.Controls;

namespace SmartChess.Utilities
{
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant),
            new PropertyMetadata(false));

        public static string GetBoundPassword(PasswordBox passwordBox)
        {
            return (string)passwordBox.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(PasswordBox passwordBox, string value)
        {
            passwordBox.SetValue(BoundPasswordProperty, value);
        }

        private static bool GetUpdatingPassword(PasswordBox passwordBox)
        {
            return (bool)passwordBox.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(PasswordBox passwordBox, bool value)
        {
            passwordBox.SetValue(UpdatingPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                // Если мы уже обновляем пароль из кода, выходим чтобы избежать цикла
                if (GetUpdatingPassword(passwordBox))
                    return;

                // Отписываемся от события чтобы избежать рекурсии
                passwordBox.PasswordChanged -= HandlePasswordChanged;

                // Устанавливаем новый пароль
                passwordBox.Password = e.NewValue?.ToString() ?? string.Empty;

                // Снова подписываемся на событие
                passwordBox.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                // Устанавливаем флаг что мы обновляем пароль из UI
                SetUpdatingPassword(passwordBox, true);

                try
                {
                    // Обновляем BoundPassword свойство
                    SetBoundPassword(passwordBox, passwordBox.Password);
                }
                finally
                {
                    // Снимаем флаг
                    SetUpdatingPassword(passwordBox, false);
                }
            }
        }
    }
}