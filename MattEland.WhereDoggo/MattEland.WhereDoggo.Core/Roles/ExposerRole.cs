namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Exposer role from One Night Ultimate Werewolf Alien expansion.
/// The Exposer may reveal a random number (1-3) of cards from the center
/// </summary>
/// <href>https://one-night.fandom.com/wiki/Exposer</href>
[RoleFor(RoleTypes.Exposer)]
public class ExposerRole : CardBase
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
            IHasCard? holder = player.PickSingleCard(game.CenterSlots.Where(s => !s.CurrentCard.IsRevealed));

            // Exposers may choose to skip exposing things
            if (holder == null)
            {
                game.LogEvent(new SkippedNightActionEvent(player));
                return;
            }

            holder.CurrentCard.IsRevealed = true;
            game.LogEvent(new RevealedRoleEvent(player, holder));
            game.LogEvent(new RevealedRoleObservedEvent(player, holder));
        }
    }
}