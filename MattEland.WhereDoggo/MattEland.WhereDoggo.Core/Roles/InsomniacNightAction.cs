using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class InsomniacNightAction : RoleNightActionBase
{
    public InsomniacNightAction() : base(RoleTypes.Insomniac)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "Insomniac, wake up and look at your card to see if you are still an insomniac";


    /// <inheritdoc />
    public override decimal NightActionOrder => 9.0m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
        => game.LogEvent(new InsomniacSawOwnCardEvent(player));
}