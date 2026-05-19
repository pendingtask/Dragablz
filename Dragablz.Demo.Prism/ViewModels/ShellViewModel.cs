using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Dragablz;

using Prism.Commands;
using Prism.Mvvm;

namespace Dragablz.Demo.Prism.ViewModels;

public class ShellViewModel : BindableBase
{
    private int _tabCounter;

    private bool _isMvvmMode = true;
    private bool _isCallbackMode;
    private bool _isMixedMode;
    private bool _showCloseButton = true;
    private bool _showAddButton = true;
    private int _fixedHeaderCount;
    private AddLocationHint _selectedAddLocationHint = AddLocationHint.Last;
    private string _lastActionLog = "Ready. Switch modes and interact with the tabs.";

    public ShellViewModel()
    {
        Tabs = new ObservableCollection<TabContentViewModel>();

        CloseTabCommand = new DelegateCommand<TabContentViewModel>(ExecuteCloseTab);
        AddTabCommand = new DelegateCommand(ExecuteAddTab);
        AddPredefinedTabsCommand = new DelegateCommand(ExecuteAddPredefinedTabs);
        SwitchModeCommand = new DelegateCommand<string>(ExecuteSwitchMode);
        ClearLogCommand = new DelegateCommand(() => LastActionLog = string.Empty);
        RefreshEffectiveProperties();

        SeedTabs();
    }

    public ObservableCollection<TabContentViewModel> Tabs { get; }

    // ── Mode properties ──────────────────────────────────────────

    public bool IsMvvmMode
    {
        get => _isMvvmMode;
        set
        {
            if (SetProperty(ref _isMvvmMode, value) && value)
            {
                IsCallbackMode = false;
                IsMixedMode = false;
                RefreshEffectiveProperties();
            }
        }
    }

    public bool IsCallbackMode
    {
        get => _isCallbackMode;
        set
        {
            if (SetProperty(ref _isCallbackMode, value) && value)
            {
                IsMvvmMode = false;
                IsMixedMode = false;
                RefreshEffectiveProperties();
            }
        }
    }

    public bool IsMixedMode
    {
        get => _isMixedMode;
        set
        {
            if (SetProperty(ref _isMixedMode, value) && value)
            {
                IsMvvmMode = false;
                IsCallbackMode = false;
                RefreshEffectiveProperties();
            }
        }
    }

    // ── Effective bindings (switched by mode) ────────────────────

    private ICommand? _effectiveClosingItemCommand;
    public ICommand? EffectiveClosingItemCommand
    {
        get => _effectiveClosingItemCommand;
        set => SetProperty(ref _effectiveClosingItemCommand, value);
    }

    private ICommand? _effectiveAddingItemCommand;
    public ICommand? EffectiveAddingItemCommand
    {
        get => _effectiveAddingItemCommand;
        set => SetProperty(ref _effectiveAddingItemCommand, value);
    }

    private ItemActionCallback? _effectiveClosingItemCallback;
    public ItemActionCallback? EffectiveClosingItemCallback
    {
        get => _effectiveClosingItemCallback;
        set => SetProperty(ref _effectiveClosingItemCallback, value);
    }

    private Func<object>? _effectiveNewItemFactory;
    public Func<object>? EffectiveNewItemFactory
    {
        get => _effectiveNewItemFactory;
        set => SetProperty(ref _effectiveNewItemFactory, value);
    }

    // ── Commands ─────────────────────────────────────────────────

    public DelegateCommand<TabContentViewModel> CloseTabCommand { get; }
    public DelegateCommand AddTabCommand { get; }
    public DelegateCommand AddPredefinedTabsCommand { get; }
    public DelegateCommand<string> SwitchModeCommand { get; }
    public DelegateCommand ClearLogCommand { get; }

    // ── Options ──────────────────────────────────────────────────

    public bool ShowCloseButton
    {
        get => _showCloseButton;
        set => SetProperty(ref _showCloseButton, value);
    }

    public bool ShowAddButton
    {
        get => _showAddButton;
        set => SetProperty(ref _showAddButton, value);
    }

    public int FixedHeaderCount
    {
        get => _fixedHeaderCount;
        set => SetProperty(ref _fixedHeaderCount, value);
    }

    public AddLocationHint SelectedAddLocationHint
    {
        get => _selectedAddLocationHint;
        set => SetProperty(ref _selectedAddLocationHint, value);
    }

    public string LastActionLog
    {
        get => _lastActionLog;
        set => SetProperty(ref _lastActionLog, value);
    }

    public Array AddLocationHintValues { get; } = Enum.GetValues(typeof(AddLocationHint));

    // ── Command handlers ─────────────────────────────────────────

    private void ExecuteCloseTab(TabContentViewModel tab)
    {
        Tabs.Remove(tab);
        Log($"MVVM Command: Closed '{tab.Header}'");
    }

    private void ExecuteAddTab()
    {
        var tab = new TabContentViewModel();
        Tabs.Add(tab);
        Log($"MVVM Command: Added '{tab.Header}'");
    }

    private void ExecuteAddPredefinedTabs()
    {
        var t1 = new TabContentViewModel("Home", "Welcome to the Dragablz Prism Demo!\n\nThis demonstrates MVVM ICommand support alongside traditional callback patterns.");
        var t2 = new TabContentViewModel("Settings", "Configuration options for the demo.");
        Tabs.Add(t1);
        Tabs.Add(t2);
        Log("Added predefined tabs");
    }

    private void ExecuteSwitchMode(string mode)
    {
        switch (mode)
        {
            case "MVVM":
                IsMvvmMode = true;
                break;
            case "Callback":
                IsCallbackMode = true;
                break;
            case "Mixed":
                IsMixedMode = true;
                break;
        }
    }

    // ── Callback / Factory handlers ──────────────────────────────

    private void OnClosingItemCallback(ItemActionCallbackArgs<TabablzControl> args)
    {
        var tab = args.DragablzItem?.Content as TabContentViewModel;
        var result = MessageBox.Show(
            $"Close tab '{tab?.Header}'?\n\n(Callback mode — you can cancel this)",
            "Confirm Close",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
        {
            args.Cancel();
            Log($"Callback: Close CANCELLED for '{tab?.Header}'");
        }
        else
        {
            Log($"Callback: Close confirmed for '{tab?.Header}'");
        }
    }

    private object OnNewItemFactory()
    {
        var tab = new TabContentViewModel();
        Log($"Factory: Created '{tab.Header}'");
        return tab;
    }

    // ── Helpers ──────────────────────────────────────────────────

    private void SeedTabs()
    {
        Tabs.Add(new TabContentViewModel("Home", "Welcome to Dragablz + Prism Demo!\n\nUse the panel on the left to switch modes and configure options."));
        Tabs.Add(new TabContentViewModel("MVVM Info", "MVVM Mode: Close and Add are handled by ICommand (DelegateCommand) bindings.\n\nThe ViewModel's command receives the tab content as a parameter and removes/adds it from the ObservableCollection."));
        Tabs.Add(new TabContentViewModel("Callback Info", "Callback Mode: Close uses ClosingItemCallback (shows a confirmation dialog).\n\nAdd uses NewItemFactory (Func<object>).\n\nThese are the traditional code-behind patterns."));
        Tabs.Add(new TabContentViewModel("Mixed Info", "Mixed Mode: BOTH Command AND Callback/Factory are active.\n\nPriority: Command executes FIRST, then Callback/Factory.\n\nFor close: if the Command removes the item from the collection, the Callback is skipped (item already gone).\n\nFor add: Command fires, then Factory creates an additional item."));
    }

    private void RefreshEffectiveProperties()
    {
        EffectiveClosingItemCommand = (IsMvvmMode || IsMixedMode) ? CloseTabCommand : null;
        EffectiveAddingItemCommand = (IsMvvmMode || IsMixedMode) ? AddTabCommand : null;
        EffectiveClosingItemCallback = (IsCallbackMode || IsMixedMode) ? OnClosingItemCallback : null;
        EffectiveNewItemFactory = (IsCallbackMode || IsMixedMode) ? OnNewItemFactory : null;

        var mode = IsMvvmMode ? "MVVM" : IsCallbackMode ? "Callback" : "Mixed";
        Log($"Switched to {mode} mode");
    }

    private void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        LastActionLog = $"[{timestamp}] {message}\n{LastActionLog}";
    }
}
