using MattEland.WhereDoggo.Core.OneNight;

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

            dicts[role] = probabilities;
        }

        return dicts;
    }
}