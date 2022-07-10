namespace MattEland.WhereDoggo.Core.Roles;

public class InsomniacRole : GameRoleBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Insomniac;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 9.0m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        game.LogEvent(new InsomniacSawOwnCardEvent(player));
    }
}