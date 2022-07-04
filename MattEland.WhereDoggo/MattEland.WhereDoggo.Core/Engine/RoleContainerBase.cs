﻿namespace MattEland.WhereDoggo.Core.Engine;

public abstract class RoleContainerBase
{
    protected RoleContainerBase(string name, GameRoleBase initialRole)
    {
        Name = name;
        InitialRole = initialRole;
        CurrentRole = initialRole;
    }

    public string Name { get; }
    public GameRoleBase InitialRole { get; }
    public GameRoleBase CurrentRole { get; set; }

    public override string ToString() => Name;
}