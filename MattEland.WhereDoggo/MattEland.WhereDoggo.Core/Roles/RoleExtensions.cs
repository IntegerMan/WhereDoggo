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
    

    public static Dictionary<RoleTypes, int> BuildRoleCounts(this OneNightWhereDoggoGame game)
    {
        Dictionary<RoleTypes, int> roleCounts = new();

        foreach (RoleTypes role in game.Roles.Select(r => r.RoleType).Distinct())
        {
            int count = game.CountRolesOfType(role);
            roleCounts[role] = count;
        }

        return roleCounts;
    }    

    private static int CountRolesOfType(this OneNightWhereDoggoGame game, RoleTypes role)
    {
        return game.Roles.Count(r => r.RoleType == role);
    }
    
}