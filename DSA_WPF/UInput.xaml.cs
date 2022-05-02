using System.Windows;
using System.Windows.Controls;

namespace DSA_WPF
{
    /// <summary>
    /// Interaction logic for UInput.xaml
    /// </summary>
    public partial class UInput : UserControl
    {
        public UInput()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(UInput), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            "Hint", typeof(string), typeof(UInput), new PropertyMetadata(default(string)));

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

    }
}
