namespace MattEland.WhereDoggo.Core.Engine.Phases;

public class DayPhase : GamePhaseBase
{
    public DayPhase(Game game) : base(game)
    {
    }
    
    /// <inheritdoc />
    public override GamePhases Phase => GamePhases.Day;

    /// <inheritdoc />
    public override string Name => "Day";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
    }    
}