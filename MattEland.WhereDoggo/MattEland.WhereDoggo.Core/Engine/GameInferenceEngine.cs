namespace MattEland.WhereDoggo.Core.Engine;

public class GameInferenceEngine
{
    public IDictionary<RoleContainerBase, ContainerRoleProbabilities>
        BuildFinalRoleProbabilities(GamePlayer player, OneNightWhereDoggoGame game)
    {
        Dictionary<RoleContainerBase, ContainerRoleProbabilities> dicts = new();

        int numRoles = game.Entities.Count;
        int numRabbits = CountRolesOfType(game, RoleTypes.Rabbit);
        int numDoggo = CountRolesOfType(game, RoleTypes.Doggo);

        // Initial pass
        foreach (RoleContainerBase role in game.Entities)
        {
            ContainerRoleProbabilities probabilities = new(game);

            foreach (GameEventBase @event in player.Events)
            {
                @event.UpdatePlayerPerceptions(player, role, probabilities);
            }

            if (probabilities.IsCertain)
            {
                numRoles--;
                if (probabilities.ProbabilityRabbit >= 1)
                {
                    numRabbits--;
                }
                if (probabilities.ProbabilityDoggo >= 1)
                {
                    numDoggo--;
                }
            }

            dicts[role] = probabilities;
        }


        // Firm up uncertain probabilities
        foreach (KeyValuePair<RoleContainerBase, ContainerRoleProbabilities> kvp in dicts)
        {
            if (!kvp.Value.IsCertain)
            {
                kvp.Value.RecalculateProbability(numRoles, numRabbits, numDoggo);
            }
        }

        return dicts;
    }

    private static int CountRolesOfType(OneNightWhereDoggoGame game, RoleTypes role)
    {
        return game.Roles.Count(r => r.Role == role);
    }

    private static int CountOtherRolesOfType(GamePlayer player, OneNightWhereDoggoGame game, RoleTypes role)
    {
        int count = CountRolesOfType(game, role);
        if (player.InitialRole.Role == role)
        {
            count -= 1;
        }

        return count;
    }
}