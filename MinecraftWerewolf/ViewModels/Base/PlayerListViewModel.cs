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
    public PlayerListViewModel(ObservableCollection<GamePlayer> players, GameCard? source)
    {
        Players = new ObservableCollection<InternalGamePlayer>(players.Select(p =>
            new InternalGamePlayer(this, p)));
        SourceCard = source;
    }

    [ObservableProperty] private ObservableCollection<InternalGamePlayer> _players;
    [ObservableProperty] private PlayerListType _listType = PlayerListType.OnlyAlive;
    [ObservableProperty] private bool _allowSelf;
    [ObservableProperty] private GameCard? _sourceCard;
    [ObservableProperty] private IRelayCommand<GamePlayer> _playerSelectCommand;
}

public enum PlayerListType
{
    All,
    OnlyAlive,
    OnlyDead,
    OnlyDying
}

public partial class InternalGamePlayer : ObservableObject
{
    [ObservableProperty] private GamePlayer _base;

    private readonly PlayerListViewModel vm;

    public InternalGamePlayer(PlayerListViewModel vm, GamePlayer player)
    {
        this.vm = vm;
        Base = player;

        this.vm.PropertyChanged += (sender, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(vm.ListType):
                case nameof(vm.AllowSelf):
                    OnPropertyChanged(nameof(CanBeTargeted));
                    break;
            }
        };
        Base.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Base.IsAlive))
                OnPropertyChanged(nameof(CanBeTargeted));
        };
    }

    public bool CanBeTargeted
    {
        get
        {
            switch (vm.ListType)
            {
                case PlayerListType.OnlyAlive when !Base.IsAlive:
                case PlayerListType.OnlyDead when Base.IsAlive:
                case PlayerListType.OnlyDying when !Base.ShouldDie:
                    return false;
                
                case PlayerListType.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (vm.SourceCard == null || vm.AllowSelf)
                return true;
            
            if (vm.SourceCard == Base.Card)
                return false;
            
            return true;
        }
    }

    public IRelayCommand<GamePlayer> PlayerSelectCommand => vm.PlayerSelectCommand;
}