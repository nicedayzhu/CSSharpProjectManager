using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using Avalonia;

namespace CSSharpProjectManager.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private async void OnCopyVersionClicked(object? sender, RoutedEventArgs e)
        {
            var version = "1.0.0"; // 可根据需要动态获取
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard != null)
                await clipboard.SetTextAsync(version);
        }

        private void OnWebsiteClicked(object? sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://bbs.csgocn.net/thread-1049.htm",
                UseShellExecute = true
            });
        }

        private void OnGitHubClicked(object? sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/nicedayzhu",
                UseShellExecute = true
            });
        }
    }
} 