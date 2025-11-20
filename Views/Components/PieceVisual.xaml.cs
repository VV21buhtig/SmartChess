using System.Windows;
using System.Windows.Controls;

namespace SmartChess.Views.Components
{
    public partial class PieceVisual : UserControl
    {
        public PieceVisual()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(PieceVisual));

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }
    }
}