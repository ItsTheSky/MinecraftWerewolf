﻿using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class SpiderViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public SpiderViewModel(WerewolfGame game, GameCard source) : base(game, source,
        PlayerListType.OnlyAlive)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;
        AllowSelf = false;
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.IsParalyzed = true;
        game.NextCard();
    }

}