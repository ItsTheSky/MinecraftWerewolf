using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core;

namespace MinecraftWerewolf.ViewModels.Dialogs;

public partial class DeathsSumUpViewModel : ObservableObject
{
    public DeathsSumUpViewModel(List<GamePlayer> killedPlayers)
    {
        foreach (var player in killedPlayers)
        {
            var internalPlayer = new InternalGamePlayer
            {
                Base = player,
                WasLover = player.Love != null
            };
            DeadPlayers.Add(internalPlayer);
        }
        
        AnyDeath = killedPlayers.Count > 0;
    }
    
    [ObservableProperty] private ObservableCollection<InternalGamePlayer> _deadPlayers = new();
    [ObservableProperty] private bool _anyDeath;
    
}

public partial class InternalGamePlayer : ObservableObject
{

    [ObservableProperty] private GamePlayer _base;
    [ObservableProperty] private bool _wasLover;

}