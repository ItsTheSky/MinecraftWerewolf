using CommunityToolkit.Mvvm.ComponentModel;

namespace MinecraftWerewolf.Core.Models;

/// <summary>
/// Represent a pre-game player (used for config, not
/// yet in an actual game).
/// </summary>
public partial class PreGamePlayer : ObservableObject
{

    [ObservableProperty] private string _name;
    [ObservableProperty] private GameCard _card;

    [ObservableProperty] private PreGamePlayer? _left;
    [ObservableProperty] private PreGamePlayer? _right;

}