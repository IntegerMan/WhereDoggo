namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The available roles that can be added to a game of One Night Ultimate Werewolf.
/// </summary>
public enum RoleTypes
{
    /// <summary>
    /// The <see cref="VillagerRole" />.
    /// </summary>
    Villager,
    
    /// <summary>
    /// The <see cref="WerewolfRole" />.
    /// </summary>
    Werewolf,
    
    /// <summary>
    /// The <see cref="InsomniacRole" />.
    /// </summary>
    Insomniac,
    
    /// <summary>
    /// The <see cref="SentinelRole" />.
    /// </summary>
    Sentinel,

    /// <summary>
    /// The <see cref="ApprenticeSeerRole" />.
    /// </summary>
    [Description("Apprentice Seer")]
    ApprenticeSeer,
    
    /// <summary>
    /// The <see cref="MasonRole" />.
    /// </summary>
    Mason,
    
    /// <summary>
    /// The <see cref="RevealerRole"/>
    /// </summary>
    Revealer,
    
    /// <summary>
    /// The <see cref="ExposerRole"/>
    /// </summary>
    Exposer,
    
    /// <summary>
    /// The <see cref="MysticWolfRole"/>
    /// </summary>
    [Description("Mystic Wolf")]
    MysticWolf,
    
    /// <summary>
    /// The <see cref="ThingRole"/> (that goes bump in the night)
    /// </summary>
    [Description("The Thing")]
    Thing
}
