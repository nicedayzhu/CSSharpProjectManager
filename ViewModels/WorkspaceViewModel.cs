using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CSSharpProjectManager.ViewModels;

public partial class WorkspaceViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _workspacePath;

    [RelayCommand]
    private async Task SelectWorkspacePath()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "选择工作区目录",
            Directory = WorkspacePath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        var result = await dialog.ShowAsync(App.MainWindow);
        if (!string.IsNullOrWhiteSpace(result))
        {
            WorkspacePath = result;
            // 这里可以添加保存工作区路径的逻辑
        }
    }
}