namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The seer from One Night Ultimate Werewolf.
/// The seer may look at one other player's card, look at 2 cards from the center, or do nothing in her night action.
/// </summary>
/// <href>http://onenightultimate.com/?p=51</href> 
[RoleFor(RoleTypes.Seer)]
public class SeerRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Seer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 5m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        base.PerformNightAction(game, player);
        
        // Choose whether we're skipping, getting another player, or getting 2 center cards
        IList<CardContainer> playerChoices = game.Players.Where(p => p != player).Cast<CardContainer>().ToList(); 
        IList<CardContainer> centerChoices = game.CenterSlots.Cast<CardContainer>().ToList();
        List<CardContainer> cards = player.PickSeerCards(playerChoices, centerChoices);

        switch (cards.Count)
        {
            case <= 0:
                // A seer should really never skip their night action. You're just losing information.
                game.LogEvent(new SkippedNightActionEvent(player));
                break;
            case 1:
                // Look at the other player's card
                game.LogEvent(new ObservedPlayerCardEvent(player, cards.Single()));
                break;
            default:
                // Look at the center cards
                cards.ForEach(c => game.LogEvent(new ObservedCenterCardEvent(player, c)));
                break;
        }
    }
}