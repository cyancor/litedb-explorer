using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LiteDB.Explorer.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace LiteDB.Explorer.ViewModels;

[DataContract]
public partial class StartViewModel : ReactiveObject
{
    private ObservableCollection<RecentDatabase> _recents = [];

    [Reactive]
    private RecentDatabase? _selectedDatabase;

    [DataMember]
    public ObservableCollection<RecentDatabase> Recents
    {
        get => _recents;
        set => this.RaiseAndSetIfChanged(ref _recents, value);
    }
}
