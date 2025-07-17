using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSSharpProjectManager.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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
        FinishCreateProjectCommand = new RelayCommand(async () => await FinishCreateProject(), CanFinish);
        CurrentStep = 0;
    }

    private void ShowCreateProject()
    {
        if (string.IsNullOrWhiteSpace(Workspace.WorkspacePath))
        {
            ShowMessage("请先设置工作区路径，再创建新项目！");
            return;
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
    private bool CanFinish() => CurrentStep == 1 && NewProjectVM != null;
    private async Task FinishCreateProject()
    {
        if (NewProjectVM != null)
        {
            await NewProjectVM.CreateProjectCommand.ExecuteAsync(null);
            IsCreatingProject = false;
            NewProjectVM = null;
            CurrentStep = 0;
        }
    }
    private async void ShowMessage(string message)
    {
        var messageBox = MessageBoxManager.GetMessageBoxStandard(
            title: "提示",
            text: message,
            @enum: ButtonEnum.Ok,
            icon: Icon.Warning
        );
        await messageBox.ShowAsync();
    }

    partial void OnCurrentStepChanged(int value)
    {
        NextStepCommand.NotifyCanExecuteChanged();
        PreviousStepCommand.NotifyCanExecuteChanged();
        FinishCreateProjectCommand.NotifyCanExecuteChanged();
    }
}