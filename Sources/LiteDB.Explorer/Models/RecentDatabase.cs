namespace LiteDB.Explorer.Models;

public class RecentDatabase(string name, string path)
{
    public string Name { get; set; } = name;
    public string Path { get; set; } = path;
}
