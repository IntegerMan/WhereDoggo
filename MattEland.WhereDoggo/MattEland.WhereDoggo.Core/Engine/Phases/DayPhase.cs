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
    protected internal override void Initialize(Game game)
    {
        EnqueueAction(() => WakeAll(game));

        // Role Claim goes in two rounds: initial and final
        // Players may defer in the initial, then must claim in the final
        EnqueueAction(() => PerformInitialRoleClaim(game));
        EnqueueAction(() => PerformFinalRoleClaim(game));
    }

    /// <inheritdoc />
    public override string Name => "Day";

    private void PerformInitialRoleClaim(Game game)
    {
        foreach (GamePlayer player in game.Players)
        {
            RoleTypes? roleClaim = player.GetInitialRoleClaim();

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

    private void PerformFinalRoleClaim(Game game)
    {
        foreach (GamePlayer player in game.Players.Where(p => p.Events.OfType<DeferredClaimingRoleEvent>().Any(e => e.Player == p)))
        {
            RoleTypes roleClaim = player.GetFinalRoleClaim();

            BroadcastEvent(new ClaimedRoleEvent(player, roleClaim));
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