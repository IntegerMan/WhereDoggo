namespace MattEland.WhereDoggo.Core.Engine.Phases;

public class SetupGamePhase : GamePhaseBase
{
    public SetupGamePhase(Game game) : base(game)
    {
    }
    
    /// <inheritdoc />
    public override GamePhases Phase => GamePhases.Setup;

    /// <inheritdoc />
    public override string Name => "Setup";

    /// <inheritdoc />
    public override void Run(Game game)
    {
        
    }    
}