using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LiteDB.Async;
using LiteDB.Explorer.Models;
using LiteDB.Explorer.ViewModels;

namespace LiteDB.Explorer.Views;

public partial class StartWindow : Window
{
    public StartWindow()
    {
        InitializeComponent();
    }

    public async void OpenNew(object? sender, RoutedEventArgs routedEventArgs)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel == null)
        {
            return;
        }

        var file = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Database",
            AllowMultiple = false
        });

        if (file.Any())
        {
            var filePath = file[0].Path.AbsolutePath;
            var name = Path.GetFileNameWithoutExtension(filePath);

            if ((DataContext as StartViewModel)?.Recents.FirstOrDefault(recent => recent.Path == filePath) == null)
            {
                (DataContext as StartViewModel)?.Recents.Add(new RecentDatabase(name: name, path: filePath));
            }

            OpenWindow(filePath);
        }
    }

    private void OpenWindow(string filePath)
    {

        var connectionString = new ConnectionString
        {
            Filename = filePath
        };

        var mainWindow = new MainWindow(new MainWindowViewModel(new LiteDatabaseAsync(connectionString)));
        mainWindow.Show();

        Close();
    }

    private void OnRecentEntryClicked(object? sender, TappedEventArgs e)
    {
        var recentDatabase = (DataContext as StartViewModel)?.SelectedDatabase;
        if (recentDatabase != null)
        {
            OpenWindow(recentDatabase.Path);
        }
    }
}
