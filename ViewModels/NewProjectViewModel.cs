using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CSSharpProjectManager.ViewModels;

public partial class NewProjectViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _projectName;

    [ObservableProperty]
    private string? _projectPath;

    public NewProjectViewModel(string? workspacePath)
    {
        ProjectPath = workspacePath;
    }

    [RelayCommand]
    private async Task CreateProject()
    {
        if (string.IsNullOrWhiteSpace(ProjectName))
            return;
        
        var fullPath = Path.Combine(ProjectPath ?? "", ProjectName);
        Directory.CreateDirectory(fullPath);
        // 这里可以添加更多项目创建逻辑
        
        // 关闭窗口或导航回主视图
    }
}