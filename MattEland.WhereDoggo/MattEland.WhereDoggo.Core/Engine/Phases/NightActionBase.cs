using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine.Phases;

public abstract class NightActionBase
{
    public abstract decimal NightActionOrder { get; }

    public abstract string WakeInstructions { get; }

    public abstract IEnumerable<GamePlayer> RelevantPlayers(IEnumerable<GamePlayer> players);

    public abstract void PerformNightAction(Game game, GamePlayer player);

    public virtual IEnumerable<ClaimBase> GenerateClaims(GamePlayer player)
    {
        yield break;
    }
}
