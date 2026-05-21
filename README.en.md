![Dragablz](https://dragablz.files.wordpress.com/2015/01/dragablztext22.png)

# Dragablz — Tearable Tab Control for WPF

**Community-maintained fork of [ButchersBoy/Dragablz](https://github.com/ButchersBoy/Dragablz)**, targeting .NET 10 + WPF with ongoing features, bug fixes, and expanded Material Design theme support.

[![NuGet](https://img.shields.io/nuget/v/Dragablz.svg?style=flat-square)](http://www.nuget.org/packages/Dragablz/)

---

## Overview

Dragablz is a WPF tab control library with these core capabilities:

- **Tab tear-off** — drag a tab out of a window to create a new window, or drop into another tab group
- **Docking layout (Dockablz)** — split panels in any direction within a window, drag-and-drop reordering
- **Floating/MDI windows** — detach tabs into independent floating windows with free positioning, maximize, and restore
- **MVVM-friendly** — full data binding support, `ClosingItemCallback` / `AddingItemCommand` hooks
- **Chromeless window** — built-in `DragablzWindow` with transparency, resizing, and system commands

## Project Status

| | |
|---|---|
| Target framework | `net10.0-windows` |
| External dependencies | **Zero** (WPF framework only) |
| Project structure | Core library `Dragablz` + NUnit tests `Dragablz.Test` + Prism demo `Dragablz.Demo.Prism` |
| XAML namespaces | `dragablz` → `Dragablz`, `dockablz` → `Dragablz.Dockablz` |
| Source files | 91 C# files + 4 XAML themes |

## Quick Start

### 1. Installation

Reference the project directly, or via NuGet:

```
dotnet add package Dragablz
```

### 2. Merge theme resources (`App.xaml`)

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/Dragablz;component/Themes/Generic.xaml" />
            <ResourceDictionary Source="pack://application:,,,/Dragablz;component/Themes/MaterialDesign.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 3. Minimal XAML

```xml
<dragablz:TabablzControl Margin="8">
    <dragablz:TabablzControl.InterTabController>
        <dragablz:InterTabController />
    </dragablz:TabablzControl.InterTabController>
    <TabItem Header="Tab 1" IsSelected="True">
        <TextBlock Text="Hello World" />
    </TabItem>
    <TabItem Header="Tab 2">
        <TextBlock Text="Drag me!" />
    </TabItem>
</dragablz:TabablzControl>
```

### 4. Apply Material Design styles

```xml
<dragablz:TabablzControl
    Style="{StaticResource MaterialDesignElevatedTabablzControlStyle}"
    ItemsSource="{Binding Tabs}"
    ShowDefaultCloseButton="True">
    <dragablz:TabablzControl.HeaderItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Header}" />
        </DataTemplate>
    </dragablz:TabablzControl.HeaderItemTemplate>
</dragablz:TabablzControl>
```

## Built-in Themes

| Theme | File | Description |
|-------|------|-------------|
| Generic (default) | `Themes/Generic.xaml` | ~1900 lines, classic WPF tab look, trapezoid tabs, floating tool windows |
| **Material Design** | `Themes/MaterialDesign.xaml` | ~1100 lines, includes ripple effects, animated underline indicator, colored header strip, MD window |
| MahApps | `Themes/MahApps.xaml` | ~440 lines, MahApps.Metro style with trapezoid overlap tabs |

### Material Design Styles

| Key | Target | Description |
|-----|--------|-------------|
| `MaterialDesignTabablzControlStyle` | TabablzControl | Base MD style, DropShadowEffect on header |
| `MaterialDesignElevatedTabablzControlStyle` | TabablzControl | **New** — colored header strip, animated underline, stronger shadow |
| `MaterialDesignAlternateTabablzControlStyle` | TabablzControl | Light variant (MaterialDesignPaper background) |
| `MaterialDesignDragableTabItemStyle` | DragablzItem | Base MD tab item with Ripple effect |
| `MaterialDesignAnimatedDragableTabItemStyle` | DragablzItem | **New** — ScaleTransform animated underline indicator |
| `MaterialDesignDenseDragableTabItemStyle` | DragablzItem | **New** — compact (32px height, 12px font) |
| `MaterialDesignDragablzWindowStyle` | DragablzWindow | **New** — colored title bar MD window |
| `MaterialDesignDropZoneStyle` | DropZone | **New** — MD-styled drop zone with fade animation |

### New Utility Classes (`Dragablz.Themes` namespace)

| Class | Description |
|-------|-------------|
| `UnderlineIndicator` | Animated underline indicator control with ScaleTransform expand/collapse |
| `MaterialDesignHeaderedAssist` | Attached properties: `HeaderBackground`, `HeaderForeground`, `IsIndicatorAnimated`, `IndicatorThickness` |

## Core Concepts

### Tab Drag Pipeline

```
DragablzItem (Thumb drag)
  → DragablzItemsControl (IItemsOrganiser coordinates layout)
    → TabablzControl (MonitorBreach/MonitorReentry boundary detection)
      → InterTabController → InterTabClient → new window
```

### Dockablz Layout Tree

```
Layout (root ContentControl)
  ├── Content = TabablzControl          ← leaf: single tab group
  │   OR
  ├── Content = Branch (Control)        ← recursive binary split
  │   ├── FirstItem  → TabablzControl | Branch
  │   └── SecondItem → TabablzControl | Branch
  └── _floatingItems (MDI floating layer)
```

### ItemsOrganiser Strategy Pattern

- `StackOrganiser` (abstract) → `HorizontalOrganiser` / `VerticalOrganiser` — linear stack, 200ms animation
- `CanvasOrganiser` — free-form canvas, Z-order only

## MVVM Usage

```xml
<dragablz:TabablzControl
    ItemsSource="{Binding Documents}"
    SelectedItem="{Binding SelectedDocument}"
    ClosingItemCallback="{Binding ClosingItemCallback}"
    ClosingItemCommand="{Binding CloseDocumentCommand}"
    NewItemFactory="{Binding NewItemFactory}" />
```

Callback signature: `ItemActionCallback(ItemActionCallbackArgs<TabablzControl> args)` — call `args.Cancel()` to prevent close/consolidation.

## Project Documentation

- **CLAUDE.en.md** — Full project reference map: 91 classes, all DPs, method signatures, architecture diagrams, file index (English)
- **CLAUDE.md** — Same reference in Chinese
- **CHANGELOG.md** — Change log for this fork

---

This fork originated from [BornToBeRoot/NETworkManager](https://github.com/BornToBeRoot/NETworkManager), continuously migrating to the latest .NET and enhancing Material Design theme support.
