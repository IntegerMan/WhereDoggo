using Microsoft.ML.Probabilistic.Collections;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents a Mason role from One Night Ultimate Werewolf (base game).
/// Masons wake up in the night and observe other masons.
/// </summary>
/// <href>http://onenightultimate.com/?p=48</href>
[RoleFor(RoleTypes.Mason)]
public class MasonRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Mason;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 4m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        List<GamePlayer> otherPlayers = game.Players.Where(p => p != player).ToList();

        // If no other masons awoke, log an event indicating we know they're a mason
        if (otherPlayers.All(p => p.InitialRole.RoleType != RoleTypes.Mason))
        {
            game.LogEvent(new OnlyMasonEvent(player));
        }
        
        // Observe each other player and learn if they're a mason or not
        otherPlayers.ForEach(observedPlayer =>
        {
            if (observedPlayer.InitialRole.RoleType == RoleTypes.Mason)
            {
                // If they didn't wake up, we now know they can't be a mason
                game.LogEvent(new KnowsRoleEvent(game.CurrentPhase, player, observedPlayer));
            }
            else
            {
                // If we saw another mason, record it
                game.LogEvent(new SawNotRoleEvent(player, observedPlayer, RoleTypes.Mason));
            }
        });
    }
}