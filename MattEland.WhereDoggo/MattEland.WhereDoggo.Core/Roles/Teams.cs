namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The different teams a <see cref="RoleTypes"/> can be on.
/// </summary>
public enum Teams
{
    /// <summary>
    /// The villager team. Villagers win if at least one werewolf is killed.
    /// </summary>
    Villagers,
    /// <summary>
    /// The werewolf team. The werewolves wins if no werewolves are killed.
    /// </summary>
    Werewolves
}