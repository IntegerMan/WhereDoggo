namespace MattEland.WhereDoggo.Core.Roles.NightActions;

public class MysticWolfNightAction : RoleNightActionBase
{
    public MysticWolfNightAction() : base(RoleTypes.MysticWolf)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "Mystic Wolf, wake up and look at one other player's card";

    /// <inheritdoc />
    public override decimal NightActionOrder => 2.2m;


    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        IEnumerable<IHasCard> otherPlayerTargets = game.GetOtherPlayerTargets(player);
        IDictionary<IHasCard, CardProbabilities> probs = player.Brain.BuildInitialRoleProbabilities();
        IHasCard? cardHolder = player.PickSingleCard(otherPlayerTargets.Where(t => probs[t].CalculateTeamProbability(Teams.Werewolves) < 1m));

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