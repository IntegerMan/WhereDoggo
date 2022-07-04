namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameRoleBase
{
    public virtual bool IsDoggo => false;
    public abstract RoleTypes Role { get; }
}