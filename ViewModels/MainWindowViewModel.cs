using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSSharpProjectManager.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Diagnostics;

namespace CSSharpProjectManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private WorkspaceViewModel _workspace = new();

    [ObservableProperty]
    private bool _isCreatingProject;

    [ObservableProperty]
    private NewProjectViewModel? _newProjectVM;

    [ObservableProperty]
    private int _currentStep;

    [ObservableProperty]
    private bool _isCreating;

    public IRelayCommand ShowCreateProjectCommand { get; }
    public IRelayCommand NextStepCommand { get; }
    public IRelayCommand PreviousStepCommand { get; }
    public IRelayCommand CancelCreateProjectCommand { get; }
    public IRelayCommand FinishCreateProjectCommand { get; }

    public MainWindowViewModel()
    {
        ShowCreateProjectCommand = new RelayCommand(ShowCreateProject);
        NextStepCommand = new RelayCommand(NextStep, CanNextStep);
        PreviousStepCommand = new RelayCommand(PreviousStep, () => CurrentStep > 0);
        CancelCreateProjectCommand = new RelayCommand(CancelCreateProject);
        FinishCreateProjectCommand = new RelayCommand(async void () =>
        {
            try
            {
                await FinishCreateProject();
            }
            catch (Exception e)
            {
                throw; // TODO 处理异常
            }
        }, CanFinish);
        CurrentStep = 0;
        IsCreatingProject = false; // 确保默认是false
    }

    private async void ShowCreateProject()
    {
        if (string.IsNullOrWhiteSpace(Workspace.WorkspacePath))
        {
            ShowMessage("请先设置工作区路径，再创建新项目！");
            return;
        }
        if (!CheckDotnetSdkInstalled())
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "缺少 .NET SDK",
                "未检测到 .NET SDK 环境，请前往官网下载并安装！\n\nhttps://dotnet.microsoft.com/download",
                ButtonEnum.Ok,
                Icon.Error
            ).ShowAsync();
            return;
        }
        if (!IsCounterStrikeSharpTemplatesInstalled())
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(
                "缺少 CounterStrikeSharp 模板",
                "未检测到 CounterStrikeSharpTemplates 模板，是否自动为您安装？\n\n（如拒绝，请手动运行：dotnet new install CounterStrikeSharpTemplates）",
                ButtonEnum.YesNo,
                Icon.Question
            ).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                // 弹出安装进度窗口
                bool installSuccess = false;
                var progressWindow = new InstallProgressWindow();
                var vm = new InstallProgressViewModel(() => progressWindow.Close());
                progressWindow.DataContext = vm;
                var installTask = vm.InstallTemplateAsync();
                progressWindow.ShowDialog(App.MainWindow);
                installSuccess = await installTask;

                if (installSuccess && IsCounterStrikeSharpTemplatesInstalled())
                {
                    await MessageBoxManager.GetMessageBoxStandard(
                        "安装成功",
                        "CounterStrikeSharpTemplates 模板已成功安装！",
                        ButtonEnum.Ok,
                        Icon.Success
                    ).ShowAsync();
                }
                else
                {
                    await MessageBoxManager.GetMessageBoxStandard(
                        "安装失败",
                        "自动安装失败，请手动运行：dotnet new install CounterStrikeSharpTemplates",
                        ButtonEnum.Ok,
                        Icon.Error
                    ).ShowAsync();
                    return;
                }
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    "缺少 CounterStrikeSharp 模板",
                    "请先运行如下命令安装模板：\n\ndotnet new install CounterStrikeSharpTemplates",
                    ButtonEnum.Ok,
                    Icon.Error
                ).ShowAsync();
                return;
            }
        }
        NewProjectVM = new NewProjectViewModel { WorkspacePath = Workspace.WorkspacePath };
        // 监听ProjectName变化，刷新下一步按钮可用状态
        NewProjectVM.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NewProjectVM.ProjectName))
            {
                NextStepCommand.NotifyCanExecuteChanged();
            }
        };
        IsCreatingProject = true;
        CurrentStep = 0;
    }

    private void NextStep()
    {
        if (CurrentStep < 1) CurrentStep++;
    }
    private bool CanNextStep() => CurrentStep == 0 && NewProjectVM != null && !string.IsNullOrWhiteSpace(NewProjectVM.ProjectName);
    private void PreviousStep()
    {
        if (CurrentStep > 0) CurrentStep--;
    }
    private void CancelCreateProject()
    {
        IsCreatingProject = false;
        NewProjectVM = null;
        CurrentStep = 0;
    }
    private bool CanFinish() => CurrentStep == 1 && NewProjectVM != null && !IsCreating;
    private async Task FinishCreateProject()
    {
        if (NewProjectVM != null)
        {
            IsCreating = true;
            FinishCreateProjectCommand.NotifyCanExecuteChanged();
            await NewProjectVM.CreateProjectCommand.ExecuteAsync(null);
            IsCreating = false;
            FinishCreateProjectCommand.NotifyCanExecuteChanged();
            IsCreatingProject = false;
            NewProjectVM = null;
            CurrentStep = 0;
        }
    }
    private static async void ShowMessage(string message)
    {
        try
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                title: "提示",
                text: message,
                @enum: ButtonEnum.Ok,
                icon: Icon.Warning
            );
            await messageBox.ShowAsync();
        }
        catch (Exception e)
        {
            throw; // TODO 处理异常
        }
    }

    private static bool CheckDotnetSdkInstalled()
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-sdks",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            // 只要有输出，说明有SDK
            return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsCounterStrikeSharpTemplatesInstalled()
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "new list CounterStrikeSharpTemplates",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            // 检查输出中是否包含模板包名
            return output.Contains("CounterStrikeSharpTemplates", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static bool InstallCounterStrikeSharpTemplates()
    {
        try
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "new install CounterStrikeSharpTemplates",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    partial void OnCurrentStepChanged(int value)
    {
        NextStepCommand.NotifyCanExecuteChanged();
        PreviousStepCommand.NotifyCanExecuteChanged();
        FinishCreateProjectCommand.NotifyCanExecuteChanged();
    }
}