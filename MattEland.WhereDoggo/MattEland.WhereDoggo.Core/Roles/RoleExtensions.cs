namespace MattEland.WhereDoggo.Core.Roles;

public static class RoleExtensions
{
    public static Teams DetermineTeam(this RoleTypes role)
    {
        switch (role)
        {
            case RoleTypes.Werewolf:
                return Teams.Werewolves;
            
            case RoleTypes.Villager:
            case RoleTypes.Insomniac:
            default:
                return Teams.Villagers;
        }
    }

    /// <summary>
    /// Creates a <see cref="GameRoleBase"/> out of a <see cref="RoleTypes"/>.
    /// </summary>
    /// <param name="roleType">The role to create</param>
    /// <returns>A <see cref="GameRoleBase"/> representing the specified <paramref name="roleType"/></returns>
    /// <exception cref="NotSupportedException">Thrown if no <see cref="GameRoleBase"/> is configured for this <see cref="RoleTypes"/></exception>
    public static GameRoleBase BuildGameRole(this RoleTypes roleType)
    {
        // TODO: Activator.CreateInstance mixed with attribute decorators could remove this manual step
        switch (roleType)
        {
            case RoleTypes.Villager:
                return new VillagerRole();
            case RoleTypes.Werewolf:
                return new WerewolfRole();
            case RoleTypes.Insomniac:
                return new InsomniacRole();
            case RoleTypes.Sentinel:
                return new SentinelRole();
            default:
                throw new NotSupportedException($"{nameof(BuildGameRole)} doesn't know how to create a role for {roleType}");
        }
    }

    public static Dictionary<RoleTypes, int> BuildRoleCounts(this Game game)
    {
        Dictionary<RoleTypes, int> roleCounts = new();

        foreach (RoleTypes role in game.Roles.Select(r => r.RoleType).Distinct())
        {
            int count = game.CountRolesOfType(role);
            roleCounts[role] = count;
        }

        return roleCounts;
    }    

    private static int CountRolesOfType(this Game game, RoleTypes role)
    {
        return game.Roles.Count(r => r.RoleType == role);
    }
    
}