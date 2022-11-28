namespace MattEland.WhereDoggo.Core.Engine.Phases;

public abstract class RoleNightActionBase : NightActionBase
{
    private readonly RoleTypes _role;

    protected RoleNightActionBase(RoleTypes role)
    {
        _role = role;
    }

    public override IEnumerable<GamePlayer> RelevantPlayers(IEnumerable<GamePlayer> players) 
        => players.Where(p => p.InitialCard.RoleType == _role);
}