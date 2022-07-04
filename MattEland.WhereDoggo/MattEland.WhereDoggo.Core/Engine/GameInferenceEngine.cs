namespace MattEland.WhereDoggo.Core.Engine;

public class GameInferenceEngine
{
    public IDictionary<RoleContainerBase, ContainerRoleProbabilities>
        BuildFinalRoleProbabilities(GamePlayer player, OneNightWhereDoggoGame game)
    {
        Dictionary<RoleContainerBase, ContainerRoleProbabilities> dicts = 
            new Dictionary<RoleContainerBase, ContainerRoleProbabilities>();

        foreach (RoleContainerBase role in game.Entities)
        {
            ContainerRoleProbabilities probabilities = new();

            if (role == player)
            {
                probabilities.MarkAsCertainOfRole(player.InitialRole);
            }
            else
            {
                int numOtherRoles = game.Entities.Count - 1;

                int numRabbits = CountOtherRolesOfType(player, game, RoleTypes.Rabbit);
                int numDoggos = CountOtherRolesOfType(player, game, RoleTypes.Doggo);

                probabilities.ProbabilityRabbit = numRabbits / (decimal) numOtherRoles;
                probabilities.ProbabilityDoggo = numDoggos / (decimal) numOtherRoles;
            }

            dicts[role] = probabilities;
        }

        return dicts;
    }

    private static int CountOtherRolesOfType(GamePlayer player, OneNightWhereDoggoGame game, RoleTypes role)
    {
        int count = game.Roles.Count(r => r.Role == role);
        if (player.InitialRole.Role == role)
        {
            count -= 1;
        }

        return count;
    }
}