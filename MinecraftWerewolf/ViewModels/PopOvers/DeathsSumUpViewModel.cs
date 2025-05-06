using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class DeathsSumUpViewModel : ObservableObject
{
    public DeathsSumUpViewModel(List<PlayerDeath> killedPlayers)
    {
        foreach (var player in killedPlayers)
            DeadPlayers.Add(player);
        
        AnyDeath = killedPlayers.Count > 0;
    }
    
    [ObservableProperty] private ObservableCollection<PlayerDeath> _deadPlayers = new();
    [ObservableProperty] private bool _anyDeath;
    
}