using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class ChargedCreeperSelectorViewModel(WerewolfGame game)
    : PlayerListViewModel(game.Players, null, PlayerListType.OnlyAlive)
{
    [ObservableProperty] private InternalGamePlayer? _leftPlayer;
    [ObservableProperty] private InternalGamePlayer? _rightPlayer;
}