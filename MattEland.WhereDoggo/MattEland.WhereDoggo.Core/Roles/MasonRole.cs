using Microsoft.ML.Probabilistic.Collections;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents a Mason role from One Night Ultimate Werewolf (base game).
/// Masons wake up in the night and observe other masons.
/// </summary>
/// <href>http://onenightultimate.com/?p=48</href>
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
        game.Players.Where(p => p != player).ForEach(observedPlayer =>
        {
            if (observedPlayer.InitialRole.RoleType == RoleTypes.Mason)
            {
                game.LogEvent(new KnowsRoleEvent(game.CurrentPhase, player, observedPlayer));
            }
            else
            {
                game.LogEvent(new SawNotRoleEvent(player, observedPlayer, RoleTypes.Mason));
            }
        });
    }
}