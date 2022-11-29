using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class MasonNightAction : RoleNightActionBase
{
    public MasonNightAction() : base(RoleTypes.Mason)
    {
    }

    /// <inheritdoc />
    public override string WakeInstructions => "Masons, wake up and see if there are other Masons";


    /// <inheritdoc />
    public override decimal NightActionOrder => 4.0m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        List<GamePlayer> otherPlayers = game.Players.Where(p => p != player).ToList();

        // If no other masons awoke, log an event indicating we know they're a mason
        if (otherPlayers.All(p => p.InitialCard is not MasonRole))
        {
            game.LogEvent(new OnlyMasonEvent(player));
        }

        // Observe each other player and learn if they're a mason or not
        otherPlayers.ForEach(observedPlayer =>
        {
            if (observedPlayer.InitialCard is MasonRole)
            {
                // If they didn't wake up, we now know they can't be a mason
                game.LogEvent(new KnowsRoleEvent(player, observedPlayer));
            }
            else
            {
                // If we saw another mason, record it
                game.LogEvent(new SawNotRoleEvent(player, observedPlayer, RoleTypes.Mason));
            }
        });
    }
}