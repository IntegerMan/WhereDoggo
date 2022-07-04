namespace MattEland.WhereDoggo.Core.Engine.Roles;

public class DoggoRole : GameRoleBase
{
    public override string ToString() => "Doggo";

    public override bool IsDoggo => true;

    public override RoleTypes Role => RoleTypes.Doggo;
}