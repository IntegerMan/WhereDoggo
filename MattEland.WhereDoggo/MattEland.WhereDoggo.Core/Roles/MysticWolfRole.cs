namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Mystic Wolf is a werewolf that may look at one player's card during the night 
/// </summary>
/// <href>http://onenightultimate.com/?p=37</href>
[RoleFor(RoleTypes.MysticWolf)]
public class MysticWolfRole : WerewolfRole
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.MysticWolf;

    /// <inheritdoc />
    public override Teams Team => Teams.Werewolves;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 2.2m;
    

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        base.PerformNightAction(game, player);

        IHasCard? cardHolder = player.PickSingleCard(game.Players.Where(p => p != player));

        if (cardHolder == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            game.LogEvent(new ObservedPlayerCardEvent(player, cardHolder));
        }
    }
    
    
}