using Prism.Mvvm;

namespace Dragablz.Demo.Prism.ViewModels;

public class TabContentViewModel : BindableBase
{
    private static int _counter;

    private string _header;
    private string _content;
    private bool _isSelected;

    public TabContentViewModel(string? header = null, string? content = null)
    {
        var id = Interlocked.Increment(ref _counter);
        _header = header ?? $"Tab {id}";
        _content = content ?? $"Content for tab {id}.\n\nThis tab was created at {DateTime.Now:T}.";
    }

    public string Header
    {
        get => _header;
        set => SetProperty(ref _header, value);
    }

    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}
