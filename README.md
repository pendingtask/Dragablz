![Dragablz](https://dragablz.files.wordpress.com/2015/01/dragablztext22.png)

# Dragablz — WPF 可拖拽 Tab 控件

**基于 [ButchersBoy/Dragablz](https://github.com/ButchersBoy/Dragablz) 的社区维护分支**，面向 .NET 10 + WPF，持续增加功能、修复缺陷并扩充 Material Design 主题。

[![NuGet](https://img.shields.io/nuget/v/Dragablz.svg?style=flat-square)](http://www.nuget.org/packages/Dragablz/)

---

## 概述

Dragablz 是一个 WPF Tab 控件库，核心能力：

- **Tab 拖拽撕裂** —— 将 Tab 拖出窗口自动创建新窗口，或拖入其他 Tab 组
- **停靠布局（Dockablz）** —— 在窗口内任意方向分割面板，支持拖放重组
- **浮动/MDI 窗口** —— Tab 可脱离为独立浮动窗口，自由定位、最大化、还原
- **MVVM 友好** —— 完整支持数据绑定，`ClosingItemCallback` / `AddingItemCommand` 等钩子
- **Chromeless 窗口** —— 自带 `DragablzWindow`，支持透明、调整大小、系统命令

## 项目状态

| | |
|---|---|
| 目标框架 | `net10.0-windows` |
| 外部依赖 | **零**（仅依赖 WPF 框架） |
| 项目结构 | 核心库 `Dragablz` + NUnit 测试 `Dragablz.Test` + Prism 示例 `Dragablz.Demo.Prism` |
| XAML 命名空间 | `dragablz` → `Dragablz`，`dockablz` → `Dragablz.Dockablz` |
| 源文件 | 89 个 C# 文件 + 4 个 XAML 主题 |

## 快速开始

### 1. 安装

直接引用项目，或通过 NuGet：

```
dotnet add package Dragablz
```

### 2. 合并主题资源（`App.xaml`）

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

### 3. 最少 XAML

```xml
<dragablz:TabablzControl Margin="8">
    <dragablz:TabablzControl.InterTabController>
        <dragablz:InterTabController />
    </dragablz:TabablzControl.InterTabController>
    <TabItem Header="标签 1" IsSelected="True">
        <TextBlock Text="Hello World" />
    </TabItem>
    <TabItem Header="标签 2">
        <TextBlock Text="拖拽我试试！" />
    </TabItem>
</dragablz:TabablzControl>
```

### 4. 应用 Material Design 样式

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

## 内置主题

| 主题 | 文件 | 说明 |
|------|------|------|
| Generic（默认） | `Themes/Generic.xaml` | ~1900 行，经典 WPF Tab 外观、梯形 Tab、浮动工具窗口 |
| **Material Design** | `Themes/MaterialDesign.xaml` | ~1100 行，含涟漪效果、动画下划线指示器、彩色头部条、MD 窗口 |
| MahApps | `Themes/MahApps.xaml` | ~440 行，MahApps.Metro 风格梯形重叠 Tab |

### Material Design 可用样式

| Key | 目标 | 说明 |
|-----|------|------|
| `MaterialDesignTabablzControlStyle` | TabablzControl | 基础 MD 风格，DropShadowEffect 头部阴影 |
| `MaterialDesignElevatedTabablzControlStyle` | TabablzControl | **新增** — 彩色头部条、动画下划线、更强阴影 |
| `MaterialDesignAlternateTabablzControlStyle` | TabablzControl | 浅色变体（MaterialDesignPaper 背景） |
| `MaterialDesignDragableTabItemStyle` | DragablzItem | 基础 MD Tab 项，含 Ripple 涟漪 |
| `MaterialDesignAnimatedDragableTabItemStyle` | DragablzItem | **新增** — ScaleTransform 动画下划线指示器 |
| `MaterialDesignDenseDragableTabItemStyle` | DragablzItem | **新增** — 紧凑型（32px 高，12px 字） |
| `MaterialDesignDragablzWindowStyle` | DragablzWindow | **新增** — 彩色标题栏 MD 窗口 |
| `MaterialDesignDropZoneStyle` | DropZone | **新增** — MD 风格放置区，含淡入动画 |

### 新增辅助类（`Dragablz.Themes` 命名空间）

| 类 | 说明 |
|----|------|
| `UnderlineIndicator` | 动画下划线指示器控件，激活时 ScaleTransform 水平展开 |
| `MaterialDesignHeaderedAssist` | 附加属性：`HeaderBackground`、`HeaderForeground`、`IsIndicatorAnimated`、`IndicatorThickness` |

## 核心概念

### Tab 拖拽流水线

```
DragablzItem (Thumb 拖拽)
  → DragablzItemsControl (IItemsOrganiser 协调排列)
    → TabablzControl (MonitorBreach/MonitorReentry 越界/重入检测)
      → InterTabController → InterTabClient → 新窗口
```

### Dockablz 布局树

```
Layout (根 ContentControl)
  ├── Content = TabablzControl          ← 叶：单 Tab 组
  │   或
  ├── Content = Branch (Control)        ← 递归二分分割
  │   ├── FirstItem  → TabablzControl | Branch
  │   └── SecondItem → TabablzControl | Branch
  └── _floatingItems (DragablzItemsControl) ← MDI 浮动层
```

### 布局器策略模式

- `StackOrganiser`（抽象）→ `HorizontalOrganiser` / `VerticalOrganiser` — 线性堆叠，带 200ms 动画
- `CanvasOrganiser` — 自由画布，仅管理 Z 轴顺序

## MVVM 用法

```xml
<dragablz:TabablzControl
    ItemsSource="{Binding Documents}"
    SelectedItem="{Binding SelectedDocument}"
    ClosingItemCallback="{Binding ClosingItemCallback}"
    ClosingItemCommand="{Binding CloseDocumentCommand}"
    NewItemFactory="{Binding NewItemFactory}" />
```

回调签名：`ItemActionCallback(ItemActionCallbackArgs<TabablzControl> args)`，调用 `args.Cancel()` 可阻止关闭/合并。

## 项目文档

- **CLAUDE.md** — 完整项目地图：89 个类逐类参考、所有 DependencyProperty、方法签名、架构图、文件索引（中文）
- **CHANGELOG.md** — 本分支的修改记录

---

本分支由 [BornToBeRoot/NETworkManager](https://github.com/BornToBeRoot/NETworkManager) 派生，持续迁移至最新 .NET 并增强 Material Design 主题支持。
