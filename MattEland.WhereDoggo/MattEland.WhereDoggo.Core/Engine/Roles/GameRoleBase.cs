﻿namespace MattEland.WhereDoggo.Core.Engine.Roles;

public abstract class GameRoleBase
{

    public override string ToString() => RoleType.ToString(); // TODO: Use Friendly Name
    public abstract RoleTypes RoleType { get; }
    public abstract Teams Team { get; }
}