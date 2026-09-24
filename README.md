# Xemu Manager

A cross-platform game library manager and launcher for [xemu](https://xemu.app), the Original Xbox emulator.

xemu is a great emulator, but it has no game library: you pick an ISO through a file dialog every time, and first-time setup (MCPX boot ROM, BIOS, hard disk image, EEPROM) is manual. Xemu Manager aims to be for xemu what [Xenia Manager](https://github.com/xenia-manager/xenia-manager) is for Xenia, with first-class support for Linux and the Steam Deck.

> **Status:** early development. See [docs/PLAN.md](docs/PLAN.md) for the roadmap.

## Platforms

| Platform | Status |
|---|---|
| Windows 10/11 (x64, ARM64) | Planned (primary dev platform) |
| Linux (x64, ARM64) — AppImage / Flatpak | Planned |
| Steam Deck (SteamOS) — Desktop Mode and Gaming Mode | Planned |
| macOS (Intel, Apple Silicon) | Planned |

## Planned features

- **Game library:** scan folders for Xbox ISOs (XISO / Redump), read title name and ID from the game's XBE, show box art
- **One-click launch:** start any game in xemu without browsing for files
- **Guided setup:** point to your own MCPX / BIOS dumps, get a ready-to-use hard disk image, validate everything before first launch
- **Per-game settings:** override xemu settings per title without touching your global config
- **xemu install and updates:** download and update xemu from its official releases
- **Compatibility info:** show each game's status from the official xemu compatibility list
- **Steam integration:** add games as Non-Steam shortcuts, controller-friendly big-screen mode for the Steam Deck

## Tech stack

- C# / .NET 10
- [Avalonia UI](https://avaloniaui.net) for the cross-platform desktop UI
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/) for MVVM

## Project layout

```
src/
  XemuManager.Core/       UI-independent logic: config, ISO/XBE parsing, library, launching
  XemuManager.Desktop/    Avalonia app (Views + ViewModels)
tests/
  XemuManager.Core.Tests/ xUnit tests for Core
docs/
  PLAN.md                 Roadmap and milestones
```

## Building

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet build
dotnet run --project src/XemuManager.Desktop
dotnet test
```

## Legal

Xemu Manager does **not** include or download any copyrighted Microsoft files (MCPX boot ROM, BIOS, dashboard) or games. You must dump these from hardware you own.

Xemu Manager is an independent community project and is not affiliated with the xemu project.
