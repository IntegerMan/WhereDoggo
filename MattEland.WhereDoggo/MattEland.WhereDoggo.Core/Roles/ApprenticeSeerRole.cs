namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The apprentice seer can view one card in the middle, much like a Lone Werewolf can.
/// </summary>
/// <href>http://onenightultimate.com/?p=53</href>
[RoleFor(RoleTypes.ApprenticeSeer)]
public class ApprenticeSeerRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.ApprenticeSeer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 5.1m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        IHasCard? slot = player.PickSingleCard(game.CenterSlots.Cast<IHasCard>().ToList());

        if (slot == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            game.LogEvent(new ObservedCenterCardEvent(player, slot));
        }
    }
}