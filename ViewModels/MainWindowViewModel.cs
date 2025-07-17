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

    [RelayCommand]
    // private void CreateNewProject()
    // {
    //     var newProjectWindow = new NewProjectWindow
    //     {
    //         DataContext = new NewProjectViewModel(Workspace.WorkspacePath)
    //     };
    //     newProjectWindow.Show();
    // }
    private async Task CreateNewProject()
    {
        if (string.IsNullOrWhiteSpace(Workspace.WorkspacePath))
        {
            // ✅ 新版本 API
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                title: "提示",
                text: "请先设置工作区路径，再创建新项目！",
                @enum: ButtonEnum.Ok,
                icon: Icon.Warning
            );
            var result = await messageBox.ShowAsync();// 异步显示弹窗
            return;
        }

        var newProjectWindow = new NewProjectWindow
        {
            DataContext = new NewProjectViewModel
            {
                WorkspacePath = Workspace.WorkspacePath
            }
        };
        newProjectWindow.Show();
    }
}