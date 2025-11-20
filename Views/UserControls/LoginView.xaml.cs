using SmartChess.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace SmartChess.Views.UserControls
{
    public partial class LoginView : UserControl
    {
        private readonly AuthViewModel? _viewModel;

        public LoginView()
        {
            InitializeComponent();
            System.Diagnostics.Trace.WriteLine($"LoginView DataContext: {DataContext?.GetType()}");

            this.Loaded += (s, e) =>
            {
                Debug.WriteLine($"LoginView DataContext: {DataContext}");
                Debug.WriteLine($"LoginView IsRegisterMode: {(DataContext as AuthViewModel)?.IsRegisterMode}");
            };

            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is AuthViewModel vm)
                    vm.Password = PasswordBox.Password;
            };

            ConfirmPasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is AuthViewModel vm)
                    vm.ConfirmPassword = ConfirmPasswordBox.Password;
            };
        }

        public LoginView(AuthViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}