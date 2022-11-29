namespace MattEland.WhereDoggo.Core.Engine.Phases;

public abstract class RoleNightActionBase : NightActionBase
{
    private readonly IEnumerable<RoleTypes> _roles;

    protected RoleNightActionBase(RoleTypes role)
    {
        _roles = new[] {role};
    }

    protected RoleNightActionBase(IEnumerable<RoleTypes> roles)
    {
        _roles = roles.ToArray();
    }

    public override IEnumerable<GamePlayer> RelevantPlayers(IEnumerable<GamePlayer> players) 
        => players.Where(p => _roles.Contains(p.InitialCard.RoleType));
}