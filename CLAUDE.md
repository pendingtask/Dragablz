# Dragablz — 项目地图/综述

> Claude Code 导航用项目全览。覆盖 89 个 C# 源文件、4 个 XAML 主题、6 个命名空间、0 个 NuGet 依赖。

---

## 1. 项目概览

| 项目 | 详情 |
|------|------|
| **解决方案** | `Dragablz.sln`（VS2022） |
| **项目组成** | Dragablz（核心库）、Dragablz.Test（NUnit 单元测试）、Dragablz.Demo.Prism（WPF 演示程序） |
| **目标框架** | `net10.0-windows`，UseWPF=true |
| **NuGet 依赖** | 无（零外部依赖） |
| **程序集版本** | 0.0.4.0 |
| **XAML 命名空间** | `dragablz` → `Dragablz`，`dockablz` → `Dragablz.Dockablz` |
| **InternalsVisibleTo** | `Dragablz.Test`（允许测试项目访问 internal 类型） |

### 源文件目录树（仅核心库）

```
Dragablz/
├── *.cs                          (40 个文件 — 根命名空间 Dragablz)
├── Core/
│   ├── CollectionTeaser.cs       集合包装器（反射）
│   ├── Extensions.cs             树遍历和窗口几何扩展
│   ├── FuncComparer.cs           函数式比较器
│   ├── HitTest.cs                Win32 HT* 命中测试常量
│   ├── InstanceRegistry.cs       弱引用实例注册表
│   ├── InterTabTransfer.cs       跨 Tab 拖放传输状态
│   ├── MultiComparer.cs          链式比较器
│   ├── Native.cs                 Win32/DWM P/Invoke 封装
│   ├── SystemCommand.cs          Win32 SC_* 系统命令常量
│   ├── TabHeaderDragStartInformation.cs  拖拽起始快照
│   └── WindowMessage.cs          Win32 WM_* + WMSZ_* 窗口消息常量
├── Converters/
│   ├── BooleanAndToVisibilityConverter.cs  多值布尔 AND → 可见性
│   ├── EqualityToBooleanConverter.cs       相等性 → 布尔
│   ├── EqualityToVisibilityConverter.cs    相等性 → 可见性
│   └── ShowDefaultCloseButtonConverter.cs  关闭按钮可见性逻辑
├── Dockablz/
│   ├── Branch.cs, BranchAccessor.cs, BranchItem.cs, BranchResult.cs
│   ├── CouldBeHeaderedStyleSelector.cs
│   ├── DropZone.cs, DropZoneLocation.cs
│   ├── Extensions.cs             Query/Visit 扩展
│   ├── Finder.cs                 布局中查找 TabablzControl
│   ├── FloatRequestedEvent.cs, FloatTransfer.cs, FloatingItemSnapShot.cs
│   ├── Layout.cs, LayoutAccessor.cs
│   ├── LocationReport.cs, LocationReportBuilder.cs, LocationReportException.cs, LocationSnapShot.cs
│   └── Tiler.cs, TilerCalculator.cs
├── Referenceless/
│   ├── AnonymousDisposable.cs    匿名一次性操作
│   ├── DefaultDisposable.cs      单例空操作 Disposable
│   ├── Disposable.cs             工厂方法
│   ├── ICancelable.cs            可取消接口
│   └── SerialDisposable.cs       可替换的 Disposable 持有者
├── Themes/
│   ├── BrushToRadialGradientBrushConverter.cs  径向渐变转换器
│   ├── MaterialDesignAssist.cs    Material Design 辅助附加属性
│   ├── Ripple.cs                 Material Design 涟漪效果控件
│   ├── RippleAssist.cs           涟漪配置附加属性
│   ├── SystemCommandIcon.cs      窗口系统命令图标控件
│   ├── Generic.xaml              (~1909 行 — 默认主题)
│   ├── MaterialDesign.xaml       (~802 行 — Material Design 主题)
│   ├── MahApps.xaml              (~442 行 — MahApps.Metro 主题)
│   └── Dockablz.xaml             (空白占位文件)
└── Properties/
    ├── AssemblyInfo.cs
    ├── Resources.Designer.cs
    └── Settings.Designer.cs
```

---

## 2. 命名空间快速导航

| 命名空间 | 可见性 | 用途 | 关键类型 |
|----------|--------|------|----------|
| `Dragablz` | 公开 API | 核心 Tab/拖拽控件、接口、枚举 | `TabablzControl`、`DragablzItem`、`DragablzWindow` |
| `Dragablz.Dockablz` | 公开 API | 停靠布局引擎 | `Layout`、`Branch`、`DropZone`、`LayoutAccessor` |
| `Dragablz.Core` | Internal | Win32 P/Invoke、树遍历、工具类 | `Native`、`Extensions`、`InstanceRegistry<T>` |
| `Dragablz.Converters` | 公开 | WPF 值/多值转换器 | `BooleanAndToVisibilityConverter` |
| `Dragablz.Themes` | 公开 | 主题支持（Material 涟漪、图标） | `Ripple`、`SystemCommandIcon` |
| `Dragablz.Referenceless` | Internal | Rx 风格 Disposable 原语 | `SerialDisposable`、`AnonymousDisposable` |

---

## 3. 逐类参考

### 3.1 命名空间 `Dragablz`——核心控件

#### `TabablzControl` : `TabControl`
**文件：** `Dragablz/TabablzControl.cs`

核心 Tab 控件。管理 Tab 头部布局、拖拽撕裂（tear-off）、跨 Tab 传输、Tab 关闭/添加、键盘导航。

**模板部件：** `PART_HeaderItemsControl`（DragablzItemsControl）、`PART_ItemsHolder`（Panel）

**静态命令：**
- `CloseItemCommand` — 关闭 Tab 的路由命令
- `AddItemCommand` — 添加新 Tab 的路由命令

**静态方法：**
- `GetLoadedInstances()` → `IEnumerable<TabablzControl>` — 获取所有存活实例
- `CloseItem(object tabContentItem)` — 关闭包含指定内容的所有 Tab
- `AddItem(object item, object nearItem, AddLocationHint)` — 在指定项附近添加
- `SelectItem(object item)` — 跨所有实例查找并选中
- `GetIsClosingAsPartOfDragOperation(Window)` → bool — 附加属性 getter
- `GetIsWrappingTabItem(DependencyObject)` → bool — 附加属性 getter

**依赖属性（共 28 个）：**

| DP 名称 | 类型 | 默认值 | 用途 |
|---------|------|--------|------|
| `AdjacentHeaderItemOffset` | double | 0 | 相邻头部项间距 |
| `HeaderItemsOrganiser` | IItemsOrganiser | new HorizontalOrganiser() | 头部项布局策略 |
| `HeaderMemberPath` | string | null | 数据项上 Header 成员的路径 |
| `HeaderItemTemplate` | DataTemplate | null | 头部项模板 |
| `HeaderPrefixContent` | object | null | 头部 Tab 之前的内容 |
| `HeaderPrefixContentStringFormat` | string | null | 前缀内容格式化字符串 |
| `HeaderPrefixContentTemplate` | DataTemplate | null | 前缀内容模板 |
| `HeaderPrefixContentTemplateSelector` | DataTemplateSelector | null | 前缀内容模板选择器 |
| `HeaderSuffixContent` | object | null | 头部 Tab 之后的内容 |
| `HeaderSuffixContentStringFormat` | string | null | 后缀内容格式化字符串 |
| `HeaderSuffixContentTemplate` | DataTemplate | null | 后缀内容模板 |
| `HeaderSuffixContentTemplateSelector` | DataTemplateSelector | null | 后缀内容模板选择器 |
| `ShowDefaultCloseButton` | bool | false | 是否在 Tab 上显示关闭按钮 |
| `ShowDefaultAddButton` | bool | false | 是否显示添加按钮 |
| `IsHeaderPanelVisible` | bool | true | 头部面板可见性 |
| `AddLocationHint` | AddLocationHint | Last | 新 Tab 出现的位置 |
| `FixedHeaderCount` | int | 0 | 前 N 个 Tab 为固定（不可拖拽、无关闭按钮） |
| `DisableBranchConsolidation` | bool | false | 禁止拖出时的分支合并 |
| `InterTabController` | InterTabController | null | 跨 Tab 拖拽必需；提供配置和回调 |
| `NewItemFactory` | Func\<object\> | null | 新项工厂（配合 AddItemCommand 使用） |
| `IsEmpty`（只读） | bool | true | 是否无 Tab |
| `ClosingItemCallback` | ItemActionCallback | null | 关闭前的回调；调用 args.Cancel() 可取消 |
| `ClosingItemCommand` | ICommand | null | 关闭项时执行的命令 |
| `AddingItemCommand` | ICommand | null | 添加项时执行的命令 |
| `ConsolidateOrphanedItems` | bool | false | 窗口关闭时自动将剩余 Tab 迁移到其他宿主 |
| `ConsolidatingOrphanedItemCallback` | ItemActionCallback | null | 可取消单个孤立项的合并 |
| `IsDraggingWindow`（只读） | bool | false | 窗口正被 Tab 拖拽中 |
| `EmptyHeaderSizingHint` | EmptyHeaderSizingHint | Collapse | 无 Tab 时头部的尺寸策略 |

**路由事件：** `IsEmptyChanged`（冒泡）、`IsDraggingWindowChanged`（冒泡）

**关键方法：**
- `AddToSource(object item)` — 添加项；优先委托给 IManualInterTabClient
- `RemoveFromSource(object item)` — 从源集合移除
- `GetOrderedHeaders()` → `IEnumerable<DragablzItem>` — 按可视位置排序的项
- `OnApplyTemplate()` — 获取模板部件，绑定 DragablzItemsControl 事件
- `RemoveItem(DragablzItem)` → object（internal）— 移除 Tab，处理合并/关闭
- `ReceiveDrag(InterTabTransfer)`（internal）— 从另一窗口接收拖来的 Tab

**核心 private 方法：**
- `ItemDragStarted(...)` — 记录起始位置，管理选择状态
- `MonitorBreach(...)` — 检测拖拽超出头部边界 → 创建新窗口
- `MonitorReentry(...)` — 检测拖拽进入另一个 TabablzControl
- `IsTransposing(TabControl)` → bool — 检查方向是否相反
- `CloseItem(DragablzItem, TabablzControl)`（静态）— 执行 ClosingItemCommand + ClosingItemCallback
- `UpdateSelectedItem()` — 创建/显示选中项的 ContentPresenter

---

#### `DragablzItem` : `ContentControl`
**文件：** `Dragablz/DragablzItem.cs`

单个 Tab 项。通过 Thumb（`PART_Thumb`）管理拖拽、调整大小手柄、鼠标事件和路由拖拽事件。

**模板部件：** `PART_Thumb`（Thumb）

**嵌套枚举：** `SizeGrip { NotApplicable, Left, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft }`——标识窗口调整大小的手柄位置

**依赖属性：**

| DP 名称 | 类型 | 默认值 | 用途 |
|---------|------|--------|------|
| `X` | double | 0 | X 坐标（拖拽/定位用） |
| `Y` | double | 0 | Y 坐标（拖拽/定位用） |
| `LogicalIndex`（只读） | int | 0 | 排序后的逻辑索引 |
| `SizeGrip`（附加属性） | SizeGrip | NotApplicable | 应用于 Thumb 以标识调整大小操作 |
| `ContentRotateTransformAngle`（附加，继承） | double | 0 | 旋转内容（用于垂直 Tab） |
| `IsSelected` | bool | false | 选中状态（默认双向绑定） |
| `IsDragging`（只读） | bool | false | 此项目正在被拖拽 |
| `IsSiblingDragging`（只读） | bool | false | 兄弟项目正在被拖拽 |
| `IsCustomThumb`（附加属性） | bool | false | 标记 Thumb 为自定义拖拽手柄 |

**路由事件：** `XChanged`、`YChanged`、`LogicalIndexChanged`、`IsDraggingChanged`、`IsSiblingDraggingChanged`、`MouseDownWithin`、`DragStarted`、`DragDelta`、`PreviewDragDelta`（隧道）、`DragCompleted`

**关键方法：**
- `OnApplyTemplate()` — 选择并订阅拖拽 Thumb，处理拖拽续捕获
- `InstigateDrag(Action<DragablzItem> continuation)` — 以编程方式启动拖拽
- `OnPreviewMouseRightButtonDown/Up(...)` — 右键操作期间临时禁用 Thumb 命中测试

**Internal 属性：** `UnderlyingContent`、`MouseAtDragStart`、`PartitionAtDragStart`、`IsDropTargetFound`

**核心 private 方法：**
- `SelectAndSubscribeToThumb()` → `Tuple<Thumb, IDisposable>` — 选择模板或自定义 Thumb，订阅拖拽事件
- `ThumbOnDragStarted/Delta/Completed(...)` — 核心拖拽状态机；引发路由事件
- `SizeThumbOnDragDelta(...)`（静态）— 通过大小手柄调整大小，更新 X/Y/Width/Height
- `FindCustomThumb()` → Thumb — 在可视树中搜索带 IsCustomThumb 的 Thumb

---

#### `HeaderedDragablzItem` : `DragablzItem`
**文件：** `Dragablz/HeaderedDragablzItem.cs`

扩展 `DragablzItem`，增加头部内容属性。

**依赖属性：** `HeaderContent`（object）、`HeaderContentStringFormat`（string）、`HeaderContentTemplate`（DataTemplate）、`HeaderContentTemplateSelector`（DataTemplateSelector）

---

#### `DragablzItemsControl` : `ItemsControl`
**文件：** `Dragablz/DragablzItemsControl.cs`

头部项的宿主控件。使用 `IItemsOrganiser` 布局项目，管理兄弟项之间的拖拽协调，并通过 `PositionMonitor` 报告位置变化。

**依赖属性：**

| DP 名称 | 类型 | 默认值 | 用途 |
|---------|------|--------|------|
| `FixedItemCount` | int | 0 | 不可移动的项目数 |
| `ItemsOrganiser` | IItemsOrganiser | null | 布局策略 |
| `PositionMonitor` | PositionMonitor | null | 位置变化观察者 |
| `ItemsPresenterWidth`（只读） | double | 0 | 计算出的内容展示区宽度 |
| `ItemsPresenterHeight`（只读） | double | 0 | 计算出的内容展示区高度 |

**关键方法：**
- `AddToSource(object, AddLocationHint)` / `AddToSource(object, object, AddLocationHint)` — 带位置提示的添加
- `MoveItem(MoveItemRequest)` — 在布局中移动项
- `InstigateDrag(object, Action<DragablzItem>)` — 以编程方式启动拖拽
- `DragablzItems()` → `IEnumerable<DragablzItem>` — 获取所有 DragablzItem 容器

**关键逻辑：** `ItemDragDelta` 通过 `ItemsOrganiser.ConstrainLocation` 约束位置、更新 X/Y，然后调用 `OrganiseOnDrag` 重新排列兄弟项。`MeasureOverride` 支持 `LockedMeasure`（用于拖拽传输）。

**Internal 属性：** `ContainerCustomisations`（容器生命周期委托）、`LockedMeasure`（Size?，用于锁定测量）

---

#### `DragablzWindow` : `Window`
**文件：** `Dragablz/DragablzWindow.cs`

无边框 Window，带自定义拖拽、调整大小和系统命令按钮。

**模板部件：** `PART_WindowSurface`（Grid — 用于拖拽窗口）、`PART_WindowRestoreThumb`（Thumb — 还原）、`PART_WindowResizeThumb`（Thumb — 调整大小）

**静态命令：** `CloseWindowCommand`、`RestoreWindowCommand`、`MaximizeWindowCommand`、`MinimizeWindowCommand`

**依赖属性：** `IsBeingDraggedByTab`（bool，只读）— 窗口正被单个 Tab 拖拽

**关键方法：**
- `OnApplyTemplate()` — 绑定窗口表面 Grid、还原 Thumb、调整大小 Thumb
- `CriticalHandle` → IntPtr — 通过反射获取 Window 原生句柄

**核心 private 方法：**
- `WindowSurfaceGridOnMouseLeftButtonDown(...)` — 单击 DragMove，双击最大化
- `WindowResizeThumbOnDragDelta(...)` — 处理 8 方向调整大小，含 DPI 缩放，限制在 MinWidth/MinHeight
- `SelectSizingMode(Point, Size)` → SizeGrip（静态）— 根据鼠标在 Thumb 中的位置确定是边角还是边缘
- `SelectCursor(SizeGrip)` → Cursor（静态）— 返回合适的调整大小光标
- 系统命令处理器通过 `Native.PostMessage` 发送 `WM_SYSCOMMAND`

---

#### `InterTabController` : `FrameworkElement`
**文件：** `Dragablz/InterTabController.cs`

跨 Tab 拖拽的配置节点。挂载在 `TabablzControl.InterTabController` 属性上。控制边界容忍度、窗口行为、分区和客户端实现。

**依赖属性：**

| DP 名称 | 类型 | 默认值 | 用途 |
|---------|------|--------|------|
| `HorizontalPopoutGrace` | double | 8 | 水平方向超出容忍距离（超出此值视为"弹出"） |
| `VerticalPopoutGrace` | double | 8 | 垂直方向超出容忍距离 |
| `MoveWindowWithSolitaryTabs` | bool | true | 单个 Tab 时拖拽整个窗口 |
| `InterTabClient` | IInterTabClient | new DefaultInterTabClient() | 创建新窗口、处理 Tab 清空 |
| `Partition` | object | null | 分区键（null = 全局分区，仅同分区可交换 Tab） |

---

#### `Trapezoid` : `ContentControl`
**文件：** `Dragablz/Trapezoid.cs`

绘制梯形 Tab 背景，使用带圆角的 `PathGeometry`。

**依赖属性：** `PenBrush`（Brush，梯形轮廓线刷）、`LongBasePenBrush`（Brush，长底边刷）、`PenThickness`（double，线宽）

**关键逻辑：** `MeasureOverride` 创建冻结的 `PathGeometry` 并裁剪控件。`OnRender` 绘制几何图形和长底边强调线。用于 MahApps 主题。

---

#### `DragablzIcon` : `Control`
**文件：** `Dragablz/DragablzIcon.cs`

最简控件——仅重写 `DefaultStyleKey`。在模板中作为可样式化图标占位符使用。

---

### 3.2 命名空间 `Dragablz`——接口

#### `IItemsOrganiser`
**文件：** `Dragablz/IItemsOrganiser.cs`

布局 `DragablzItem` 实例的策略接口。方法：

| 方法 | 用途 |
|------|------|
| `Organise(DragablzItemsControl, Size, IEnumerable<DragablzItem>)` | 对未排序项进行完整布局 |
| `Organise(DragablzItemsControl, Size, IOrderedEnumerable<DragablzItem>)` | 对已排序项进行完整布局（优化变体） |
| `OrganiseOnMouseDownWithin(...)` | 响应鼠标按下（如置顶） |
| `OrganiseOnDragStarted(...)` | 拖拽开始时快照当前状态 |
| `OrganiseOnDrag(...)` | 拖拽过程中重新排列兄弟项 |
| `OrganiseOnDragCompleted(...)` | 拖拽结束后确定最终位置和逻辑索引 |
| `ConstrainLocation(...)` → Point | 将期望位置限制在有效边界内 |
| `Measure(...)` → Size | 测量所需总尺寸 |
| `Sort(IEnumerable<DragablzItem>)` → `IEnumerable<DragablzItem>` | 按显示顺序排序 |

---

#### `IInterTabClient`
**文件：** `Dragablz/IInterTabClient.cs`

Tab 被撕裂出窗口时创建新窗口的约定。

- `GetNewHost(IInterTabClient, object partition, TabablzControl source)` → `INewTabHost<Window>`——创建新宿主窗口
- `TabEmptiedHandler(TabablzControl, Window)` → `TabEmptiedResponse`——Tab 清空后的处理策略

#### `IManualInterTabClient` : `IInterTabClient`
**文件：** `Dragablz/IManualInterTabClient.cs`

扩展 `IInterTabClient`，增加手动集合管理：`Add(object item)`、`Remove(object item)`

#### `IInterLayoutClient`
**文件：** `Dragablz/IInterLayoutClient.cs`

在同一窗口内创建新 Tab 控件（用于分支布局）的约定：
- `GetNewHost(object partition, TabablzControl source)` → `INewTabHost<UIElement>`

#### `INewTabHost<out TElement>` where TElement : UIElement
**文件：** `Dragablz/INewTabHost.cs`

创建新宿主的结果：
- `TElement Container { get; }`——新窗口/UIElement
- `TabablzControl TabablzControl { get; }`——内部的新 Tab 控件

---

### 3.3 命名空间 `Dragablz`——布局器（Organisers）

#### `StackOrganiser`（抽象） : `IItemsOrganiser`
**文件：** `Dragablz/StackOrganiser.cs`

水平/垂直线性堆叠布局的抽象基类。使用 Canvas 附加属性（Left/Top）。通过 `Storyboard`（200ms cubic ease-out）实现动画过渡。

**构造函数：** `StackOrganiser(Orientation orientation, double itemOffset = 0)`

**关键逻辑：**
- **拖拽时排序：** 拖拽开始时快照兄弟项位置。拖拽过程中，将被拖拽项插入到与其当前位置匹配的排序位置。非拖拽项保持快照中的相对顺序不变。
- **动画：** `SendToLocation` 使用 `DoubleAnimationUsingKeyFrames` 动画；通过 `_activeStoryboardTargetLocations` 字典防止重复动画。
- **Z 轴顺序：** 选中/被拖拽项获得 `int.MaxValue`，其他项从 `int.MaxValue - 1` 递减。

**嵌套类：** `LocationInfo`（private）——持有 DragablzItem 以及计算出的 Start、Mid、End 位置，用于拖拽时判断插入位置。

#### `HorizontalOrganiser` : `StackOrganiser`
**文件：** `Dragablz/HorizontalOrganiser.cs`

水平堆叠实现。构造函数：`HorizontalOrganiser()`、`HorizontalOrganiser(double itemOffset)`。

#### `VerticalOrganiser` : `StackOrganiser`
**文件：** `Dragablz/VerticalOrganiser.cs`

垂直堆叠实现。构造函数：`VerticalOrganiser()`。

#### `CanvasOrganiser` : `IItemsOrganiser`
**文件：** `Dragablz/CanvasOrganiser.cs`

自由画布布局。不设置位置——项由用户或数据绑定定位。仅在鼠标按下时管理 Z 轴顺序，并在拖拽时约束边界。

---

### 3.4 命名空间 `Dragablz`——位置监视器（Position Monitors）

#### `PositionMonitor`
**文件：** `Dragablz/PositionMonitor.cs`

在 MVVM 场景中观察项位置变化的基类。事件：`LocationChanged`（EventHandler\<LocationChangedEventArgs\>）。

#### `StackPositionMonitor`（抽象） : `PositionMonitor`
**文件：** `Dragablz/StackPositionMonitor.cs`

线性位置监视器。事件：`LocationChanged`、`OrderChanged`（EventHandler\<OrderChangedEventArgs\>）。方法：`Sort(IEnumerable<DragablzItem>)`——按坐标排序。

#### `HorizontalPositionMonitor` : `StackPositionMonitor`
**文件：** `Dragablz/HorizontalPositionMonitor.cs`

水平位置监视器。按 X 坐标排序。

#### `VerticalPositionMonitor` : `StackPositionMonitor`
**文件：** `Dragablz/VerticalPositionMonitor.cs`

垂直位置监视器。按 Y 坐标排序。

---

### 3.5 命名空间 `Dragablz`——辅助类型

#### `DragablzDragStartedEventArgs` : `DragablzItemEventArgs`
**文件：** `Dragablz/DragablzDragStartedEventArgs.cs`

属性：`DragStartedEventArgs DragStartedEventArgs`。委托：`DragablzDragStartedEventHandler`。

#### `DragablzDragDeltaEventArgs` : `DragablzItemEventArgs`
**文件：** `Dragablz/DragablzDragDeltaEventArgs.cs`

属性：`DragDeltaEventArgs DragDeltaEventArgs`、`bool Cancel`。委托：`DragablzDragDeltaEventHandler`。

#### `DragablzDragCompletedEventArgs` : `RoutedEventArgs`
**文件：** `Dragablz/DragablzDragCompletedEventArgs.cs`

属性：`DragablzItem DragablzItem`、`DragCompletedEventArgs DragCompletedEventArgs`。委托：`DragablzDragCompletedEventHandler`。

#### `DragablzItemEventArgs` : `RoutedEventArgs`
**文件：** `Dragablz/DragablzItemEventArgs.cs`

属性：`DragablzItem DragablzItem`。委托：`DragablzItemEventHandler`。

#### `ItemActionCallbackArgs<TOwner>` where TOwner : FrameworkElement
**文件：** `Dragablz/ItemActionCallbackArgs.cs`

属性：`Window`、`TOwner Owner`、`DragablzItem`、`bool IsCancelled`。方法：`Cancel()`。委托：`ItemActionCallback`（用于 `ClosingItemCallback`/`ConsolidatingOrphanedItemCallback`）。

#### `LocationChangedEventArgs` : `EventArgs`
**文件：** `Dragablz/LocationChangedEventArgs.cs`

属性：`object Item`、`Point Location`。

#### `OrderChangedEventArgs` : `EventArgs`
**文件：** `Dragablz/OrderChangedEventArgs.cs`

属性：`object[] PreviousOrder`、`object[] NewOrder`。

#### `MoveItemRequest`
**文件：** `Dragablz/MoveItemRequest.cs`

属性：`object Item`、`object Context`、`AddLocationHint AddLocationHint`。封装跨 Tab 控件移动项的请求。

#### `NewTabHost<TElement>` : `INewTabHost<TElement>` where TElement : UIElement
**文件：** `Dragablz/NewTabHost.cs`

简单容器：`TElement Container`、`TabablzControl TabablzControl`。

#### `DefaultInterTabClient` : `IInterTabClient`
**文件：** `Dragablz/DefaultInterTabClient.cs`

- `GetNewHost(...)`——通过 `Activator.CreateInstance(sourceWindow.GetType())` 创建同类型新窗口；在逻辑树中查找 TabablzControl；清除设计时项。
- `TabEmptiedHandler(...)` → `CloseWindowOrLayoutBranch`。

#### `DefaultInterLayoutClient` : `IInterLayoutClient`
**文件：** `Dragablz/DefaultInterLayoutClient.cs`

- `GetNewHost(...)`——创建新 `TabablzControl`，克隆所有本地 DP 值（跳过只读属性、FrameworkElement 值和 BindingExpressionBase 值），复制 DataContext。
- `Clone(DependencyObject from, DependencyObject to)`（private）— 通过 `GetLocalValueEnumerator` 遍历本地 DP。

#### `HeaderedItemViewModel` : `INotifyPropertyChanged`
**文件：** `Dragablz/HeaderedItemViewModel.cs`

MVVM 辅助类。属性：`Header`、`Content`、`IsSelected`（均支持 PropertyChanged 通知）。

#### `DragablzColors`（静态类）
**文件：** `Dragablz/DragablzColors.cs`

基于 DWM 颜色参数的静态笔刷：`WindowBaseColor`（可变的 Color）、`WindowGlassBrush`、`WindowGlassBalancedBrush`、`WindowInactiveBrush`。使用 `Native.DwmGetColorizationParameters` 获取颜色。`ToColor` 将 DWM 0x00BBGGRR 格式转换为 WPF Color。

#### `TabablzHeaderSizeConverter` : `IMultiValueConverter`
**文件：** `Dragablz/TabablzHeaderSizeConverter.cs`

计算头部可用尺寸：totalSize - sum(兄弟项尺寸)，限制在最大值范围内。

#### `TabablzItemStyleSelector` : `StyleSelector`
**文件：** `Dragablz/TabablzItemStyleSelector.cs`

根据项是否 `is TabItem` 返回两种 Style 之一。

#### `StoryboardCompletionListener`（internal）
**文件：** `Dragablz/StoryboardCompletionListener.cs`

监听 `Storyboard.Completed` 事件，完成后调用回调。提供 `WhenComplete()` 扩展方法。在 `StackOrganiser.SendToLocation` 中用于动画结束后的清理。

#### `ContainerCustomisations`（internal）
**文件：** `Dragablz/ContainerCustomisations.cs`

三个委托属性：`GetContainerForItemOverride`、`PrepareContainerForItemOverride`、`ClearingContainerForItemOverride`。注入到 `DragablzItemsControl` 中以自定义容器生命周期。

---

### 3.6 命名空间 `Dragablz`——枚举

| 枚举 | 文件 | 值 | 说明 |
|------|------|-----|------|
| `AddLocationHint` | `LocationHint.cs` | `First`、`Last`、`Prior`、`After` | 新 Tab 的插入位置提示 |
| `EmptyHeaderSizingHint` | `EmptyHeaderSizingHint.cs` | `Collapse`、`PreviousTab` | 无 Tab 时头部尺寸策略 |
| `TabEmptiedResponse` | `TabEmptiedResponse.cs` | `CloseWindowOrLayoutBranch`、`CloseLayoutBranch`、`DoNothing` | Tab 清空后的响应策略 |

---

### 3.7 命名空间 `Dragablz.Dockablz`——布局引擎

#### `Layout` : `ContentControl`
**文件：** `Dragablz/Dockablz/Layout.cs`

根停靠容器。可直接承载 `TabablzControl`，也可承载递归的 `Branch` 树。通过内部 `DragablzItemsControl` 管理浮动/MDI 项。

**模板部件：** `PART_TopDropZone`、`PART_RightDropZone`、`PART_BottomDropZone`、`PART_LeftDropZone`、`PART_FloatDropZone`、`PART_FloatContentPresenter`

**静态命令：** `UnfloatItemCommand`、`MaximiseFloatingItem`、`RestoreFloatingItem`、`CloseFloatingItem`、`TileFloatingItemsCommand`、`TileFloatingItemsVerticallyCommand`、`TileFloatingItemsHorizontallyCommand`

**依赖属性（15 个）：**
`Partition`、`InterLayoutClient`（IInterLayoutClient，默认 DefaultInterLayoutClient）、`IsParticipatingInDrag`（只读）、`BranchTemplate`、`IsFloatDropZoneEnabled`、`FloatingItemsContainerMargin`、`FloatingItemsSource`、`FloatingItemsControlStyle`、`FloatingItemContainerStyle`、`FloatingItemContainerStyleSelector`（默认 CouldBeHeaderedStyleSelector）、`FloatingItemTemplate`、`FloatingItemTemplateSelector`、`FloatingItemHeaderMemberPath`、`FloatingItemDisplayMemberPath`、`ClosingFloatingItemCallback`

**附加属性：** `IsFloatingInLayout`（只读）、`IsTopLeftItem`（只读，标识布局中最左上角的 Tab 控件）、`FloatingItemState`（WindowState）、`LocationSnapShot`（internal，浮动项最大化/还原时的位置快照）

**关键静态方法：**
- `GetLoadedInstances()` → `IEnumerable<Layout>`——获取所有已加载 Layout
- `Find(TabablzControl)` → `LocationReport`——在布局中定位 Tab 控件
- `Branch(TabablzControl, Orientation, bool makeSecond, double proportion)`——在指定 Tab 控件位置创建分支
- `ConsolidateBranch(DependencyObject redundantNode)` → bool——移除冗余 Branch，将存活的子项上提

**关键实例方法：**
- `OnApplyTemplate()` — 从模板获取 6 个 DropZone 和浮动 ContentPresenter
- `FloatingDragablzItems()` → `IEnumerable<DragablzItem>`

**核心 private 方法：**
- `MonitorDropZones(Point cursorPos)` — 遍历所有 DropZone，对命中项设置 `IsOffered = true`
- `SetupParticipatingLayouts()` — 标记所有匹配分区的 Layout 为参与拖拽
- `Float(Layout, DragablzItem)` — 从源 Tab 控件移除项，添加到浮动集合
- `Branch(DropZoneLocation, DragablzItem)` — 在 DropZone 位置创建新 Branch

---

#### `Branch` : `Control`
**文件：** `Dragablz/Dockablz/Branch.cs`

两窗格分割容器（水平或垂直），支持按比例调整大小。

**模板部件：** `PART_FirstContentPresenter`、`PART_SecondContentPresenter`

**依赖属性：** `Orientation`（方向）、`FirstItem`（object，第一项内容）、`FirstItemLength`（GridLength，Star ~0.5，默认双向绑定）、`SecondItem`（object）、`SecondItemLength`（GridLength，Star ~0.5，默认双向绑定）

**属性：** `FirstContentPresenter`、`SecondContentPresenter`（internal）

**方法：** `GetFirstProportion()` → double — 返回第一项的比例大小 `1/(First+Second) * First`

---

#### `DropZone` : `Control`
**文件：** `Dragablz/Dockablz/DropZone.cs`

停靠拖放操作的边缘区域目标控件。

**依赖属性：** `Location`（DropZoneLocation，标识位置）、`IsOffered`（bool，只读，internal setter——当拖拽项悬停时为 true）

---

#### `LayoutAccessor`
**文件：** `Dragablz/Dockablz/LayoutAccessor.cs`

访问者模式的布局内容访问器。

**属性：** `Layout`、`FloatingItems`（IEnumerable\<DragablzItem\>）、`BranchAccessor`、`TabablzControl`

**方法：**
- `Visit(Action<BranchAccessor>, Action<TabablzControl>, Action<object>)` → 返回 this（流式 API）——根据内容类型调用合适的回调
- `TabablzControls()` → `IEnumerable<TabablzControl>`——递归收集布局中所有 Tab 控件

---

#### `BranchAccessor`
**文件：** `Dragablz/Dockablz/BranchAccessor.cs`

访问者模式的分支内容访问器。

**属性：** `Branch`、`FirstItemBranchAccessor`（递归）、`SecondItemBranchAccessor`（递归）、`FirstItemTabablzControl`、`SecondItemTabablzControl`

**方法：** `Visit(BranchItem, Action<BranchAccessor>, Action<TabablzControl>, Action<object>)` → 返回 this

---

#### `BranchResult`
**文件：** `Dragablz/Dockablz/BranchResult.cs`

创建分支的结果。属性：`Branch`、`TabablzControl`。

#### `LocationReport`
**文件：** `Dragablz/Dockablz/LocationReport.cs`

在布局中查找 TabablzControl 的结果。属性：`TabablzControl`、`RootLayout`（Layout）、`ParentBranch`（Branch?，父分支）、`IsLeaf`、`IsSecondLeaf`。

#### `LocationReportBuilder`（internal）
**文件：** `Dragablz/Dockablz/LocationReportBuilder.cs`

LocationReport 的可变构建器。方法：`MarkFound()`、`MarkFound(Branch, bool isSecondLeaf)`、`ToLocationReport()` → LocationReport。仅允许标记一次找到的结果。

#### `Finder`（internal 静态类）
**文件：** `Dragablz/Dockablz/Finder.cs`

方法：`Find(TabablzControl)` → LocationReport——遍历所有已加载 Layout 实例，使用 LocationReportBuilder 和 LayoutAccessor.Visit 模式查找 Tab 控件。

#### `Tiler`（internal 静态类）
**文件：** `Dragablz/Dockablz/Tiler.cs`

排列浮动项。方法：
- `Tile(IEnumerable<DragablzItem>, Size bounds)`——网格布局
- `TileHorizontally(...)`——单行水平排列
- `TileVertically(...)`——单列垂直排列

#### `TilerCalculator`（internal 静态类）
**文件：** `Dragablz/Dockablz/TilerCalculator.cs`

方法：`GetCellCountPerColumn(int totalCells)` → int[]——计算每列单元格数。完美平方数全分 sqrt；否则从右向左分配余数。

#### `FloatingItemSnapShot`（internal）
**文件：** `Dragablz/Dockablz/FloatingItemSnapShot.cs`

捕获/恢复浮动项状态（位置、大小、ZIndex、WindowState）。方法：`Take(DragablzItem)` → FloatingItemSnapShot、`Apply(DragablzItem)`。

#### `LocationSnapShot`（internal）
**文件：** `Dragablz/Dockablz/LocationSnapShot.cs`

仅捕获/恢复浮动项的 Width/Height。在最大化/还原后使用。方法：`Take(FrameworkElement)`、`Apply(FrameworkElement)`。

#### `FloatTransfer`（internal）
**文件：** `Dragablz/Dockablz/FloatTransfer.cs`

Tab 移到浮动层时的瞬态状态。属性：`Content`（object）、`Width`/`Height`（已标记 Obsolete）。

#### `FloatRequestedEventArgs` : `DragablzItemEventArgs`
**文件：** `Dragablz/Dockablz/FloatRequestedEvent.cs`

浮动请求的事件参数。委托：`FloatRequestedEventHandler`。

#### `CouldBeHeaderedStyleSelector` : `StyleSelector`
**文件：** `Dragablz/Dockablz/CouldBeHeaderedStyleSelector.cs`

如果容器是 `HeaderedDragablzItem` 或 `HeaderedContentControl`，则返回 `HeaderedStyle`，否则返回 `NonHeaderedStyle`。用于浮动项的样式选择。

#### `LocationReportException` : `Exception`
**文件：** `Dragablz/Dockablz/LocationReportException.cs`

在布局中找不到 TabablzControl 时抛出。

#### `Extensions`（Dockablz）
**文件：** `Dragablz/Dockablz/Extensions.cs`

流式访问者扩展方法：`Layout.Query()` → LayoutAccessor，以及带上下文的 `Visit<TContext>` 重载。

---

### 3.8 命名空间 `Dragablz.Dockablz`——枚举

| 枚举 | 文件 | 值 | 说明 |
|------|------|-----|------|
| `BranchItem` | `BranchItem.cs` | `First`、`Second` | 标识 Branch 的第一项或第二项 |
| `DropZoneLocation` | `DropZoneLocation.cs` | `Top`、`Right`、`Bottom`、`Left`、`Floating` | 标识放置区域位置 |

---

### 3.9 命名空间 `Dragablz.Core`（Internal）

#### `Extensions`（Core）
**文件：** `Dragablz/Core/Extensions.cs`

树遍历和窗口几何辅助方法：
- `Containers<TContainer>(this ItemsControl)` → `IEnumerable<TContainer>`——按索引遍历项容器
- `LogicalTreeDepthFirstTraversal(this DependencyObject)`——逻辑树深度优先遍历
- `VisualTreeDepthFirstTraversal(this DependencyObject)`——可视树深度优先遍历
- `VisualTreeAncestory(this DependencyObject)`——可视树父级链（含自身）
- `LogicalTreeAncestory(this DependencyObject)`——逻辑树父级链（含自身）
- `GetActualLeft(this Window)` / `GetActualTop(this Window)` → double——获取窗口实际位置（最大化时通过反射读取 `_actualLeft`/`_actualTop` 私有字段）

#### `Native`（internal 静态类）
**文件：** `Dragablz/Core/Native.cs`

P/Invoke 封装：
- `GetCursorPos()` → Point（WPF）/ `GetRawCursorPos()` → POINT——获取光标位置
- `GetDC`、`GetDeviceCaps`、`ReleaseDC`——DPI 获取
- `SetWindowPlacement`、`SendMessage`、`PostMessage`——窗口操作
- `DwmGetColorizationParameters`——DWM 颜色参数（用于主题）
- `SortWindowsTopToBottom(IEnumerable<Window>)`——按 Z 轴顺序排序窗口
- `ToWpf(this Point pixelPoint)` → Point——像素坐标转 WPF 设备无关坐标

**嵌套结构体：** `POINT`（含到 Point 的隐式转换）、`RECT`、`WINDOWPLACEMENT`、`DWMCOLORIZATIONPARAMS`

#### `InstanceRegistry<T>` where T : class
**文件：** `Dragablz/Core/InstanceRegistry.cs`

线程安全的弱引用注册表。方法：`Register(T)`、`Unregister(T)`、`GetAliveInstances()` → `IEnumerable<T>`（同时清理死引用）、`Cleanup()`、`Clear()`。用于 `TabablzControl._loadedInstances` 和 `Layout._loadedLayouts`。

#### `InterTabTransfer`
**文件：** `Dragablz/Core/InterTabTransfer.cs`

跨 Tab 传输的状态载体。两个构造函数：一个用于 breach（拖出），一个用于 reentry（重新进入）。属性：`BreachOrientation`、`DragStartWindowOffset`、`Item`、`OriginatorContainer`、`TransferReason`、`DragStartItemOffset`、`ItemPositionWithinHeader`、`ItemSize`、`FloatingItemSnapShots`、`IsTransposing`。

**嵌套枚举：** `InterTabTransferReason { Breach, Reentry }`

#### `TabHeaderDragStartInformation`
**文件：** `Dragablz/Core/TabHeaderDragStartInformation.cs`

记录拖拽起始时的偏移量：`DragablzItemsControlHorizontalOffset`、`DragablzItemControlVerticalOffset`、`DragablzItemHorizontalOffset`、`DragablzItemVerticalOffset`、`DragItem`。

#### `CollectionTeaser`
**文件：** `Dragablz/Core/CollectionTeaser.cs`

通过反射将任意对象进行集合"鸭子类型"包装，统一提供 Add/Remove 操作。`TryCreate(object items, out CollectionTeaser)` → bool——包装 IList 或 ICollection\<T\>。方法：`Add(object)`、`Remove(object)`。

#### `FuncComparer<TObject>` : `IComparer<TObject>`
**文件：** `Dragablz/Core/FuncComparer.cs`

将 `Func<TObject, TObject, int>` 包装为 `IComparer<TObject>`。

#### `MultiComparer<TObject>` : `IComparer<TObject>`
**文件：** `Dragablz/Core/MultiComparer.cs`

链式比较器。工厂方法：`Ascending<TAttribute>(accessor)`、`Descending<TAttribute>(accessor)`。流式扩展：`.ThenAscending(...)`、`.ThenDescending(...)`。Compare 返回第一个非零比较结果。

#### Core 中的枚举
- `HitTest`——Win32 HT* 常量（HTCLIENT=1、HTCAPTION=2 等 23 个值）
- `SystemCommand`——Win32 SC_* 常量（SC_CLOSE=0xF060、SC_MAXIMIZE=0xF030 等 20 个值）
- `WindowMessage`——约 130 个 WM_* 消息标识符（含键盘、鼠标、DWM、输入等）
- `WindowSizingMessage`——WMSZ_* 常量（WMSZ_BOTTOM=6、WMSZ_TOPLEFT=4 等 8 个值）

---

### 3.10 命名空间 `Dragablz.Converters`

| 类 | 实现 | 逻辑 |
|-----|------|------|
| `BooleanAndToVisibilityConverter` | IMultiValueConverter | 所有布尔输入 AND → Visible/Collapsed |
| `EqualityToBooleanConverter` | IValueConverter | `Equals(value, parameter)` → bool |
| `EqualityToVisibilityConverter` | IValueConverter | `Equals(value, parameter)` → Visible/Collapsed |
| `ShowDefaultCloseButtonConverter` | IMultiValueConverter | ShowDefaultCloseButton 为 true 且 LogicalIndex >= FixedHeaderCount → Visible |

---

### 3.11 命名空间 `Dragablz.Themes`

#### `Ripple` : `ContentControl`
**文件：** `Dragablz/Themes/Ripple.cs`

Material Design 墨迹涟漪效果控件。三个视觉状态：Normal、MousePressed、MouseOut。全局追踪所有按下状态的 Ripple 实例，以处理跨窗口/UserControl 的鼠标弹起和移动事件。

**DP：** `Feedback`（Brush，涟漪颜色）、`RippleSize`（double，只读，涟漪直径）、`RippleX`（double，只读）、`RippleY`（double，只读）、`RecognizesAccessKey`（bool）

**关键逻辑：** `OnPreviewMouseLeftButtonDown` 将涟漪定位到点击位置（若 `IsCentered` 为 true 则居中）。全局 `MouseButtonEventHandler` 在鼠标弹起时按当前缩放比例等比缩减动画时间。

#### `RippleAssist`（静态类）
**文件：** `Dragablz/Themes/RippleAssist.cs`

附加 DP（均支持继承）：`ClipToBounds`（bool，默认 true）、`IsCentered`（bool，默认 false）、`RippleSizeMultiplier`（double，默认 1.0）。

#### `SystemCommandIcon` : `Control`
**文件：** `Dragablz/Themes/SystemCommandIcon.cs`

渲染窗口系统命令图标的控件。DP：`SystemCommandType`（SystemCommandType 枚举：`CloseWindow`、`MaximizeWindow`、`MinimzeWindow`、`RestoreWindow`）。

#### `MaterialDesignAssist`（静态类）
**文件：** `Dragablz/Themes/MaterialDesignAssist.cs`

附加 DP：`IndicatorBrush`（Brush）——Material Design Tab 的下划线指示器笔刷。

#### `BrushToRadialGradientBrushConverter` : `IValueConverter`
**文件：** `Dragablz/Themes/BrushToRadialGradientBrushConverter.cs`

SolidColorBrush → RadialGradientBrush（中心渐变到透明，Opacity 0.39）。用于涟漪效果背景。

---

### 3.12 命名空间 `Dragablz.Referenceless`（Internal）

轻量级 Rx 风格 Disposable 实现（避免依赖 System.Reactive）：

| 类 | 实现 | 用途 |
|-----|------|------|
| `ICancelable` | IDisposable | 含 `IsDisposed` 属性的接口 |
| `Disposable`（静态） | — | `Empty` → DefaultDisposable 单例；`Create(Action)` → AnonymousDisposable |
| `DefaultDisposable` | IDisposable | 单例空操作 `Instance`（Dispose() 无操作） |
| `AnonymousDisposable` | ICancelable | 包装一个 `Action`；通过 `Interlocked.Exchange` 确保仅执行一次 |
| `SerialDisposable` | ICancelable | 持有一个 IDisposable；原子替换（替换时自动释放前一个，自身释放时释放当前） |

---

## 4. 子系统架构

### 4.1 Tab 拖拽流水线

```
用户拖拽 Thumb
    │
    ▼
DragablzItem.ThumbOnDragStarted()         ← 记录 MouseAtDragStart
    │  引发 DragStarted 事件
    ▼
DragablzItemsControl.ItemDragStarted()    ← 通知 ItemsOrganiser
    │
    ▼
TabablzControl.ItemDragStarted()          ← 记录 TabHeaderDragStartInformation，管理选择状态
    │
    ▼
DragablzItem.ThumbOnDragDelta()           ← 引发 PreviewDragDelta（隧道）+ DragDelta（冒泡）
    │
    ├─► DragablzItemsControl.ItemDragDelta()
    │     ├─ ItemsOrganiser.ConstrainLocation() → 约束后的位置
    │     ├─ 设置 DragablzItem.X/Y
    │     └─ ItemsOrganiser.OrganiseOnDrag() → 重新排列兄弟项（带动画）
    │
    └─► TabablzControl.PreviewItemDragDelta()  ← 判断是否应拖拽整个窗口
    │     ItemDragDelta() → MonitorBreach()     ← 越界检测
    │       └─ 若越界：通过 InterTabClient 创建新窗口
    │         └─ MonitorReentry()               ← 重入检测
    │           └─ 若重入：目标 TabablzControl.ReceiveDrag(InterTabTransfer)
    │
    ▼
DragablzItem.ThumbOnDragCompleted()       ← 引发 DragCompleted
    │
    ▼
DragablzItemsControl.ItemDragCompleted()  ← 确定最终位置，重置 IsDragging/IsSiblingDragging
```

### 4.2 Dockablz 布局树

```
Layout (ContentControl)
├── Content = TabablzControl          ← 叶节点：单个 Tab 组
│   或
├── Content = Branch (Control)
│   ├── FirstItem  = TabablzControl | Branch  ← 递归嵌套
│   └── SecondItem = TabablzControl | Branch  ← 递归嵌套
└── _floatingItems (DragablzItemsControl)  ← MDI 浮动层
```

**拖拽时的放置区检测流程：**
1. `Layout.DragStarted` → `SetupParticipatingLayouts()` 标记所有 Layout 参与拖拽
2. `Layout.PreviewDragDelta` → `MonitorDropZones()` 对每个 DropZone 做 InputHitTest
3. `Layout.DragCompleted` → 若有 DropZone 被悬停（IsOffered），调用 `Float()` 或 `Branch()`

**合并逻辑：** 当 Branch 叶节点（Tab）被移除后只剩一个子项时，`Layout.ConsolidateBranch()` 将 Branch 替换为存活的子项，削减树深度。

### 4.3 ItemsOrganiser 策略模式

```
IItemsOrganiser
├── StackOrganiser（抽象）── 使用 Canvas.Left/Top，带动画过渡
│   ├── HorizontalOrganiser ── 从左到右水平排列
│   └── VerticalOrganiser   ── 从上到下垂直排列
└── CanvasOrganiser ── 自由画布，仅管理 Z 轴顺序
```

### 4.4 PositionMonitor 观察者模式

```
PositionMonitor
└── StackPositionMonitor（抽象）
    ├── HorizontalPositionMonitor ── 按 X 坐标跟踪顺序
    └── VerticalPositionMonitor   ── 按 Y 坐标跟踪顺序
```

通过 `DragablzItemsControl.PositionMonitor` 属性绑定。在 `ItemXChanged`/`ItemYChanged` 处理器中更新。

---

## 5. XAML 主题资源

### Generic.xaml（~1909 行）— `Dragablz/Themes/Generic.xaml`

默认主题。关键资源：

| Key | 目标类型 | 说明 |
|-----|---------|------|
| （默认） | TabablzControl | 完整模板：头部前缀/后缀、DragablzItemsControl、PART_ItemsHolder、添加按钮；TabStripPlacement 触发器覆盖 Top/Bottom/Left/Right 四种方向 |
| （默认） | DragablzItem | 30×30 黄色 Thumb + ContentPresenter；IsDragging/IsSiblingDragging 不透明度触发器 |
| （默认） | HeaderedDragablzItem | 头部含边框 + DropShadowEffect |
| `StandardDragablzTabItemStyle` | DragablzItem | 经典 WPF Tab 外观，多层边框，4 个 TabStripPlacement 方向 × IsSelected/IsMouseOver/IsEnabled 的 MultiDataTrigger |
| `TrapezoidDragableTabItemStyle` | DragablzItem | 梯形 Tab（用于 MahApps 风格重叠效果） |
| `ToolDragablzItemStyle` | HeaderedDragablzItem | 浮动工具窗口：8 方向调整大小 Thumb、关闭/解除浮动/最大化/还原按钮 |
| （默认） | DragablzWindow | 无边框窗口 + 系统命令按钮；IsBeingDraggedByTab 时折叠命令面板 |
| （默认） | Layout | 4 个方向 DropZone + 浮动 DropZone；CouldBeHeaderedStyleSelector |
| （默认） | Branch | 水平/垂直 Grid 切换；GridSplitter |
| （默认） | DropZone | 按位置显示不同弧形/椭圆几何路径 |
| `TabablzDragablzItemsControlStyle` | DragablzItemsControl | 基于 ScrollViewer 的头部宿主 |
| `FloatingDragablzItemsControlStyle` | DragablzItemsControl | 基于 Canvas，拉伸填充 |

转换器实例：BooleanToVisibilityConverter、EqualityToVisibilityConverter、BooleanAndToVisibilityConverter、EqualityToBooleanConverter、ShowDefaultCloseButtonConverter、BrushToRadialGradientBrushConverter。

### MaterialDesign.xaml（~802 行）— `Dragablz/Themes/MaterialDesign.xaml`

与 Generic 结构相似，替换为 Material Design 配色。关键增强：Tab 上增加了 `Ripple` 涟漪效果、通过 `MaterialDesignAssist.IndicatorBrush` 实现选中指示器、PrimaryHue 笔刷系列支持主题化。

### MahApps.xaml（~442 行）— `Dragablz/Themes/MahApps.xaml`

MahApps.Metro 主题变体。使用梯形 Tab（`Trapezoid` 控件）、MahApps.Accent 笔刷、`AdjacentHeaderItemOffset="-12"` 实现 Tab 重叠效果。

### Dockablz.xaml — `Dragablz/Themes/Dockablz.xaml`

空 ResourceDictionary。声明了 XML 命名空间但不含任何资源，作为扩展占位。

---

## 6. 关键交互流程

### 6.1 Tab 撕裂 → 创建新窗口

1. 用户开始拖拽 `DragablzItem.PART_Thumb`
2. `DragablzItem` 引发 `DragStarted` → `DragDelta` 事件
3. `TabablzControl.ItemDragStarted()` 记录 `TabHeaderDragStartInformation`
4. `TabablzControl.ItemDragDelta()` 调用 `MonitorBreach()`
5. 当鼠标超出头部边界（加上 `InterTabController.HorizontalPopoutGrace`/`VerticalPopoutGrace` 容忍值）：
   - 创建 `InterTabTransfer`（breach 类型），包含完整状态
   - 调用 `InterTabClient.GetNewHost()` → 创建同类型新 Window
   - 将新窗口定位到光标位置
   - 新 TabablzControl 调用 `ReceiveDrag(InterTabTransfer)` 接收拖来的 Tab
   - 原始 Tab 从源集合中移除
6. `DragCompleted` 时若未重入其他控件：原始 Tab 留在新窗口中

### 6.2 Dockablz 分支放置（Branch Drop）

1. Tab 在 `Layout` 范围内拖拽
2. `Layout.PreviewItemDragDelta()` → `MonitorDropZones()` 命中某 DropZone
3. 该 DropZone 的 `IsOffered` 设为 true
4. `DragCompleted` 时：
   - 若是边缘 DropZone（Top/Right/Bottom/Left）：调用 `Layout.Branch(DropZoneLocation, DragablzItem)`
     - 根据位置确定方向（Top/Bottom → Vertical，Left/Right → Horizontal）
     - 通过 `InterLayoutClient.GetNewHost()` 或 `BranchTemplate` 创建新内容
     - 根据放置位置设置 FirstItem/SecondItem
     - 默认比例为 0.5
   - 若是 `Floating` DropZone：调用 `Layout.Float(Layout, DragablzItem)`
     - 从源 TabablzControl 移除项
     - 添加到浮动 `DragablzItemsControl`

### 6.3 Tab 关闭 → 合并（Consolidation）

1. `TabablzControl.CloseItemCommand` 执行
2. `TabablzControl.CloseItem(DragablzItem, TabablzControl)`：
   - 若 `ClosingItemCallback` 返回取消 → 中止
   - 通过 `RemoveFromSource()` 从源集合移除
   - 调用 `RemoveItem()`：
     - 若为最后一个 Tab：
       - `TabEmptiedResponse.CloseWindowOrLayoutBranch` → 关闭窗口
       - `TabEmptiedResponse.CloseLayoutBranch` → 调用 `Layout.ConsolidateBranch()`
       - `TabEmptiedResponse.DoNothing` → 无操作
     - 若非最后一个 Tab：`RestorePreviousSelection()` 恢复上一个选中项
   - 若 `ConsolidateOrphanedItems` 为 true：将剩余 Tab 迁移到其他宿主

---

## 7. 文件路径索引（速查表）

### 根命名空间（`Dragablz/`）
| 文件 | 类型 |
|------|------|
| `CanvasOrganiser.cs` | CanvasOrganiser : IItemsOrganiser |
| `ContainerCustomisations.cs` | ContainerCustomisations（internal） |
| `DefaultInterLayoutClient.cs` | DefaultInterLayoutClient : IInterLayoutClient |
| `DefaultInterTabClient.cs` | DefaultInterTabClient : IInterTabClient |
| `DragablzColors.cs` | DragablzColors（静态类） |
| `DragablzDragCompletedEventArgs.cs` | DragablzDragCompletedEventArgs : RoutedEventArgs |
| `DragablzDragDeltaEventArgs.cs` | DragablzDragDeltaEventArgs : DragablzItemEventArgs |
| `DragablzDragStartedEventArgs.cs` | DragablzDragStartedEventArgs : DragablzItemEventArgs |
| `DragablzIcon.cs` | DragablzIcon : Control |
| `DragablzItem.cs` | DragablzItem : ContentControl（含 SizeGrip 枚举） |
| `DragablzItemEventArgs.cs` | DragablzItemEventArgs : RoutedEventArgs |
| `DragablzItemsControl.cs` | DragablzItemsControl : ItemsControl |
| `DragablzWindow.cs` | DragablzWindow : Window |
| `EmptyHeaderSizingHint.cs` | EmptyHeaderSizingHint 枚举 |
| `HeaderedDragablzItem.cs` | HeaderedDragablzItem : DragablzItem |
| `HeaderedItemViewModel.cs` | HeaderedItemViewModel : INotifyPropertyChanged |
| `HorizontalOrganiser.cs` | HorizontalOrganiser : StackOrganiser |
| `HorizontalPositionMonitor.cs` | HorizontalPositionMonitor : StackPositionMonitor |
| `IInterLayoutClient.cs` | IInterLayoutClient 接口 |
| `IInterTabClient.cs` | IInterTabClient + IManualInterTabClient 接口 |
| `INewTabHost.cs` | INewTabHost\<TElement\> 接口 |
| `IItemsOrganiser.cs` | IItemsOrganiser 接口 |
| `InterTabController.cs` | InterTabController : FrameworkElement |
| `ItemActionCallbackArgs.cs` | ItemActionCallbackArgs\<TOwner\> |
| `LocationChangedEventArgs.cs` | LocationChangedEventArgs : EventArgs |
| `LocationHint.cs` | AddLocationHint 枚举 |
| `MoveItemRequest.cs` | MoveItemRequest |
| `NewTabHost.cs` | NewTabHost\<TElement\> : INewTabHost\<TElement\> |
| `OrderChangedEventArgs.cs` | OrderChangedEventArgs : EventArgs |
| `PositionMonitor.cs` | PositionMonitor |
| `StackOrganiser.cs` | StackOrganiser（抽象）: IItemsOrganiser |
| `StackPositionMonitor.cs` | StackPositionMonitor（抽象）: PositionMonitor |
| `StoryboardCompletionListener.cs` | StoryboardCompletionListener（internal） |
| `TabablzControl.cs` | TabablzControl : TabControl |
| `TabablzHeaderSizeConverter.cs` | TabablzHeaderSizeConverter : IMultiValueConverter |
| `TabablzItemStyleSelector.cs` | TabablzItemStyleSelector : StyleSelector |
| `TabEmptiedResponse.cs` | TabEmptiedResponse 枚举 |
| `Trapezoid.cs` | Trapezoid : ContentControl |
| `VerticalOrganiser.cs` | VerticalOrganiser : StackOrganiser |
| `VerticalPositionMonitor.cs` | VerticalPositionMonitor : StackPositionMonitor |

### Core（`Dragablz/Core/`）
| 文件 | 类型 |
|------|------|
| `CollectionTeaser.cs` | CollectionTeaser（internal） |
| `Extensions.cs` | Extensions（internal 静态类） |
| `FuncComparer.cs` | FuncComparer\<T\> : IComparer\<T\>（internal） |
| `HitTest.cs` | HitTest 枚举（internal） |
| `InstanceRegistry.cs` | InstanceRegistry\<T\>（internal） |
| `InterTabTransfer.cs` | InterTabTransfer + InterTabTransferReason 枚举（internal） |
| `MultiComparer.cs` | MultiComparer\<T\> : IComparer\<T\>（internal） |
| `Native.cs` | Native + POINT/RECT/WINDOWPLACEMENT/DWMCOLORIZATIONPARAMS 结构体 |
| `SystemCommand.cs` | SystemCommand 枚举（internal） |
| `TabHeaderDragStartInformation.cs` | TabHeaderDragStartInformation（internal） |
| `WindowMessage.cs` | WindowMessage 枚举（internal）+ WindowSizingMessage 枚举（public） |

### Dockablz（`Dragablz/Dockablz/`）
| 文件 | 类型 |
|------|------|
| `Branch.cs` | Branch : Control |
| `BranchAccessor.cs` | BranchAccessor |
| `BranchItem.cs` | BranchItem 枚举 |
| `BranchResult.cs` | BranchResult |
| `CouldBeHeaderedStyleSelector.cs` | CouldBeHeaderedStyleSelector : StyleSelector |
| `DropZone.cs` | DropZone : Control |
| `DropZoneLocation.cs` | DropZoneLocation 枚举 |
| `Extensions.cs` | Extensions（静态类）— Query/Visit 流式 API |
| `Finder.cs` | Finder（internal 静态类） |
| `FloatRequestedEvent.cs` | FloatRequestedEventArgs + FloatRequestedEventHandler |
| `FloatTransfer.cs` | FloatTransfer（internal） |
| `FloatingItemSnapShot.cs` | FloatingItemSnapShot（internal） |
| `Layout.cs` | Layout : ContentControl |
| `LayoutAccessor.cs` | LayoutAccessor |
| `LocationReport.cs` | LocationReport |
| `LocationReportBuilder.cs` | LocationReportBuilder（internal） |
| `LocationReportException.cs` | LocationReportException : Exception |
| `LocationSnapShot.cs` | LocationSnapShot（internal） |
| `Tiler.cs` | Tiler（internal 静态类） |
| `TilerCalculator.cs` | TilerCalculator（internal 静态类） |

### Converters（`Dragablz/Converters/`）
| 文件 | 类型 |
|------|------|
| `BooleanAndToVisibilityConverter.cs` | BooleanAndToVisibilityConverter : IMultiValueConverter |
| `EqualityToBooleanConverter.cs` | EqualityToBooleanConverter : IValueConverter |
| `EqualityToVisibilityConverter.cs` | EqualityToVisibilityConverter : IValueConverter |
| `ShowDefaultCloseButtonConverter.cs` | ShowDefaultCloseButtonConverter : IMultiValueConverter |

### Themes（`Dragablz/Themes/`）
| 文件 | 类型 |
|------|------|
| `BrushToRadialGradientBrushConverter.cs` | BrushToRadialGradientBrushConverter : IValueConverter |
| `MaterialDesignAssist.cs` | MaterialDesignAssist（静态类） |
| `Ripple.cs` | Ripple : ContentControl |
| `RippleAssist.cs` | RippleAssist（静态类） |
| `SystemCommandIcon.cs` | SystemCommandIcon : Control（含 SystemCommandType 枚举） |
| `Generic.xaml` | 默认主题（~1909 行） |
| `MaterialDesign.xaml` | Material Design 主题（~802 行） |
| `MahApps.xaml` | MahApps.Metro 主题（~442 行） |
| `Dockablz.xaml` | 空白占位文件 |

### Referenceless（`Dragablz/Referenceless/`）
| 文件 | 类型 |
|------|------|
| `ICancelable.cs` | ICancelable : IDisposable（internal） |
| `Disposable.cs` | Disposable（internal 静态类） |
| `DefaultDisposable.cs` | DefaultDisposable : IDisposable（internal） |
| `AnonymousDisposable.cs` | AnonymousDisposable : ICancelable（internal） |
| `SerialDisposable.cs` | SerialDisposable : ICancelable（internal） |
