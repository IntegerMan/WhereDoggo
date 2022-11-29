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
        foreach (NightActionBase nightAction in game.NightActions)
        {
            // Wake everyone in the group together
            EnqueueAction(() =>
            {
                IEnumerable<GamePlayer> relevantPlayers = nightAction.RelevantPlayers(game.Players).ToList();

                BroadcastEvent(nightAction.WakeInstructions);
                foreach (GamePlayer player in relevantPlayers)
                {
                    player.Wake();
                }

                // Now have them observe the board and take their actions
                foreach (GamePlayer player in relevantPlayers)
                {
                    player.ObserveVisibleState();
                    nightAction.PerformNightAction(game, player);
                }
            });

        }
    }

    /// <inheritdoc />
    public override string Name => "Night";
}