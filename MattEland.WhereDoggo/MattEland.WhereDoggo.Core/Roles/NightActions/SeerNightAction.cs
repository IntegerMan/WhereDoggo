namespace MattEland.WhereDoggo.Core.Roles.NightActions;

public class SeerNightAction : RoleNightActionBase
{
    public SeerNightAction() : base(RoleTypes.Seer)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "Seer, wake up and look at one player's card or two from the center";


    /// <inheritdoc />
    public override decimal NightActionOrder => 5m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        // Choose whether we're skipping, getting another player, or getting 2 center cards
        IList<IHasCard> playerChoices = game.Players.Where(p => p != player).Cast<IHasCard>().ToList();
        IList<IHasCard> centerChoices = game.CenterSlots.Cast<IHasCard>().ToList();
        List<IHasCard> cards = player.PickSeerCards(playerChoices, centerChoices);

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