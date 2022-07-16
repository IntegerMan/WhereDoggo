namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Exposer role from One Night Ultimate Werewolf Alien expansion.
/// The Exposer may reveal a random number (1-3) of cards from the center
/// </summary>
/// <href>https://one-night.fandom.com/wiki/Exposer</href>
[RoleFor(RoleTypes.Exposer)]
public class ExposerRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Exposer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 10.2m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        int numToExpose = game.Options.ExposerOptions.DetermineCardsToExpose(game.Randomizer);

        for (int i = 0; i < numToExpose; i++)
        {
            CardContainer? card = player.PickSingleCard(game.CenterSlots.Where(c => !c.IsRevealed));

            // Exposers may choose to skip exposing things
            if (card == null)
            {
                game.LogEvent(new SkippedNightActionEvent(player));
                return;
            }

            card.IsRevealed = true;
            game.LogEvent(new RevealedRoleEvent(player, card));
            game.LogEvent(new RevealedRoleObservedEvent(game.CurrentPhase, player, card));
        }
    }
}