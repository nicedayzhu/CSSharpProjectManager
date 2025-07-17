using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace CSSharpProjectManager.ViewModels;

public partial class NewProjectViewModel : ViewModelBase
{
    public enum TemplateType
    {
        Default,
        Config,
        Lang,
        ConfigLang,
        DataMySql
    }

    [ObservableProperty]
    private string? _projectName;

    [ObservableProperty]
    private string? _projectPath;

    [ObservableProperty]
    private TemplateType _selectedTemplate = TemplateType.Default;

    [ObservableProperty]
    private bool _includeGitHubIntegration = true;
    
    [ObservableProperty]
    private string? _workspacePath; // 新增工作区路径属性

    partial void OnProjectNameChanged(string? value)
    {
        UpdateProjectPath();
    }

    partial void OnWorkspacePathChanged(string? value)
    {
        UpdateProjectPath();
    }

    private void UpdateProjectPath()
    {
        if (!string.IsNullOrWhiteSpace(WorkspacePath) && !string.IsNullOrWhiteSpace(ProjectName))
        {
            ProjectPath = Path.Combine(WorkspacePath, ProjectName);
        }
    }

    public NewProjectViewModel() // 移除参数
    {
    }

    public NewProjectViewModel(string? workspacePath) // 保留原有构造函数
    {
        WorkspacePath = workspacePath;
    }

    [RelayCommand]
    private async Task CreateProject()
    {
        if (string.IsNullOrWhiteSpace(ProjectName)) 
        {
            await ShowErrorMessage("项目名称不能为空");
            return;
        }

        if (string.IsNullOrWhiteSpace(WorkspacePath))
        {
            await ShowErrorMessage("请先设置工作区路径");
            return;
        }

        // 检查项目文件夹是否已存在
        var projectFullPath = Path.Combine(WorkspacePath, ProjectName);
        if (Directory.Exists(projectFullPath))
        {
            await ShowErrorMessage($"文件夹 {projectFullPath} 已存在，请使用其他项目名称");
            return;
        }
        try
        {
            // 构建完整命令
            var command = new StringBuilder();
            command.Append($"dotnet new cssharp -n {ProjectName} --t {SelectedTemplate.ToString().ToLower()}");
            
            if (IncludeGitHubIntegration)
            {
                command.Append(" --g");
            }

            // 准备进程启动信息
            var processInfo = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash",
                Arguments = OperatingSystem.IsWindows() 
                    ? $"/c {command}" 
                    : $"-c \"{command.ToString().Replace("\"", "\\\"")}\"",
                WorkingDirectory = WorkspacePath, // 在工作区目录下执行
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            // 启动进程
            using var process = new Process { StartInfo = processInfo };
            
            var output = new StringBuilder();
            process.OutputDataReceived += (_, e) => output.AppendLine(e.Data);
            process.ErrorDataReceived += (_, e) => output.AppendLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                await ShowSuccessMessage($"项目 {ProjectName} 创建成功！");
                // 可以在这里添加打开项目的逻辑
                // ✅ 打开项目所在文件夹
                if (Directory.Exists(projectFullPath))
                {
                    OpenFolderInExplorer(projectFullPath);
                }
            }
            else
            {
                await ShowErrorMessage($"项目创建失败:\n{output}");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorMessage($"发生错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 在文件资源管理器中打开文件夹
    /// </summary>
    /// <param name="folderPath">要打开的文件夹路径</param>
    private void OpenFolderInExplorer(string folderPath)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                Process.Start("explorer.exe", folderPath);
            }
            else if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", folderPath);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", folderPath);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"无法打开文件夹: {ex.Message}");
        }
    }
    private async Task ShowErrorMessage(string message)
    {
        if (App.MainWindow != null)
        {
            // ✅ 新版本 API
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                title: "提示",
                text: "错误" + message,
                @enum: ButtonEnum.Ok,
                icon: Icon.Error
            );
            var result = await messageBox.ShowAsync();// 异步显示弹窗
        }
    }

    private async Task ShowSuccessMessage(string message)
    {
        if (App.MainWindow != null)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                title: "提示",
                text: "成功" + message,
                @enum: ButtonEnum.Ok,
                icon: Icon.Success
            );
            var result = await messageBox.ShowAsync();// 异步显示弹窗
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        // 关闭窗口逻辑
    }
}