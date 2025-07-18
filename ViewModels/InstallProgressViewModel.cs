using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CSSharpProjectManager.ViewModels
{
    public partial class InstallProgressViewModel : ObservableObject
    {
        private readonly Action _closeWindowAction;
        private readonly StringBuilder _log = new StringBuilder();
        public string Log => _log.ToString();

        [ObservableProperty]
        private bool _canClose;

        public IRelayCommand CloseCommand { get; }

        public InstallProgressViewModel(Action closeWindowAction)
        {
            _closeWindowAction = closeWindowAction;
            CloseCommand = new RelayCommand(() => _closeWindowAction(), () => CanClose);
        }

        public async Task<bool> InstallTemplateAsync()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "new install CounterStrikeSharpTemplates",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    }
                };
                process.OutputDataReceived += (s, e) => { if (e.Data != null) { _log.AppendLine(e.Data); OnPropertyChanged(nameof(Log)); } };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) { _log.AppendLine(e.Data); OnPropertyChanged(nameof(Log)); } };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();
                CanClose = true;
                CloseCommand.NotifyCanExecuteChanged();
                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                _log.AppendLine($"发生异常: {ex.Message}");
                OnPropertyChanged(nameof(Log));
                CanClose = true;
                CloseCommand.NotifyCanExecuteChanged();
                return false;
            }
        }
    }
} 