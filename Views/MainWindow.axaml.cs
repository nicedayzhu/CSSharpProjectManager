using Avalonia.Controls;
using Avalonia.Input;
using System.Diagnostics;
using Avalonia.Interactivity;

namespace CSSharpProjectManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAboutClicked(object? sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog(this);
        }
    }
}