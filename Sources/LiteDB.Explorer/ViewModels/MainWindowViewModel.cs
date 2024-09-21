using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using LiteDB.Async;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace LiteDB.Explorer.ViewModels;

public record EntryQuickView(string Id, Dictionary<string, object> Properties);

public partial class MainWindowViewModel : ReactiveObject
{
    public readonly LiteDatabaseAsync DatabaseInstance;

    [Reactive] private string _query = "SELECT * FROM";
    [Reactive] private string? _selectedCollection;
    [Reactive] private Dictionary<string, string>? _selectedDocument;
    [Reactive] private List<string> _collectionNames = [];
    [Reactive] private ObservableCollection<Dictionary<string, string>> _entries = [];

    public MainWindowViewModel(LiteDatabaseAsync database)
    {
        DatabaseInstance = database;
        _ = SetCollectionNames();
    }

    private async Task SetCollectionNames()
    {
        CollectionNames = (await DatabaseInstance.GetCollectionNamesAsync()).ToList();
        SelectedCollection = CollectionNames.FirstOrDefault();
    }

    private string GetValue(BsonValue? value)
    {
        // if (value?.IsArray == true)
        // {
        //     return $"[{string.Join(", ", value.AsArray.RawValue)}]";
        // }

        return value?.RawValue?.ToString() ?? "";
    }

    public void GetEntries()
    {
        Entries = [];

        var result = DatabaseInstance.UnderlyingDatabase.Execute(_query);

        foreach (var rawValue in result.Current.AsDocument)
        {
            foreach (var document in rawValue.Value.AsArray)
            {
                Dictionary<string, string> properties = new();

                foreach (var property in document.AsDocument.RawValue)
                {
                    properties[property.Key] = GetValue(property.Value);
                }

                Entries.Add(properties);
            }
        }
    }
}
