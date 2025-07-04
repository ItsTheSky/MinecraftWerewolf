﻿using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.Base;

public partial class BasePlayerSelectViewModel(WerewolfGame game, GameCard source, Predicate<GamePlayer> filter) : PlayerListViewModel(game.Players, source, filter)
{

    [ObservableProperty] private bool _allowNoSelection;
    [ObservableProperty] private string _taskText = source.Description;

    public PlayerListViewModel ViewModel => this;

    [RelayCommand]
    public void NoSelection()
    {
        game.NextCard();
    }
}