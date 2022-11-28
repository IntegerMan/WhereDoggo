namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The night phase has each role with a night action wake up and take that action in sequence.
/// Players may wake multiple times in some cases.
/// </summary>
public class NightPhase : GamePhaseBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NightPhase"/> class.
    /// </summary>
    /// <param name="game">The game instance</param>
    public NightPhase(Game game) : base(game)
    {
    }

    /// <inheritdoc />
    protected internal override void Initialize(Game game)
    {
        List<IGrouping<decimal?, GamePlayer>> wakeGroups = game.Players.Where(p => p.InitialCard.HasNightAction)
                                                 .OrderBy(p => p.InitialCard.NightActionOrder)
                                                 .GroupBy(p => p.InitialCard.NightActionOrder)
                                                 .ToList();

        foreach (IGrouping<decimal?, GamePlayer> group in wakeGroups)
        {
            // Wake everyone in the group together
            EnqueueAction(() =>
            {
                foreach (GamePlayer player in group)
                {
                    player.Wake();
                }
            });

            // Now have them observe the board and take their actions
            foreach (GamePlayer player in group)
            {
                EnqueueAction(() =>
                {
                    player.ObserveVisibleState();
                    player.InitialCard.PerformNightAction(game, player);
                });
            }
        }
    }

    /// <inheritdoc />
    public override string Name => "Night";
}