namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// A villager in One Night Ultimate Werewolf.
/// Villagers have no night ability and have very little information but are on the village team.
/// </summary>
[RoleFor(RoleTypes.Villager)]
public class VillagerRole : CardBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;
    
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Villager;


    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield break;
        }
    }
}