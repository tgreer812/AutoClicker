# AutoClicker

A desktop application for automating mouse clicks, specifically optimized for sending noble trains in Tribal Wars.

## Project Structure

```
AutoClicker/
├── src/
│   ├── AutoClicker.Core/          # Core business logic
│   │   ├── Models/                # Data models
│   │   ├── Services/              # Business logic services
│   │   └── Interfaces/            # Service interfaces
│   ├── AutoClicker.UI/            # WPF application
│   │   ├── Views/                 # XAML views
│   │   ├── ViewModels/            # View models
│   │   └── Controls/              # Custom controls
│   └── AutoClicker.Tests/         # Unit tests
├── docs/                          # Documentation
├── designdoc.md                   # Design document
└── README.md                      # This file
```

## Development Status

🚧 **Project Structure Created** - The basic project structure has been set up with placeholder files.

### What's Done:
- ✅ Solution and project structure created
- ✅ Core models defined (ClickPosition, ClickSequence, AppConfiguration)
- ✅ Service interfaces defined
- ✅ Placeholder service implementations
- ✅ Basic ViewModel structure
- ✅ Project references configured

### Next Steps:
1. Implement Windows API integration for click simulation
2. Implement global hotkey registration
3. Create the main WPF window and UI
4. Implement configuration persistence
5. Add timer and scheduling functionality

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run --project src/AutoClicker.UI
```

## Features (Planned)

- 🎯 Click position recording
- ⏱️ Configurable delays between clicks
- 🔄 Sequence looping
- ⌨️ Customizable hotkeys
- 📅 Scheduled execution
- 🌍 Server time support
- 💾 Settings persistence

See `designdoc.md` for detailed specifications.