namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Extension methods related to <see cref="RoleTypes"/>
/// </summary>
public static class RoleExtensions
{
    /// <summary>
    /// Determines which team a given role is on.
    /// </summary>
    /// <param name="role">The role to consider. Can be used as an extension method.</param>
    /// <returns>The <see cref="Teams"/> the <paramref name="role"/> belongs to</returns>
    public static Teams DetermineTeam(this RoleTypes role)
    {
        switch (role)
        {
            case RoleTypes.Werewolf:
                return Teams.Werewolves;
            
            case RoleTypes.Villager:
            case RoleTypes.Insomniac:
            case RoleTypes.Sentinel:
            case RoleTypes.ApprenticeSeer:
            case RoleTypes.Mason:
            case RoleTypes.Revealer:
            default:
                return Teams.Villagers;
        }
    }

    /// <summary>
    /// Creates a <see cref="RoleBase"/> out of a <see cref="RoleTypes"/>.
    /// </summary>
    /// <param name="roleType">The role to create</param>
    /// <returns>A <see cref="RoleBase"/> representing the specified <paramref name="roleType"/></returns>
    /// <exception cref="NotSupportedException">Thrown if no <see cref="RoleBase"/> is configured for this <see cref="RoleTypes"/></exception>
    public static RoleBase BuildGameRole(this RoleTypes roleType)
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
            case RoleTypes.ApprenticeSeer:
                return new ApprenticeSeerRole();
            case RoleTypes.Mason:
                return new MasonRole();
            case RoleTypes.Revealer:
                return new RevealerRole();
            case RoleTypes.Exposer:
                return new ExposerRole();
            default:
                throw new NotSupportedException($"{nameof(BuildGameRole)} doesn't know how to create a role for {roleType}");
        }
    }

    /// <summary>
    /// Builds and returns a dictionary of occurrences of a given <see cref="RoleTypes"/> within a game.
    /// This lets you track the number of cards of a specific role, knowing there may be multiple in a game.
    /// </summary>
    /// <remarks>
    /// <see cref="RoleTypes"/> that are not present in the <paramref name="game"/> will not be included in the results.
    /// </remarks>
    /// <param name="game">The game to search. Can be used as an extension method</param>
    /// <returns>A dictionary of role counts by role.</returns>
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

    private static int CountRolesOfType(this Game game, RoleTypes role) => game.Roles.Count(r => r.RoleType == role);
}