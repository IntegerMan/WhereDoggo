namespace MattEland.WhereDoggo.Core.Roles;

public class InsomniacRole : GameRoleBase
{
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Insomniac;

    public override decimal? NightActionOrder => 9.0m;
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        game.LogEvent(new InsomniacSawOwnCardEvent(player));
    }
}