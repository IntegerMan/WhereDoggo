namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The insomniac from One Night Ultimate Werewolf.
/// Insomniacs wake near the end of the night and look at their own card to see if their role changed.
/// </summary>
/// <href>http://onenightultimate.com/?p=70</href>
public class InsomniacRole : RoleBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Insomniac;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 9.0m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player) 
        => game.LogEvent(new InsomniacSawOwnCardEvent(player));
}