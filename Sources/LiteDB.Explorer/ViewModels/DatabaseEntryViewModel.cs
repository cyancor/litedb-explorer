using System;
using System.Threading.Tasks;
using LiteDB.Async;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace LiteDB.Explorer.ViewModels;

public partial class DatabaseEntryViewModel(LiteDatabaseAsync databaseInstance, string collection, string documentId) : ReactiveObject
{
    [Notify] private string? _document;

    public async Task<string> LoadDocument()
    {
        try
        {
            var document = await databaseInstance
                .GetCollection(collection)
                .FindOneAsync(document => document["_id"].AsString == documentId || document["_id"].AsObjectId == new ObjectId(documentId));

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
