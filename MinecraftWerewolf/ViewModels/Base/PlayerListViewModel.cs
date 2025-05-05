using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.Base;

public partial class PlayerListViewModel : ObservableObject
{
    public PlayerListViewModel(ObservableCollection<GamePlayer> players, GameCard? source)
    {
        Players = new ObservableCollection<InternalGamePlayer>(players.Select(p => 
            new InternalGamePlayer(this, p)));
        SourceCard = source;
    }

    [ObservableProperty] private ObservableCollection<InternalGamePlayer> _players;
    [ObservableProperty] private bool _allowDeadPlayers;
    [ObservableProperty] private GameCard? _sourceCard;
    [ObservableProperty] private IRelayCommand<GamePlayer> _playerSelectCommand;
}

public partial class InternalGamePlayer : ObservableObject
{
    [ObservableProperty] private GamePlayer _base;
    
    private readonly PlayerListViewModel vm;

    public InternalGamePlayer(PlayerListViewModel vm, GamePlayer player)
    {
        this.vm = vm;
        Base = player;
    }

    public bool CanBeTargeted
    {
        get
        {
            if (!vm.AllowDeadPlayers && !Base.IsAlive)
                return false;
            if (vm.SourceCard == null)
                return true;
            if (vm.SourceCard == Base.Card)
                return false;

            return true;
        }
    }

    public IRelayCommand<GamePlayer> PlayerSelectCommand => vm.PlayerSelectCommand;
}