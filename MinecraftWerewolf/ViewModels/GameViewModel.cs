using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.Utilities;
using MinecraftWerewolf.ViewModels.PopOvers;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;
using DeathsSumUpViewModel = MinecraftWerewolf.ViewModels.PopOvers.DeathsSumUpViewModel;

namespace MinecraftWerewolf.ViewModels;

public partial class GameViewModel : ObservableObject
{
    public GameViewModel(List<PreGamePlayer> players)
    {
        CurrentGame = new WerewolfGame();

        var dict = new Dictionary<PreGamePlayer, GamePlayer>();
        foreach (var player in players)
        {
            var gamePlayer = new GamePlayer(player.Name, player.Card);
            dict.Add(player, gamePlayer);
            CurrentGame.Players.Add(gamePlayer);
        }
        
        // Define left/right players
        foreach (var player in players)
        {
            var gamePlayer = dict[player];
            
            gamePlayer.LeftPlayer = CurrentGame.Players.FirstOrDefault(p => p.Name == player.Left!.Name);
            gamePlayer.RightPlayer = CurrentGame.Players.FirstOrDefault(p => p.Name == player.Right!.Name);
        }
        
        CurrentGame.StartGame();
        CurrentGame.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(CurrentGame.IsNight))
            {
                Console.WriteLine("Night changed to " + CurrentGame.IsNight);
                if (!CurrentGame.IsNight)
                {
                    DebateTimerTime = 300; // 5 minutes
                    if (debateTimer != null)
                    {
                        debateTimer.Stop();
                        debateTimer.Dispose();
                    }
                    
                    debateTimer = new Timer(1000);
                    debateTimer.Elapsed += (s, e) =>
                    {
                        if (DebateTimerTime > 0)
                        {
                            DebateTimerTime--;
                        }
                        else
                        {
                            debateTimer.Stop();
                            debateTimer.Dispose();
                            DebateOver = true;
                        }
                    };
                    debateTimer.Start();
                }
            }
        };
        
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(DebateTimerTime))
                OnPropertyChanged(nameof(DebateTimerText));
        };
    }
    
    private Timer? debateTimer;

    [ObservableProperty] private WerewolfGame _currentGame;
    [ObservableProperty] private int _debateTimerTime;
    [ObservableProperty] private bool _debateOver = false;

    [ObservableProperty] private GamePlayer? _selectedPlayer;
    
    public string DebateTimerText => $"{DebateTimerTime / 60:D2}:{DebateTimerTime % 60:D2}";
    
    [RelayCommand]
    public void StopDebate()
    {
        if (debateTimer != null)
        {
            debateTimer.Stop();
            debateTimer.Dispose();
        }
        
        DebateOver = true;
        SelectedPlayer = null;
    }

    [RelayCommand]
    public async Task FinishPlayerVote()
    {
        if (SelectedPlayer == null)
            throw new InvalidOperationException("Please select a player before voting.");

        var killedPlayers = SelectedPlayer.Die();
        
        await OverlayDialog.ShowModal<DeathsSumUpDialog, DeathsSumUpViewModel>(new DeathsSumUpViewModel(killedPlayers), "LocalHost", options: new OverlayDialogOptions()
        {
            Mode = DialogMode.Info,
            IsCloseButtonVisible = false,
            CanResize = false,
            Title = "Résumé des Morts",
            Buttons = DialogButton.OK
        });
        
        CurrentGame.SetupNight();
    }

    [RelayCommand]
    public async Task ShowPlayers()
    {
        var vm = new PlayerListDrawerViewModel { Players = new ObservableCollection<GamePlayer>(CurrentGame.Players) };
        var options = new DrawerOptions
        {
            Title = "Liste des joueurs",
            CanResize = false,
            IsCloseButtonVisible = false,
            Buttons = DialogButton.OK,
            Position = Position.Bottom
        };

        await Drawer.ShowModal<PlayerListDrawer, PlayerListDrawerViewModel>(vm, "LocalHost", options);
    }
}