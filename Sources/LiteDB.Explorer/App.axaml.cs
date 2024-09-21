using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LiteDB.Async;
using LiteDB.Explorer.SuspensionDriver;
using LiteDB.Explorer.ViewModels;
using LiteDB.Explorer.Views;
using ReactiveUI;

namespace LiteDB.Explorer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var suspension = new AutoSuspendHelper(ApplicationLifetime);
            RxApp.SuspensionHost.CreateNewAppState = () => new StartViewModel();
            RxApp.SuspensionHost.SetupDefaultSuspendResume(new NewtonsoftJsonSuspensionDriver("appstate.json"));
            suspension.OnFrameworkInitializationCompleted();

            // Load the saved view model state.
            var state = RxApp.SuspensionHost.GetAppState<StartViewModel>();
            base.OnFrameworkInitializationCompleted();

            desktop.MainWindow = new StartWindow
            {
                DataContext = state
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}
