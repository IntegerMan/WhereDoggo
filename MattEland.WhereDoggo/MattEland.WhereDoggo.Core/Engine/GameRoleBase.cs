namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameRoleBase
{
    public abstract RoleTypes RoleType { get; }
    public abstract Teams Team { get; }
}