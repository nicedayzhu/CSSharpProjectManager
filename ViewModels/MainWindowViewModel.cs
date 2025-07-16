using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSSharpProjectManager.Views;

namespace CSSharpProjectManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private WorkspaceViewModel _workspace = new();

    [RelayCommand]
    private void CreateNewProject()
    {
        var newProjectWindow = new NewProjectWindow
        {
            DataContext = new NewProjectViewModel(Workspace.WorkspacePath)
        };
        newProjectWindow.Show();
    }
}