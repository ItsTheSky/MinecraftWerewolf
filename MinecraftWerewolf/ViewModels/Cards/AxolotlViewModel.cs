using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;
using MinecraftWerewolf.Views.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class AxolotlViewModel : PlayerListViewModel
{
    private WerewolfGame game;
    
    public AxolotlViewModel(WerewolfGame game, GameCard source) : base(game.Players, source)
    {
        this.game = game;

        PropertyChanged += (sender, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(FirstPlayerValue):
                    FirstPlayer = FirstPlayerValue?.Base;
                    OnPropertyChanged(nameof(HasSelectedTwoPlayers));
                    Console.WriteLine("FirstPlayerValue changed! New value: " + FirstPlayerValue?.Base);
                    OnPropertyChanged(nameof(HasSelectedTwoPlayers));
                    OnPropertyChanged(nameof(SameSelectedPlayers));
                    break;
                case nameof(SecondPlayerValue):
                    SecondPlayer = SecondPlayerValue?.Base;
                    OnPropertyChanged(nameof(HasSelectedTwoPlayers));
                    Console.WriteLine("SecondPlayerValue changed! New value: " + SecondPlayerValue?.Base);
                    OnPropertyChanged(nameof(HasSelectedTwoPlayers));
                    OnPropertyChanged(nameof(SameSelectedPlayers));
                    break;
            }
        };
    }

    [RelayCommand]
    public void OnValidate()
    {
        if (FirstPlayer == null || SecondPlayer == null)
            throw new InvalidOperationException("Please select two players before validating.");

        FirstPlayer.Love = SecondPlayer;
        SecondPlayer.Love = FirstPlayer;

        game.NextCard();
    }

    [ObservableProperty] private GamePlayer? _firstPlayer;
    [ObservableProperty] private GamePlayer? _secondPlayer;

    [ObservableProperty] private InternalGamePlayer? _firstPlayerValue;
    [ObservableProperty] private InternalGamePlayer? _secondPlayerValue;
    
    public bool SameSelectedPlayers => FirstPlayer == SecondPlayer && FirstPlayer != null;
    public bool HasSelectedTwoPlayers => FirstPlayer != null && SecondPlayer != null && !SameSelectedPlayers;
}