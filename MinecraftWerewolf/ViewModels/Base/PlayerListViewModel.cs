using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.Base;

public partial class PlayerListViewModel : ObservableObject
{
    public PlayerListViewModel(ObservableCollection<GamePlayer> players, GameCard? source,
        Predicate<GamePlayer> filter)
    {
        Players = new ObservableCollection<InternalGamePlayer>(players.Select(p =>
            new InternalGamePlayer(this, p)));
        SourceCard = source;
        Filter = filter;
    }

    [ObservableProperty] private ObservableCollection<InternalGamePlayer> _players;
    [ObservableProperty] private Predicate<GamePlayer> _filter;
    [ObservableProperty] private bool _allowSelf = false;
    [ObservableProperty] private GameCard? _sourceCard;
    [ObservableProperty] private IRelayCommand<GamePlayer> _playerSelectCommand;
}

public static class PlayerListType
{
    /*
     case PlayerListType.OnlyAlive when !Base.IsAlive:
     case PlayerListType.OnlyDead when Base.IsAlive:
     case PlayerListType.OnlyPartialAlive when Base.ShouldDie:
     case PlayerListType.OnlyDying when !Base.ShouldDie:
     */
    
    public static readonly Predicate<GamePlayer> All = _ => true;
    public static readonly Predicate<GamePlayer> OnlyAlive = p => p.IsAlive;
    public static readonly Predicate<GamePlayer> OnlyPartialAlive = p => !p.ShouldDie;
    public static readonly Predicate<GamePlayer> OnlyDying = p => p.ShouldDie;
}

public partial class InternalGamePlayer : ObservableObject
{
    [ObservableProperty] private GamePlayer _base;

    private readonly PlayerListViewModel vm;

    public InternalGamePlayer(PlayerListViewModel vm, GamePlayer player)
    {
        this.vm = vm;
        Base = player;
        
        this.vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is nameof(vm.AllowSelf) or nameof(vm.Filter))
                OnPropertyChanged(nameof(CanBeTargeted));
        };
    }

    public bool CanBeTargeted => vm.Filter(Base) && (vm.AllowSelf || vm.SourceCard != Base.Card);

    public IRelayCommand<GamePlayer> PlayerSelectCommand => vm.PlayerSelectCommand;
}