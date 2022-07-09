using MattEland.WhereDoggo.Core.Events;
using MattEland.WhereDoggo.Core.Gamespace;
using MattEland.WhereDoggo.Core.Roles;

namespace MattEland.WhereDoggo.Core.Engine;

public class GameInferenceEngine
{
    public IDictionary<RoleContainerBase, ContainerRoleProbabilities>
        BuildFinalRoleProbabilities(GamePlayer player, OneNightWhereDoggoGame game)
    {
        Dictionary<RoleContainerBase, ContainerRoleProbabilities> dicts = new();

        int numRoles = game.Entities.Count;
        var counts = game.BuildRoleCounts();

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
                counts[probabilities.LikelyRole] -= 1;
            }

            dicts[role] = probabilities;
        }


        // Firm up uncertain probabilities
        foreach (KeyValuePair<RoleContainerBase, ContainerRoleProbabilities> kvp in dicts)
        {
            if (!kvp.Value.IsCertain)
            {
                kvp.Value.RecalculateProbability(numRoles, counts);
            }
        }

        return dicts;
    }

    private static int CountRolesOfType(OneNightWhereDoggoGame game, RoleTypes role)
    {
        return game.Roles.Count(r => r.RoleType == role);
    }

    private static int CountOtherRolesOfType(GamePlayer player, OneNightWhereDoggoGame game, RoleTypes role)
    {
        int count = CountRolesOfType(game, role);
        if (player.InitialRole.RoleType == role)
        {
            count -= 1;
        }

        return count;
    }
}