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
    public override GamePhases Phase => GamePhases.Night;

    /// <inheritdoc />
    public override string Name => "Night";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
        foreach (GamePlayer player in game.Players.Where(p => p.InitialRole.HasNightAction).OrderBy(p => p.InitialRole.NightActionOrder))
        {
            player.Wake();
            player.InitialRole.PerformNightAction(game, player);
        }        
    }
}