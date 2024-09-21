using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using LiteDB.Explorer.ViewModels;
using TextMateSharp.Grammars;

namespace LiteDB.Explorer.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();

        var textEditor = this.FindControl<TextEditor>("TextEditor");

        if (textEditor != null)
        {
            var registryOptions = new RegistryOptions(ThemeName.Dark);
            var textMateInstallation = textEditor.InstallTextMate(registryOptions);

            textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId("sql"));
        }
    }

    protected override void OnClosing(WindowClosingEventArgs args)
    {
        base.OnClosing(args);

        (DataContext as MainWindowViewModel)?.DatabaseInstance.Dispose();
    }

    private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        var dataGrid = sender as DataGrid;

        // Cancel default auto-generated columns
        e.Cancel = true;
        dataGrid?.Columns.Clear();

        // Get the first entry to get the keys for dynamic column generation
        var firstEntry = (DataContext as MainWindowViewModel)?.Entries.FirstOrDefault();
        if (firstEntry == null)
        {
            return;
        }

        // Dynamically add columns for each key in the dictionary
        foreach (var key in firstEntry)
        {
            dataGrid?.Columns.Add(new DataGridTextColumn
            {
                Header = key.Key,
                Binding = new Binding($"[{key.Key}]"),
                Width = new DataGridLength(150),
                MinWidth = 50
            });
        }
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs eventArgs)
    {
        var collection = eventArgs.AddedItems[0] as string;
        (DataContext as MainWindowViewModel)!.Query = $"SELECT * FROM {collection} LIMIT 50";
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as MainWindowViewModel)?.GetEntries();
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        var viewModel = DataContext as MainWindowViewModel;
        var id = viewModel?.SelectedDocument?["_id"];

        if (id != null && viewModel is { SelectedCollection: not null })
        {
            var detailedWindow = new DatabaseEntryWindow(new DatabaseEntryViewModel(viewModel.DatabaseInstance, viewModel.SelectedCollection, id));
            detailedWindow.Show();
        }
    }
}
