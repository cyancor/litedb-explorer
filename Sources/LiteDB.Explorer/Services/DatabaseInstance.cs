using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB.Async;

namespace LiteDB.Explorer.Services;

public partial class DatabaseInstance
{
    private readonly LiteDatabaseAsync _currentDatabase;

    public DatabaseInstance(string path, ConnectionType connectionType = ConnectionType.Shared, string? password = null)
    {
        var connectionString = new ConnectionString
        {
            Filename = path,
            Connection = connectionType
        };

        if (!string.IsNullOrEmpty(password))
        {
            connectionString.Password = password;
        }

        _currentDatabase = new LiteDatabaseAsync(connectionString);
    }

    public async Task<IEnumerable<string>> GetCollectionNames()
    {
        return await _currentDatabase.GetCollectionNamesAsync();
    }

    // public async IAsyncEnumerable<string> GetCollectionEntries(string name)
    // {
    //     var collection = _currentDatabase.GetCollection(name);
    // }
}
