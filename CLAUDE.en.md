# Dragablz — Project Reference Map

> Complete project overview for Claude Code navigation. 91 C# source files, 4 XAML themes, 6 namespaces, 0 NuGet dependencies.

---

## 1. Project Overview

| Aspect | Detail |
|--------|--------|
| Solution | `Dragablz.sln` (VS2022) |
| Projects | Dragablz (core), Dragablz.Test (NUnit), Dragablz.Demo.Prism (WPF demo) |
| Target | `net10.0-windows`, UseWPF=true |
| NuGet Deps | None |
| Assembly Version | 0.0.4.0 |
| XAML Namespaces | `dragablz` → `Dragablz`, `dockablz` → `Dragablz.Dockablz` |
| InternalsVisibleTo | `Dragablz.Test` |

### Source File Tree (core library only)

```
Dragablz/
├── *.cs                          (40 files — root namespace Dragablz)
├── Core/
│   ├── CollectionTeaser.cs       Collection duck-typing wrapper
│   ├── Extensions.cs             Tree traversal & window geometry helpers
│   ├── FuncComparer.cs           Delegate-to-Comparer adapter
│   ├── HitTest.cs                Win32 HT* hit-test constants
│   ├── InstanceRegistry.cs       Weak-reference alive-instance registry
│   ├── InterTabTransfer.cs       Inter-tab drag transfer state
│   ├── MultiComparer.cs          Chained multi-criteria comparer
│   ├── Native.cs                 Win32/DWM P/Invoke wrappers
│   ├── SystemCommand.cs          Win32 SC_* system command constants
│   ├── TabHeaderDragStartInformation.cs  Drag-start snapshot
│   └── WindowMessage.cs          Win32 WM_* + WMSZ_* message constants
├── Converters/
│   ├── BooleanAndToVisibilityConverter.cs  Boolean AND → Visibility
│   ├── EqualityToBooleanConverter.cs       Equality → bool
│   ├── EqualityToVisibilityConverter.cs    Equality → Visibility
│   └── ShowDefaultCloseButtonConverter.cs  Close-button visibility
├── Dockablz/
│   ├── Branch.cs, BranchAccessor.cs, BranchItem.cs, BranchResult.cs
│   ├── CouldBeHeaderedStyleSelector.cs
│   ├── DropZone.cs, DropZoneLocation.cs
│   ├── Extensions.cs             Fluent Query/Visit API
│   ├── Finder.cs                 Locate TabablzControl in layout
│   ├── FloatRequestedEvent.cs, FloatTransfer.cs, FloatingItemSnapShot.cs
│   ├── Layout.cs, LayoutAccessor.cs
│   ├── LocationReport.cs, LocationReportBuilder.cs, LocationReportException.cs, LocationSnapShot.cs
│   └── Tiler.cs, TilerCalculator.cs
├── Referenceless/
│   ├── AnonymousDisposable.cs    Wraps Action, invokes once via Interlocked
│   ├── DefaultDisposable.cs      Singleton no-op IDisposable
│   ├── Disposable.cs             Factory methods
│   ├── ICancelable.cs            IDisposable + IsDisposed property
│   └── SerialDisposable.cs       Atomically replaceable disposable
├── Themes/
│   ├── BrushToRadialGradientBrushConverter.cs  SolidColorBrush → RadialGradientBrush
│   ├── MaterialDesignAssist.cs      IndicatorBrush attached property
│   ├── MaterialDesignHeaderedAssist.cs  HeaderBackground, HeaderForeground, etc. (new)
│   ├── Ripple.cs                    Material Design ink ripple effect
│   ├── RippleAssist.cs              Ripple configuration attached properties
│   ├── SystemCommandIcon.cs         Window system command icon control
│   ├── UnderlineIndicator.cs        Animated underline indicator control (new)
│   ├── Generic.xaml              (~1909 lines — default theme)
│   ├── MaterialDesign.xaml       (~1100 lines — Material Design theme, 14 styles)
│   ├── MahApps.xaml              (~442 lines — MahApps.Metro theme)
│   └── Dockablz.xaml             (empty placeholder)
└── Properties/
    ├── AssemblyInfo.cs
    ├── Resources.Designer.cs
    └── Settings.Designer.cs
```

---

## 2. Namespace Quick-Nav

| Namespace | Visibility | Purpose | Key Types |
|-----------|-----------|---------|-----------|
| `Dragablz` | Public API | Core tab/drag controls, interfaces, enums | `TabablzControl`, `DragablzItem`, `DragablzWindow` |
| `Dragablz.Dockablz` | Public API | Docking layout engine | `Layout`, `Branch`, `DropZone`, `LayoutAccessor` |
| `Dragablz.Core` | Internal | Win32 P/Invoke, tree traversal, utilities | `Native`, `Extensions`, `InstanceRegistry<T>` |
| `Dragablz.Converters` | Public | WPF value/multi-value converters | `BooleanAndToVisibilityConverter` |
| `Dragablz.Themes` | Public | Theme support (Material ripple, icons) | `Ripple`, `UnderlineIndicator`, `SystemCommandIcon` |
| `Dragablz.Referenceless` | Internal | Rx-style disposable primitives | `SerialDisposable`, `AnonymousDisposable` |

---

## 3. Class-by-Class Reference

### 3.1 Namespace `Dragablz` — Core Controls

#### `TabablzControl` : `TabControl`
**File:** `Dragablz/TabablzControl.cs`

Central tab control. Manages header layout, drag-to-tear, inter-tab transfer, close/add, and keyboard navigation.

**Template Parts:** `PART_HeaderItemsControl` (DragablzItemsControl), `PART_ItemsHolder` (Panel)

**Static Commands:** `CloseItemCommand`, `AddItemCommand`

**Static Methods:**
- `GetLoadedInstances()` → `IEnumerable<TabablzControl>` — all alive instances
- `CloseItem(object tabContentItem)` — close all tabs with given content
- `AddItem(object item, object nearItem, AddLocationHint)` — add near another item
- `SelectItem(object item)` — find and select across all instances

**Key Dependency Properties (28 total):**
`AdjacentHeaderItemOffset`, `HeaderItemsOrganiser`, `HeaderMemberPath`, `HeaderItemTemplate`, `HeaderPrefixContent` (and StringFormat/Template/TemplateSelector), `HeaderSuffixContent` (same), `ShowDefaultCloseButton`, `ShowDefaultAddButton`, `IsHeaderPanelVisible`, `AddLocationHint`, `FixedHeaderCount`, `DisableBranchConsolidation`, `InterTabController`, `NewItemFactory`, `IsEmpty` (r/o), `ClosingItemCallback`, `ClosingItemCommand`, `AddingItemCommand`, `ConsolidateOrphanedItems`, `ConsolidatingOrphanedItemCallback`, `IsDraggingWindow` (r/o), `EmptyHeaderSizingHint`

**Routed Events:** `IsEmptyChanged`, `IsDraggingWindowChanged`

**Key Methods:** `AddToSource`, `RemoveFromSource`, `GetOrderedHeaders`, `OnApplyTemplate`, `RemoveItem` (internal), `ReceiveDrag` (internal)

**Core Private:** `ItemDragStarted`, `MonitorBreach`, `MonitorReentry`, `IsTransposing`, `CloseItem`, `UpdateSelectedItem`

---

#### `DragablzItem` : `ContentControl`
**File:** `Dragablz/DragablzItem.cs`

Individual tab item. Drag via `PART_Thumb` (Thumb), resize grips, routed drag events.

**Nested Enum:** `SizeGrip { NotApplicable, Left, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft }`

**Key DPs:** `X`, `Y`, `LogicalIndex` (r/o), `SizeGrip` (attached), `ContentRotateTransformAngle` (attached, inherits), `IsSelected`, `IsDragging` (r/o), `IsSiblingDragging` (r/o), `IsCustomThumb` (attached)

**Routed Events (10):** `XChanged`, `YChanged`, `LogicalIndexChanged`, `IsDraggingChanged`, `IsSiblingDraggingChanged`, `MouseDownWithin`, `DragStarted`, `DragDelta`, `PreviewDragDelta` (Tunnel), `DragCompleted`

**Key Methods:** `OnApplyTemplate`, `InstigateDrag(Action<DragablzItem>)`

**Internal Props:** `UnderlyingContent`, `MouseAtDragStart`, `PartitionAtDragStart`, `IsDropTargetFound`

**Core Private:** `SelectAndSubscribeToThumb`, `ThumbOnDragStarted/Delta/Completed`, `SizeThumbOnDragDelta`

---

#### `HeaderedDragablzItem` : `DragablzItem`
**File:** `Dragablz/HeaderedDragablzItem.cs`

Extends `DragablzItem` with `HeaderContent`, `HeaderContentStringFormat`, `HeaderContentTemplate`, `HeaderContentTemplateSelector`.

---

#### `DragablzItemsControl` : `ItemsControl`
**File:** `Dragablz/DragablzItemsControl.cs`

Header items host. Uses `IItemsOrganiser` for layout, manages sibling drag coordination, reports position via `PositionMonitor`.

**DPs:** `FixedItemCount`, `ItemsOrganiser`, `PositionMonitor`, `ItemsPresenterWidth` (r/o), `ItemsPresenterHeight` (r/o)

**Key Methods:** `AddToSource`, `MoveItem`, `InstigateDrag`, `DragablzItems()`

**Key Logic:** `ItemDragDelta` constrains via `ItemsOrganiser.ConstrainLocation`, sets X/Y, calls `OrganiseOnDrag`. `MeasureOverride` supports `LockedMeasure` for transfers.

---

#### `DragablzWindow` : `Window`
**File:** `Dragablz/DragablzWindow.cs`

Chromeless Window with custom drag, 8-directional resize, system commands.

**Template Parts:** `PART_WindowSurface` (Grid), `PART_WindowRestoreThumb`, `PART_WindowResizeThumb`

**Static Commands:** `CloseWindowCommand`, `RestoreWindowCommand`, `MaximizeWindowCommand`, `MinimizeWindowCommand`

**DP:** `IsBeingDraggedByTab` (bool, r/o)

**Core Private:** `WindowSurfaceGridOnMouseLeftButtonDown` (click=DragMove, dblclick=Maximize), `WindowResizeThumbOnDragDelta` (8-direction resize + DPI), `SelectSizingMode`, `SelectCursor`

---

#### `InterTabController` : `FrameworkElement`
**File:** `Dragablz/InterTabController.cs`

Configuration for inter-tab dragging.

**DPs:** `HorizontalPopoutGrace` (8), `VerticalPopoutGrace` (8), `MoveWindowWithSolitaryTabs` (true), `InterTabClient` (DefaultInterTabClient), `Partition` (null=global)

---

#### `Trapezoid` : `ContentControl`
**File:** `Dragablz/Trapezoid.cs`

Draws trapezoid tab background via `PathGeometry`. DPs: `PenBrush`, `LongBasePenBrush`, `PenThickness`. Used by MahApps theme.

#### `DragablzIcon` : `Control`
**File:** `Dragablz/DragablzIcon.cs`

Minimal styled icon placeholder. Overrides DefaultStyleKey only.

---

### 3.2 Namespace `Dragablz` — Interfaces

**`IItemsOrganiser`** — Layout strategy: `Organise` (2 overloads), `OrganiseOnMouseDownWithin`, `OrganiseOnDragStarted/OnDrag/OnDragCompleted`, `ConstrainLocation`, `Measure`, `Sort`

**`IInterTabClient`** — `GetNewHost(IInterTabClient, object partition, TabablzControl)` → `INewTabHost<Window>`, `TabEmptiedHandler` → `TabEmptiedResponse`

**`IManualInterTabClient`** — extends `IInterTabClient` with `Add(object)`, `Remove(object)`

**`IInterLayoutClient`** — `GetNewHost(object partition, TabablzControl)` → `INewTabHost<UIElement>` (for branching within same window)

**`INewTabHost<out TElement>`** — `TElement Container`, `TabablzControl TabablzControl`

---

### 3.3 Namespace `Dragablz` — Organisers

**`StackOrganiser`** (abstract) : `IItemsOrganiser` — Horizontal/vertical linear stack base. Uses Canvas Left/Top. 200ms cubic-ease-out animated transitions via `SendToLocation`. Drag reordering: snapshots positions at start, inserts dragged item at matched location, stable sort for siblings.

**`HorizontalOrganiser`** : `StackOrganiser` — Left-to-right. Constructors: `()`, `(double itemOffset)`

**`VerticalOrganiser`** : `StackOrganiser` — Top-to-bottom.

**`CanvasOrganiser`** : `IItemsOrganiser` — Free-form. Only manages Z-order (bring-to-front on click) and constrains drag.

---

### 3.4 Namespace `Dragablz` — Position Monitors

**`PositionMonitor`** — Base observer. Event: `LocationChanged`

**`StackPositionMonitor`** (abstract) : `PositionMonitor` — Linear observer. Also `OrderChanged` event, `Sort` by coordinate.

**`HorizontalPositionMonitor`** / **`VerticalPositionMonitor`** — Concrete monitors sorting by X / Y.

---

### 3.5 Namespace `Dragablz` — Supporting Types

| Class | Role |
|-------|------|
| `DragablzDragStartedEventArgs` : DragablzItemEventArgs | Carries `DragStartedEventArgs` |
| `DragablzDragDeltaEventArgs` : DragablzItemEventArgs | Carries `DragDeltaEventArgs` + `Cancel` |
| `DragablzDragCompletedEventArgs` : RoutedEventArgs | Carries `DragablzItem` + `DragCompletedEventArgs` |
| `DragablzItemEventArgs` : RoutedEventArgs | Carries `DragablzItem` |
| `ItemActionCallbackArgs<TOwner>` | Window, Owner, DragablzItem, IsCancelled; `Cancel()` |
| `LocationChangedEventArgs` : EventArgs | `object Item`, `Point Location` |
| `OrderChangedEventArgs` : EventArgs | `object[] PreviousOrder`, `object[] NewOrder` |
| `MoveItemRequest` | Item, Context, AddLocationHint |
| `NewTabHost<TElement>` : INewTabHost<TElement> | Container + TabablzControl |
| `DefaultInterTabClient` : IInterTabClient | Creates new Window via Activator.CreateInstance; TabEmptiedHandler → CloseWindowOrLayoutBranch |
| `DefaultInterLayoutClient` : IInterLayoutClient | Creates TabablzControl, clones local DP values |
| `HeaderedItemViewModel` : INotifyPropertyChanged | Header, Content, IsSelected |
| `DragablzColors` (static) | DWM colorization brushes |
| `TabablzHeaderSizeConverter` : IMultiValueConverter | available = total - sum(siblings), clamped |
| `TabablzItemStyleSelector` : StyleSelector | Two styles based on `is TabItem` |
| `StoryboardCompletionListener` (internal) | OnComplete callback; `WhenComplete()` extension |
| `ContainerCustomisations` (internal) | Delegate props for container lifecycle injection |

### 3.6 Namespace `Dragablz` — Enums

| Enum | Values |
|------|--------|
| `AddLocationHint` | First, Last, Prior, After |
| `EmptyHeaderSizingHint` | Collapse, PreviousTab |
| `TabEmptiedResponse` | CloseWindowOrLayoutBranch, CloseLayoutBranch, DoNothing |

---

### 3.7 Namespace `Dragablz.Dockablz` — Layout Engine

#### `Layout` : `ContentControl`
**File:** `Dragablz/Dockablz/Layout.cs`

Root docking container. Hosts `TabablzControl` directly, recursively `Branch`, or floating `DragablzItemsControl` (MDI).

**Template Parts:** `PART_TopDropZone`, `PART_RightDropZone`, `PART_BottomDropZone`, `PART_LeftDropZone`, `PART_FloatDropZone`, `PART_FloatContentPresenter`

**Static Commands:** `UnfloatItemCommand`, `MaximiseFloatingItem`, `RestoreFloatingItem`, `CloseFloatingItem`, `TileFloatingItemsCommand`, `TileFloatingItemsVerticallyCommand`, `TileFloatingItemsHorizontallyCommand`

**DPs (15):** `Partition`, `InterLayoutClient`, `IsParticipatingInDrag` (r/o), `BranchTemplate`, `IsFloatDropZoneEnabled`, `FloatingItemsContainerMargin`, `FloatingItemsSource`, `FloatingItemsControlStyle`, `FloatingItemContainerStyle`, `FloatingItemContainerStyleSelector`, `FloatingItemTemplate`, `FloatingItemTemplateSelector`, `FloatingItemHeaderMemberPath`, `FloatingItemDisplayMemberPath`, `ClosingFloatingItemCallback`

**Attached Props:** `IsFloatingInLayout` (r/o), `IsTopLeftItem` (r/o), `FloatingItemState` (WindowState), `LocationSnapShot` (internal)

**Key Static Methods:** `GetLoadedInstances`, `Find(TabablzControl)` → LocationReport, `Branch(...)`, `ConsolidateBranch(...)` → bool

**Core Private:** `MonitorDropZones`, `SetupParticipatingLayouts`, `Float(...)`, `Branch(DropZoneLocation, DragablzItem)`

#### `Branch` : `Control`
**File:** `Dragablz/Dockablz/Branch.cs`

Two-pane split with proportional sizing. DPs: `Orientation`, `FirstItem`, `FirstItemLength` (GridLength Star 0.5 TwoWay), `SecondItem`, `SecondItemLength`. Method: `GetFirstProportion()`.

#### `DropZone` : `Control`
**File:** `Dragablz/Dockablz/DropZone.cs`

Edge drop-target. DPs: `Location` (DropZoneLocation), `IsOffered` (bool r/o, internal setter).

#### Accessor Classes
- **`LayoutAccessor`** — Visitor pattern: `Visit(Action<BranchAccessor>, Action<TabablzControl>, Action<object>)`, `TabablzControls()` → recursive collect
- **`BranchAccessor`** — `Visit(BranchItem, ...)`, recursive First/Second child access
- **`BranchResult`** — Result: Branch + TabablzControl
- **`LocationReport`** — Find result: TabablzControl, RootLayout, ParentBranch, IsLeaf, IsSecondLeaf
- **`LocationReportBuilder`** (internal) — Mutable builder, `MarkFound()`, `ToLocationReport()`
- **`Finder`** (internal static) — `Find(TabablzControl)` → LocationReport via LayoutAccessor.Visit across all loaded Layouts

#### Floating Item Management
- **`Tiler`** (internal static) — `Tile`, `TileHorizontally`, `TileVertically`
- **`TilerCalculator`** (internal static) — `GetCellCountPerColumn(int)` → int[] (perfect square distrib, remainder right-to-left)
- **`FloatingItemSnapShot`** (internal) — Capture/restore position+size+ZIndex+WindowState
- **`LocationSnapShot`** (internal) — Capture/restore Width+Height only (maximize/restore)
- **`FloatTransfer`** (internal) — Transient state: Content

#### Other Dockablz Types
- **`FloatRequestedEventArgs`** : DragablzItemEventArgs
- **`CouldBeHeaderedStyleSelector`** : StyleSelector — HeaderedStyle vs NonHeaderedStyle based on container type
- **`LocationReportException`** : Exception
- **`Extensions`** (Dockablz) — `Layout.Query()` → LayoutAccessor, `Visit<TContext>` fluent overloads

### 3.8 Dockablz Enums
`BranchItem` { First, Second } | `DropZoneLocation` { Top, Right, Bottom, Left, Floating }

---

### 3.9 Namespace `Dragablz.Core` (Internal)

**`Extensions`** — Tree traversal: `Containers<TContainer>`, `LogicalTreeDepthFirstTraversal`, `VisualTreeDepthFirstTraversal`, `VisualTreeAncestory`, `LogicalTreeAncestory`, `GetActualLeft`/`GetActualTop` (reflection for maximized state)

**`Native`** — P/Invoke: `GetCursorPos` → Point/POINT, `GetDC`/`GetDeviceCaps`/`ReleaseDC` (DPI), `SetWindowPlacement`, `SendMessage`, `PostMessage`, `DwmGetColorizationParameters`, `SortWindowsTopToBottom`, `ToWpf`. Structs: `POINT` (implicit→Point), `RECT`, `WINDOWPLACEMENT`, `DWMCOLORIZATIONPARAMS`

**`InstanceRegistry<T>`** — Thread-safe WeakReference<T> registry: Register, Unregister, GetAliveInstances, Cleanup, Clear

**`InterTabTransfer`** — Transfer state: BreachOrientation, DragStartWindowOffset, Item, OriginatorContainer, TransferReason, etc. Nested: `InterTabTransferReason { Breach, Reentry }`

**`TabHeaderDragStartInformation`** — Offsets at drag start

**`CollectionTeaser`** — Duck-type IList/ICollection<T> via reflection: TryCreate, Add, Remove

**`FuncComparer<T>`** : `IComparer<T>` — Wraps `Func<T,T,int>` as comparer

**`MultiComparer<T>`** : `IComparer<T>` — Chained comparer: Ascending/Descending + ThenAscending/ThenDescending

**Core Enums:** `HitTest` (HT*), `SystemCommand` (SC_*), `WindowMessage` (~130 WM_*), `WindowSizingMessage` (WMSZ_*)

---

### 3.10 Namespace `Dragablz.Converters`

| Class | Implements | Logic |
|-------|-----------|-------|
| `BooleanAndToVisibilityConverter` | IMultiValueConverter | AND all bools → Visible/Collapsed |
| `EqualityToBooleanConverter` | IValueConverter | Equals(value, parameter) → bool |
| `EqualityToVisibilityConverter` | IValueConverter | Equals(value, parameter) → Visible/Collapsed |
| `ShowDefaultCloseButtonConverter` | IMultiValueConverter | Visible if ShowDefaultCloseButton AND LogicalIndex >= FixedHeaderCount |

---

### 3.11 Namespace `Dragablz.Themes`

#### `Ripple` : `ContentControl`
Material Design ink ripple. Visual states: Normal, MousePressed, MouseOut. Global tracking of pressed instances for cross-window mouse-up/move. DPs: Feedback, RippleSize (r/o), RippleX (r/o), RippleY (r/o), RecognizesAccessKey.

#### `RippleAssist` (static)
Attached DPs (all inherit): ClipToBounds (true), IsCentered (false), RippleSizeMultiplier (1.0)

#### `SystemCommandIcon` : `Control`
Renders window command icon. DP: SystemCommandType (CloseWindow, MaximizeWindow, MinimzeWindow, RestoreWindow).

#### `MaterialDesignAssist` (static)
Attached DP: IndicatorBrush (Brush) — underline indicator color.

#### `MaterialDesignHeaderedAssist` (static) — NEW
Attached DPs (all inherit): HeaderBackground (Brush), HeaderForeground (Brush), IsIndicatorAnimated (bool, true), IndicatorThickness (double, 2.0)

#### `UnderlineIndicator` : `Control` — NEW
Animated underline indicator. DPs: IsActivated (bool), IndicatorBrush (Brush), IndicatorThickness (double, 2.0), IndicatorCornerRadius (double, 0). VSM: Activated (300ms ScaleX 0→1 SineEase EaseOut), Deactivated (200ms ScaleX 1→0 SineEase EaseIn).

#### `BrushToRadialGradientBrushConverter` : `IValueConverter`
SolidColorBrush → RadialGradientBrush (fade to transparent, centered, 0.39 opacity)

---

### 3.12 Namespace `Dragablz.Referenceless` (Internal)

| Class | Purpose |
|-------|---------|
| `ICancelable` : IDisposable | + IsDisposed property |
| `Disposable` (static) | Empty → DefaultDisposable Instance; Create(Action) → AnonymousDisposable |
| `DefaultDisposable` : IDisposable | Singleton no-op |
| `AnonymousDisposable` : ICancelable | Wraps Action, invokes once via Interlocked.Exchange |
| `SerialDisposable` : ICancelable | Atomically replaceable disposable (disposes previous on swap) |

---

## 4. Subsystem Architecture

### 4.1 Tab Drag Pipeline

```
User drags thumb
    ↓
DragablzItem.ThumbOnDragStarted()         ← records MouseAtDragStart, fires DragStarted
    ↓
DragablzItemsControl.ItemDragStarted()    ← notifies ItemsOrganiser
    ↓
TabablzControl.ItemDragStarted()          ← records TabHeaderDragStartInformation
    ↓
DragablzItem.ThumbOnDragDelta()           ← fires PreviewDragDelta (Tunnel) + DragDelta (Bubble)
    │
    ├→ DragablzItemsControl.ItemDragDelta()
    │    ├ ItemsOrganiser.ConstrainLocation() → clamped
    │    ├ Set DragablzItem.X/Y
    │    └ ItemsOrganiser.OrganiseOnDrag() → reflow siblings
    │
    └→ TabablzControl: PreviewItemDragDelta() → MonitorBreach() / MonitorReentry()
         └ if breached: new window via InterTabClient
           └ if reentered: ReceiveDrag(InterTabTransfer)
    ↓
DragablzItem.ThumbOnDragCompleted()       ← fires DragCompleted
    ↓
DragablzItemsControl.ItemDragCompleted()  ← finalize, reset flags
```

### 4.2 Dockablz Layout Tree

```
Layout (ContentControl)
├── Content = TabablzControl          ← leaf
│   OR
├── Content = Branch (Control)
│   ├── FirstItem  → TabablzControl | Branch  ← recursive
│   └── SecondItem → TabablzControl | Branch
└── _floatingItems (DragablzItemsControl)  ← MDI floating layer
```

Drop detection: `Layout.DragStarted` → `SetupParticipatingLayouts()` → `PreviewDragDelta` → `MonitorDropZones()` → `DragCompleted` → `Float()` or `Branch()`.

Consolidation: on tab removal, `Layout.ConsolidateBranch()` replaces Branch with surviving child.

### 4.3 ItemsOrganiser Strategy Pattern

```
IItemsOrganiser
├── StackOrganiser (abstract) — Canvas.Left/Top, 200ms animation
│   ├── HorizontalOrganiser — left-to-right
│   └── VerticalOrganiser   — top-to-bottom
└── CanvasOrganiser — free-form, Z-order only
```

### 4.4 PositionMonitor Observer Pattern

```
PositionMonitor
└── StackPositionMonitor (abstract)
    ├── HorizontalPositionMonitor — by X
    └── VerticalPositionMonitor   — by Y
```

---

## 5. XAML Theme Resources

### Generic.xaml (~1909 lines) — default theme
All implicit default styles: TabablzControl, DragablzItem, HeaderedDragablzItem, DragablzWindow, DragablzItemsControl, Layout, Branch, DropZone, Trapezoid, SystemCommandIcon + keyed: `StandardDragablzTabItemStyle`, `TrapezoidDragableTabItemStyle`, `ToolDragablzItemStyle`, `FloatingDragablzItemStyle`, `TabablzDragablzItemsControlStyle`, `FloatingDragablzItemsControlStyle`.

### MaterialDesign.xaml (~1100 lines) — Material Design theme
**14 styles:** `MaterialDesignTabablzControlStyle`, `MaterialDesignElevatedTabablzControlStyle` (NEW), `MaterialDesignAlternateTabablzControlStyle`, `MaterialDesignDragableTabItemStyle`, `MaterialDesignAnimatedDragableTabItemStyle` (NEW), `MaterialDesignDragableTabItemVerticalStyle`, `MaterialDesignDenseDragableTabItemStyle` (NEW), `MaterialDesignAlternateDragableTabItemStyle`, `MaterialDesignAlternateDragableTabItemVerticalStyle`, `MaterialDesignToolDragablzItemStyle`, `MaterialDesignDragablzWindowStyle` (NEW), `MaterialDesignDropZoneStyle` (NEW), + implicit UnderlineIndicator.

### MahApps.xaml (~442 lines) — MahApps.Metro theme
Trapezoid tabs, MahApps.Accent brushes, `AdjacentHeaderItemOffset="-12"` overlap.

### Dockablz.xaml — empty placeholder

---

## 6. Key Interaction Flows

### 6.1 Tab Tear-Off → New Window
1. Drag `PART_Thumb` → `DragStarted` → `DragDelta`
2. `MonitorBreach()` detects exit from header bounds (with grace tolerance)
3. Creates `InterTabTransfer` (breach), calls `InterTabClient.GetNewHost()`, creates new Window
4. New TabablzControl calls `ReceiveDrag(InterTabTransfer)`, original tab removed from source

### 6.2 Dockablz Branch Drop
1. Tab dragged within Layout bounds → `MonitorDropZones()` → `InputHitTest`
2. Matching DropZone `IsOffered = true`
3. On `DragCompleted`: edge → `Layout.Branch(...)` creates new Branch; Floating → `Layout.Float(...)` moves to floating layer

### 6.3 Tab Close → Consolidation
1. `CloseItemCommand` → `CloseItem()` → `RemoveFromSource()` → `RemoveItem()`
2. Last tab: `TabEmptiedResponse` → close window / consolidate branch / do nothing
3. `ConsolidateOrphanedItems`: move remaining tabs to another host

---

## 7. File Path Index

### Root Namespace (`Dragablz/`)
| File | Type |
|------|------|
| `CanvasOrganiser.cs` | CanvasOrganiser : IItemsOrganiser |
| `ContainerCustomisations.cs` | ContainerCustomisations (internal) |
| `DefaultInterLayoutClient.cs` | DefaultInterLayoutClient : IInterLayoutClient |
| `DefaultInterTabClient.cs` | DefaultInterTabClient : IInterTabClient |
| `DragablzColors.cs` | DragablzColors (static) |
| `DragablzDragCompletedEventArgs.cs` | DragablzDragCompletedEventArgs : RoutedEventArgs |
| `DragablzDragDeltaEventArgs.cs` | DragablzDragDeltaEventArgs : DragablzItemEventArgs |
| `DragablzDragStartedEventArgs.cs` | DragablzDragStartedEventArgs : DragablzItemEventArgs |
| `DragablzIcon.cs` | DragablzIcon : Control |
| `DragablzItem.cs` | DragablzItem : ContentControl (+ SizeGrip enum) |
| `DragablzItemEventArgs.cs` | DragablzItemEventArgs : RoutedEventArgs |
| `DragablzItemsControl.cs` | DragablzItemsControl : ItemsControl |
| `DragablzWindow.cs` | DragablzWindow : Window |
| `EmptyHeaderSizingHint.cs` | EmptyHeaderSizingHint enum |
| `HeaderedDragablzItem.cs` | HeaderedDragablzItem : DragablzItem |
| `HeaderedItemViewModel.cs` | HeaderedItemViewModel : INotifyPropertyChanged |
| `HorizontalOrganiser.cs` | HorizontalOrganiser : StackOrganiser |
| `HorizontalPositionMonitor.cs` | HorizontalPositionMonitor : StackPositionMonitor |
| `IInterLayoutClient.cs` | IInterLayoutClient interface |
| `IInterTabClient.cs` | IInterTabClient + IManualInterTabClient |
| `INewTabHost.cs` | INewTabHost<TElement> interface |
| `IItemsOrganiser.cs` | IItemsOrganiser interface |
| `InterTabController.cs` | InterTabController : FrameworkElement |
| `ItemActionCallbackArgs.cs` | ItemActionCallbackArgs<TOwner> |
| `LocationChangedEventArgs.cs` | LocationChangedEventArgs : EventArgs |
| `LocationHint.cs` | AddLocationHint enum |
| `MoveItemRequest.cs` | MoveItemRequest |
| `NewTabHost.cs` | NewTabHost<TElement> : INewTabHost<TElement> |
| `OrderChangedEventArgs.cs` | OrderChangedEventArgs : EventArgs |
| `PositionMonitor.cs` | PositionMonitor |
| `StackOrganiser.cs` | StackOrganiser (abstract) : IItemsOrganiser |
| `StackPositionMonitor.cs` | StackPositionMonitor (abstract) : PositionMonitor |
| `StoryboardCompletionListener.cs` | StoryboardCompletionListener (internal) |
| `TabablzControl.cs` | TabablzControl : TabControl |
| `TabablzHeaderSizeConverter.cs` | TabablzHeaderSizeConverter : IMultiValueConverter |
| `TabablzItemStyleSelector.cs` | TabablzItemStyleSelector : StyleSelector |
| `TabEmptiedResponse.cs` | TabEmptiedResponse enum |
| `Trapezoid.cs` | Trapezoid : ContentControl |
| `VerticalOrganiser.cs` | VerticalOrganiser : StackOrganiser |
| `VerticalPositionMonitor.cs` | VerticalPositionMonitor : StackPositionMonitor |

### Core (`Dragablz/Core/`)
| File | Type |
|------|------|
| `CollectionTeaser.cs` | CollectionTeaser (internal) |
| `Extensions.cs` | Extensions (internal static) |
| `FuncComparer.cs` | FuncComparer<T> : IComparer<T> (internal) |
| `HitTest.cs` | HitTest enum (internal) |
| `InstanceRegistry.cs` | InstanceRegistry<T> (internal) |
| `InterTabTransfer.cs` | InterTabTransfer + InterTabTransferReason enum |
| `MultiComparer.cs` | MultiComparer<T> : IComparer<T> (internal) |
| `Native.cs` | Native + POINT/RECT/WINDOWPLACEMENT/DWMCOLORIZATIONPARAMS |
| `SystemCommand.cs` | SystemCommand enum (internal) |
| `TabHeaderDragStartInformation.cs` | TabHeaderDragStartInformation (internal) |
| `WindowMessage.cs` | WindowMessage enum (internal) + WindowSizingMessage (public) |

### Dockablz (`Dragablz/Dockablz/`)
| File | Type |
|------|------|
| `Branch.cs` | Branch : Control |
| `BranchAccessor.cs` | BranchAccessor |
| `BranchItem.cs` | BranchItem enum |
| `BranchResult.cs` | BranchResult |
| `CouldBeHeaderedStyleSelector.cs` | CouldBeHeaderedStyleSelector : StyleSelector |
| `DropZone.cs` | DropZone : Control |
| `DropZoneLocation.cs` | DropZoneLocation enum |
| `Extensions.cs` | Extensions (static) — Query/Visit |
| `Finder.cs` | Finder (internal static) |
| `FloatRequestedEvent.cs` | FloatRequestedEventArgs + FloatRequestedEventHandler |
| `FloatTransfer.cs` | FloatTransfer (internal) |
| `FloatingItemSnapShot.cs` | FloatingItemSnapShot (internal) |
| `Layout.cs` | Layout : ContentControl |
| `LayoutAccessor.cs` | LayoutAccessor |
| `LocationReport.cs` | LocationReport |
| `LocationReportBuilder.cs` | LocationReportBuilder (internal) |
| `LocationReportException.cs` | LocationReportException : Exception |
| `LocationSnapShot.cs` | LocationSnapShot (internal) |
| `Tiler.cs` | Tiler (internal static) |
| `TilerCalculator.cs` | TilerCalculator (internal static) |

### Converters (`Dragablz/Converters/`)
| File | Type |
|------|------|
| `BooleanAndToVisibilityConverter.cs` | BooleanAndToVisibilityConverter : IMultiValueConverter |
| `EqualityToBooleanConverter.cs` | EqualityToBooleanConverter : IValueConverter |
| `EqualityToVisibilityConverter.cs` | EqualityToVisibilityConverter : IValueConverter |
| `ShowDefaultCloseButtonConverter.cs` | ShowDefaultCloseButtonConverter : IMultiValueConverter |

### Themes (`Dragablz/Themes/`)
| File | Type |
|------|------|
| `BrushToRadialGradientBrushConverter.cs` | BrushToRadialGradientBrushConverter : IValueConverter |
| `MaterialDesignAssist.cs` | MaterialDesignAssist (static) |
| `MaterialDesignHeaderedAssist.cs` | MaterialDesignHeaderedAssist (static) |
| `Ripple.cs` | Ripple : ContentControl |
| `RippleAssist.cs` | RippleAssist (static) |
| `SystemCommandIcon.cs` | SystemCommandIcon : Control (+ SystemCommandType enum) |
| `UnderlineIndicator.cs` | UnderlineIndicator : Control |
| `Generic.xaml` | Default theme (~1909 lines) |
| `MaterialDesign.xaml` | Material Design theme (~1100 lines) |
| `MahApps.xaml` | MahApps.Metro theme (~442 lines) |
| `Dockablz.xaml` | Empty placeholder |

### Referenceless (`Dragablz/Referenceless/`)
| File | Type |
|------|------|
| `ICancelable.cs` | ICancelable : IDisposable (internal) |
| `Disposable.cs` | Disposable (internal static) |
| `DefaultDisposable.cs` | DefaultDisposable : IDisposable (internal) |
| `AnonymousDisposable.cs` | AnonymousDisposable : ICancelable (internal) |
| `SerialDisposable.cs` | SerialDisposable : ICancelable (internal) |
