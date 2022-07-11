namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// An Attribute that gets applied to classes and indicates that they can be used to create instances of that class for a specific role.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RoleForAttribute : Attribute
{
    /// <summary>
    /// The <see cref="RoleTypes"/> this class can work for
    /// </summary>
    public RoleTypes Role { get; }

    /// <summary>
    /// Instantiates a new instance of <see cref="RoleForAttribute"/>
    /// </summary>
    /// <param name="role">The role this class can serve</param>
    public RoleForAttribute(RoleTypes role)
    {
        Role = role;
    }
}