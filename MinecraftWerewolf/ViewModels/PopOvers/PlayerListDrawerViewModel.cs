using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class PlayerListDrawerViewModel : ObservableObject
{
    
    [ObservableProperty] private ObservableCollection<GamePlayer> _players;
    
}