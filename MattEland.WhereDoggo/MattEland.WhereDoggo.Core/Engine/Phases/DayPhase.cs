using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The day phase has all players wake up and look around
/// </summary>
public class DayPhase : GamePhaseBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DayPhase"/> class.
    /// </summary>
    /// <param name="game">The game instance</param>
    public DayPhase(Game game) : base(game)
    {
    }

    /// <inheritdoc />
    public override string Name => "Day";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
        // Wake all players up
        WakeAll(game);

        // Each player should claim their role if they're good
        PerformInitialRoleClaim(game);
    }

    private void PerformInitialRoleClaim(Game game)
    {
        foreach (GamePlayer player in game.Players)
        {
            RoleTypes? roleClaim = player.GetRoleClaim();

            if (roleClaim == null)
            {
                BroadcastEvent(new DeferredClaimingRoleEvent(player));
            }
            else
            {
                BroadcastEvent(new ClaimedRoleEvent(player, roleClaim.Value));
            }
        }
    }

    private static void WakeAll(Game game)
    {
        foreach (GamePlayer p in game.Players)
        {
            p.Wake();
        }

        foreach (GamePlayer p in game.Players)
        {
            p.ObserveVisibleState();
        }
    }
}