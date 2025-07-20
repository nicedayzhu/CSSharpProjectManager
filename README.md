# CSSharpProjectManager

一个跨平台的 CounterStrikeSharp 插件项目管理器，支持一键新建插件项目、多模板选择、自动检测/安装依赖、友好的多步表单界面，适用于 Windows 和 Linux 桌面环境。

## 功能特性

- 一键新建 CounterStrikeSharp 插件项目
- 多模板类型选择（基础、配置、多语言、数据库等）
- 自动检测并引导安装 .NET SDK 和 CounterStrikeSharpTemplates 模板
- 支持自动安装模板并显示安装进度
- 工作区路径设置与锁定，防止误操作
- 现代化多步表单界面，支持上一步/下一步
- 右下角 About 浮雕按钮，展示作者和项目信息
- 跨平台支持：Windows、Linux（如 Ubuntu 22.04 桌面）

## 截图

> ![主界面截图](./Assets/avalonia-logo.ico)

## 安装与运行

### 依赖环境
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [CounterStrikeSharpTemplates](https://www.nuget.org/packages/CounterStrikeSharpTemplates) dotnet模板
- Avalonia UI (已内置)

### 获取项目

```bash
git clone https://github.com/nicedayzhu/CSSharpProjectManager.git
cd CSSharpProjectManager
dotnet restore
```

### 运行

```bash
dotnet run -c Release
```

### 发布为自包含可执行文件（Windows示例）

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## 使用说明

1. 启动程序，设置工作区路径（首次设置后不可更改）。
2. 点击“创建新项目”，按多步表单填写项目信息、选择模板。
3. 程序会自动检测 .NET SDK 和 CounterStrikeSharpTemplates 模板，缺失时会引导安装。
4. 支持自动安装模板并显示详细进度。
5. 项目创建成功后会自动打开项目文件夹。
6. 右下角“关于”按钮可查看作者、版本、官网等信息。

## 跨平台说明

- Windows：推荐直接运行或发布为exe。
- Linux（如Ubuntu 22.04）：所有进程操作均为异步，UI不卡死，支持自动检测/安装依赖。
- 需确保 `dotnet` 命令在PATH中。

## 常见问题

- **Q: 创建项目时卡死？**
  - A: 已修复，所有进程操作均为异步，Linux下用`/bin/bash -c`兼容。
- **Q: 模板未安装怎么办？**
  - A: 程序会自动检测并引导安装，或手动运行：
    ```bash
    dotnet new install CounterStrikeSharpTemplates
    ```
- **Q: 工作区路径无法更改？**
  - A: 为防止误操作，首次设置后不可更改，如需更换请重启应用。

## 作者与社区

- 作者：我不当学长
- 网站：[csgo资料库](https://bbs.csgocn.net/)
- GitHub：[CSSharpProjectManager](https://github.com/nicedayzhu/CSSharpProjectManager)

---

> 本项目与Valve无关，仅供学习交流。 