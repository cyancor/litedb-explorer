using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using LiteDB.Async;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace LiteDB.Explorer.ViewModels;

public partial class DatabaseEntryViewModel : ReactiveObject
{
    private readonly LiteDatabaseAsync _databaseInstance;
    private readonly string _collection;
    private readonly string _documentId;
    [Notify] private string? _document;

    public DatabaseEntryViewModel(LiteDatabaseAsync databaseInstance, string collection, string documentId)
    {
        _databaseInstance = databaseInstance;
        _collection = collection;
        _documentId = documentId;
    }

    public async Task<string> LoadDocument()
    {
        try
        {
            var document = await _databaseInstance
                .GetCollection(_collection)
                .FindOneAsync(document => document["_id"].AsString == _documentId || document["_id"].AsObjectId == new ObjectId(_documentId));

            if (document != null)
            {
                return LiteDB.JsonSerializer.Serialize(document, true);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
        }

        return "";
    }
}
