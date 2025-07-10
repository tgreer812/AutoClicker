# AutoClicker Design Document

## Overview
AutoClicker is a desktop application designed for automating mouse clicks, specifically optimized for sending noble trains in Tribal Wars. The application provides a GUI interface for recording click positions, setting delays between clicks, and executing click sequences with customizable keybindings.

## Core Features

### 1. Click Recording
- Record single or multiple mouse positions
- Visual feedback when recording positions
- Ability to edit/delete recorded positions
- Display list of recorded positions with coordinates

### 2. Click Sequence Execution
- Execute recorded click positions in sequence
- Configurable delay between clicks (milliseconds precision)
- Start/stop functionality with visual indicators
- Option to loop sequences or run once

### 3. Keybinding System
- Customizable hotkeys for all major actions:
  - Record position (default: '[')
  - Start sequence (default: ']')
  - Stop sequence (default: '\')
  - Clear all positions (default: 'Delete')
- Global hotkeys that work when application is minimized

### 4. Timing System
- Immediate execution mode
- Scheduled start time with countdown
- Server time offset configuration
  - Toggle between local time and server time
  - Configurable offset (e.g., UTC+1)
  - Visual indicator showing current mode and time

## Technical Architecture

### Technology Stack
- **Language**: C# with .NET 6.0+
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Input Handling**: Windows API hooks for global hotkeys
- **Click Simulation**: Windows SendInput API

### Application Structure
```
AutoClicker/
├── src/
│   ├── AutoClicker.Core/          # Core business logic
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Interfaces/
│   ├── AutoClicker.UI/            # WPF application
│   │   ├── Views/
│   │   ├── ViewModels/
│   │   └── Controls/
│   └── AutoClicker.Tests/         # Unit tests
├── docs/
└── README.md
```

### Core Components

#### 1. ClickPosition Model
```csharp
public class ClickPosition
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Order { get; set; }
    public string Label { get; set; }
}
```

#### 2. ClickSequence Model
```csharp
public class ClickSequence
{
    public List<ClickPosition> Positions { get; set; }
    public int DelayMilliseconds { get; set; }
    public bool IsLooping { get; set; }
    public DateTime? ScheduledStartTime { get; set; }
}
```

#### 3. Services
- **ClickService**: Handles mouse click simulation
- **HotkeyService**: Manages global hotkey registration
- **TimerService**: Handles scheduling and time offset calculations
- **ConfigurationService**: Persists user settings

## User Interface Design

### Main Window Layout
```
┌─────────────────────────────────────────┐
│  AutoClicker - Tribal Wars Assistant    │
├─────────────────────────────────────────┤
│ ┌─────────────┐ ┌─────────────────────┐ │
│ │ Click List  │ │ Settings            │ │
│ │             │ │                     │ │
│ │ 1. (X, Y)   │ │ Delay: [____] ms    │ │
│ │ 2. (X, Y)   │ │                     │ │
│ │ 3. (X, Y)   │ │ □ Loop sequence     │ │
│ │             │ │                     │ │
│ │ [+] [-] [↑] │ │ Time Mode:          │ │
│ │     [↓]     │ │ ○ Local ● Server    │ │
│ └─────────────┘ │ Offset: [+1] hours  │ │
│                 │                     │ │
│                 │ Start Time:         │ │
│                 │ [__:__:__]         │ │
│                 └─────────────────────┘ │
│                                         │
│ ┌─────────────────────────────────────┐ │
│ │ Hotkeys                             │ │
│ │ Record: [[] Start: []] Stop: [\]    │ │
│ │ Clear: [Del]                        │ │
│ └─────────────────────────────────────┘ │
│                                         │
│ [Start Sequence] [Stop] [Clear All]     │
│                                         │
│ Status: Ready                           │
└─────────────────────────────────────────┘
```

### Key UI Elements
1. **Click List Panel**: Shows recorded positions with edit/delete options
2. **Settings Panel**: Configure delays, loop mode, and timing
3. **Hotkey Configuration**: Display and edit keybindings
4. **Control Buttons**: Start/stop sequence, clear positions
5. **Status Bar**: Shows current state and countdown

## Data Persistence

### Configuration Storage
- Store settings in `%APPDATA%/AutoClicker/config.json`
- Settings include:
  - Saved click sequences
  - Keybindings
  - Default delay
  - Time offset
  - Window position/size

### File Format
```json
{
  "sequences": [
    {
      "name": "Noble Train 1",
      "positions": [
        {"x": 100, "y": 200, "label": "First noble"},
        {"x": 150, "y": 250, "label": "Second noble"}
      ],
      "delayMs": 50
    }
  ],
  "keybindings": {
    "record": "[",
    "start": "]",
    "stop": "\\",
    "clear": "Delete"
  },
  "timeOffset": 1,
  "defaultDelayMs": 50
}
```

## Security Considerations

1. **Anti-detection**: 
   - Add slight randomization to click positions (±2 pixels)
   - Vary delays by ±10% to appear more human-like
   - Option to disable for precise timing needs

2. **Application Safety**:
   - Require confirmation for clearing all positions
   - Auto-save sequences before clearing
   - Escape key as universal stop

## Future Enhancements

1. **Advanced Features**:
   - Multiple saved sequences with quick switching
   - Conditional clicking based on pixel color
   - Import/export sequences
   - Click-and-drag support

2. **Quality of Life**:
   - Minimize to system tray
   - Sound notifications for sequence completion
   - Visual overlay showing click positions
   - Macro recording for complex sequences

## Development Phases

### Phase 1: Core Functionality (MVP)
- Basic GUI with click recording
- Simple click execution with fixed delay
- Basic keybinding support

### Phase 2: Enhanced Features
- Scheduled execution
- Server time support
- Persistence and settings

### Phase 3: Polish
- Advanced UI features
- Error handling and logging
- Performance optimization

## Testing Strategy

1. **Unit Tests**:
   - Click position calculations
   - Time offset calculations
   - Keybinding conflict detection

2. **Integration Tests**:
   - Click simulation accuracy
   - Hotkey registration/unregistration
   - Configuration persistence

3. **Manual Testing**:
   - Various screen resolutions
   - Multi-monitor setups
   - Extended usage scenarios
