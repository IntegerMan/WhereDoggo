namespace MattEland.WhereDoggo.Core.Engine.Phases;

public class NightPhase : GamePhaseBase
{
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
        
    }
}