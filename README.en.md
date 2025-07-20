# CSSharpProjectManager

A cross-platform CounterStrikeSharp plugin project manager. Easily create plugin projects with multi-step forms, template selection, automatic dependency detection/installation, and a modern UI. Supports both Windows and Linux desktop environments.

## Features

- One-click creation of CounterStrikeSharp plugin projects
- Multiple template types (basic, config, language, database, etc.)
- Automatic detection and guided installation of .NET SDK and CounterStrikeSharpTemplates
- Supports automatic template installation with real-time progress display
- Workspace path setup and lock to prevent misoperation
- Modern multi-step form UI with next/previous step navigation
- Floating "About" button at the bottom right for author and project info
- Cross-platform: Windows & Linux (e.g., Ubuntu 22.04 Desktop)

## Screenshot

> ![Main UI Screenshot](./Assets/avalonia-logo.ico)

## Installation & Run

### Requirements
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [CounterStrikeSharpTemplates](https://www.nuget.org/packages/CounterStrikeSharpTemplates) dotnet template
- Avalonia UI (bundled)

### Get the Project

```bash
git clone https://github.com/nicedayzhu/CSSharpProjectManager.git
cd CSSharpProjectManager
dotnet restore
```

### Run

```bash
dotnet run -c Release
```

### Publish as Self-contained Executable (Windows Example)

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## Usage

1. Start the app and set the workspace path (cannot be changed after first set).
2. Click "Create New Project" and fill in the multi-step form for project info and template selection.
3. The app will auto-detect .NET SDK and CounterStrikeSharpTemplates; if missing, it will guide you to install.
4. Supports automatic template installation with detailed progress.
5. After project creation, the project folder will open automatically.
6. Click the bottom-right "About" button for author, version, and website info.

## Cross-platform Notes

- **Windows:** Recommended to run or publish as exe directly.
- **Linux (e.g., Ubuntu 22.04):** All process operations are asynchronous, UI will not freeze, and dependency detection/installation is supported.
- Ensure `dotnet` is in your PATH.

## FAQ

- **Q: The app freezes when creating a project?**
  - **A:** Fixed! All process operations are now asynchronous, and Linux uses `/bin/bash -c` for compatibility.
- **Q: What if the template is not installed?**
  - **A:** The app will auto-detect and guide installation, or you can run:
    ```bash
    dotnet new install CounterStrikeSharpTemplates
    ```
- **Q: Why can't I change the workspace path?**
  - **A:** To prevent misoperation, the path is locked after first set. Restart the app to change.

## Author & Community

- Author: 我不当学长 (Wobudang Xuezhang)
- Website: [csgo资料库](https://bbs.csgocn.net/)
- GitHub: [CSSharpProjectManager](https://github.com/nicedayzhu/CSSharpProjectManager)

---

> This project is not affiliated with Valve. For learning and communication purposes only. 