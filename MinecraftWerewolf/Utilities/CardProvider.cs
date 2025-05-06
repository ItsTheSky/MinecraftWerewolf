using System.Collections.Generic;
using MinecraftWerewolf.Core.Cards;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.Utilities;

public static class CardProvider
{

    public static readonly List<GameCard> AllCards =
    [
        new Allay(),
        new Axolotl(),
        new Bee(),
        new Blaze(),
        new Enderman(),
        new Endermite(),
        new IronGolem(),
        new Spider(),
        new Creeper()
    ];

}