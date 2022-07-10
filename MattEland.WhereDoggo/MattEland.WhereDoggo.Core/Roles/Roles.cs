using System.ComponentModel;

namespace MattEland.WhereDoggo.Core.Roles;

public enum RoleTypes
{
    Villager,
    Werewolf,
    Insomniac,
    Sentinel,
    [Description("Apprentice Seer")]
    ApprenticeSeer
}
