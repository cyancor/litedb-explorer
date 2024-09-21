using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using LiteDB.Explorer.ViewModels;
using TextMateSharp.Grammars;

namespace LiteDB.Explorer.Views;

public partial class DatabaseEntryWindow : Window
{
    private readonly TextEditor? _textEditor;

    public DatabaseEntryWindow(DatabaseEntryViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        _textEditor = this.FindControl<TextEditor>("TextEditor");

        if (_textEditor != null)
        {
            var registryOptions = new RegistryOptions(ThemeName.Dark);
            var textMateInstallation = _textEditor.InstallTextMate(registryOptions);

            textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId("json"));
        }

        LoadDocument();
    }

    private async void LoadDocument()
    {
        if (_textEditor != null)
        {
            _textEditor.Text = await (DataContext as DatabaseEntryViewModel)!.LoadDocument();
        }
    }
}
