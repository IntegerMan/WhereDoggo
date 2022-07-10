namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Revealer from One Night Ultimate Werewolf Daybreak
/// The Revealer looks at another player's card.
/// If that card is not a Werewolf or Tanner, the card stays flipped over
/// </summary>
/// <href>http://onenightultimate.com/?p=73</href>
public class RevealerRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Revealer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 10m;
    
    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        RoleContainerBase? target = player.Strategies.PickSingleCardStrategy.SelectCard(game.Players);

        if (target == null)
        {
            // TODO: Is this optional?
        }
        else
        {
            GamePlayer targetPlayer = (GamePlayer)target;

            targetPlayer.IsRevealed = true;
            game.LogEvent(new KnowsRoleEvent(game.CurrentPhase, player, targetPlayer));
        }
    }
}