using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.Views.Base;

public partial class PlayerListDisplay : UserControl
{
    public static readonly StyledProperty<RelayCommand<GamePlayer>> PlayerSelectCommandProperty =
        AvaloniaProperty.Register<PlayerListDisplay, RelayCommand<GamePlayer>>(nameof(PlayerSelectCommand));

    public static readonly StyledProperty<bool> AllowDeadPlayersProperty =
        AvaloniaProperty.Register<PlayerListDisplay, bool>(nameof(AllowDeadPlayers), false);

    public static readonly StyledProperty<WerewolfGame> GameProperty =
        AvaloniaProperty.Register<PlayerListDisplay, WerewolfGame>(nameof(Game));

    public static readonly StyledProperty<GameCard> SourceCardProperty =
        AvaloniaProperty.Register<PlayerListDisplay, GameCard>(nameof(SourceCard));

    public RelayCommand<GamePlayer> PlayerSelectCommand
    {
        get => GetValue(PlayerSelectCommandProperty);
        set => SetValue(PlayerSelectCommandProperty, value);
    }

    public bool AllowDeadPlayers
    {
        get => GetValue(AllowDeadPlayersProperty);
        set => SetValue(AllowDeadPlayersProperty, value);
    }

    public WerewolfGame Game
    {
        get => GetValue(GameProperty);
        set => SetValue(GameProperty, value);
    }

    public GameCard SourceCard
    {
        get => GetValue(SourceCardProperty);
        set => SetValue(SourceCardProperty, value);
    }

    public PlayerListDisplay()
    {
        InitializeComponent();
    }
}