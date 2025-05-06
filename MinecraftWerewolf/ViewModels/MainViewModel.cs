using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Cards;
using MinecraftWerewolf.Utilities;
using MinecraftWerewolf.ViewModels.Dialogs;
using MinecraftWerewolf.Views;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Controls;

namespace MinecraftWerewolf.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly MainView _mainView;
    
    public MainViewModel(MainView mainView)
    {
        _mainView = mainView;
        
        var enderman = new Enderman();
        var endermite = new Endermite();
        var ironGolem = new IronGolem();
        var axolotl = new Axolotl();
        var spider = new Bee();
        
        CurrentGame = new WerewolfGame
        {
            Players =
            [
                new GamePlayer("Player1", enderman),
                new GamePlayer("Player2", axolotl),
                new GamePlayer("Player3", spider),
                new GamePlayer("Player4", endermite),
                new GamePlayer("Player5", enderman),
                new GamePlayer("Player6", ironGolem),
                new GamePlayer("Player7", enderman)
            ],
        };
        
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
        
        await Dialog.ShowModal<DeathsSumUpDialog, DeathsSumUpViewModel>(new DeathsSumUpViewModel(killedPlayers), options: new DialogOptions()
        {
            Mode = DialogMode.Info,
            StartupLocation = WindowStartupLocation.CenterScreen,
            IsCloseButtonVisible = false,
            CanResize = false,
            Title = "Résumé des Morts",
        });
        
        CurrentGame.SetupNight();
    }
}