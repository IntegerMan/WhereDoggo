using System.ComponentModel;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The available roles that can be added to a game of One Night Ultimate Werewolf.
/// </summary>
public enum RoleTypes
{
    Villager,
    Werewolf,
    Insomniac,
    Sentinel,
    [Description("Apprentice Seer")]
    ApprenticeSeer
}
