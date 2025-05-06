namespace MinecraftWerewolf.Core.Models;

public record PlayerDeath(GamePlayer Player, DeathSource Source, int Level = 0)
{

    public int LeftSpacing => Level <= 1 ? 0 : 20 * (Level - 1);
    public bool ShowArrow => Level > 0;

}