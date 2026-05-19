using System.Windows;
using Dragablz.Demo.Prism.ViewModels;

namespace Dragablz.Demo.Prism;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var shell = new ShellWindow
        {
            DataContext = new ShellViewModel()
        };
        shell.Show();
    }
}
