namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Revealer from One Night Ultimate Werewolf Daybreak
/// The Revealer looks at another player's card.
/// If that card is not a Werewolf or Tanner, the card stays flipped over
/// </summary>
/// <href>http://onenightultimate.com/?p=73</href>
[RoleFor(RoleTypes.Revealer)]
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
        CardContainer? target = player.PickSingleCard(game.Players.Where(p => p != player));

        if (target == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            GamePlayer targetPlayer = (GamePlayer)target;

            targetPlayer.IsRevealed = true;
            game.LogEvent(new RevealedRoleEvent(player, targetPlayer));
            game.LogEvent(new RevealedRoleObservedEvent(game.CurrentPhase, player, targetPlayer));

            // Werewolves should not be revealed
            if (targetPlayer.CurrentTeam == Teams.Werewolves)
            {
                targetPlayer.IsRevealed = false;
                game.LogEvent(new RevealerHidEvilRoleEvent(player, targetPlayer));
            }
        }
    }
}