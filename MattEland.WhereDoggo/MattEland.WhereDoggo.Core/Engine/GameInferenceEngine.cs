
namespace MattEland.WhereDoggo.Core.Engine;

public class GameInferenceEngine
{
    public IDictionary<RoleContainerBase, ContainerRoleProbabilities>
        BuildFinalRoleProbabilities(GamePlayer player, Game game)
    {
        Dictionary<RoleContainerBase, ContainerRoleProbabilities> dicts = new();

        Dictionary<RoleTypes, int> counts = game.BuildRoleCounts();

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
                counts[probabilities.LikelyRole] -= 1;
            }

            dicts[role] = probabilities;
        }


        // Firm up uncertain probabilities
        foreach (KeyValuePair<RoleContainerBase, ContainerRoleProbabilities> kvp in dicts)
        {
            if (!kvp.Value.IsCertain)
            {
                kvp.Value.RecalculateProbability(counts);
            }
        }

        return dicts;
    }

    private static int CountRolesOfType(Game game, RoleTypes role)
    {
        return game.Roles.Count(r => r.RoleType == role);
    }
}