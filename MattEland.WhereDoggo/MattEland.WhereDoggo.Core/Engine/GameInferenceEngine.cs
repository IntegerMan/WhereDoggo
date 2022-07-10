
namespace MattEland.WhereDoggo.Core.Engine;

public class GameInferenceEngine
{
    private readonly GamePlayer _player;
    private readonly Game _game;

    public GameInferenceEngine(GamePlayer player, Game game)
    {
        _player = player;
        _game = game;

        BuildFinalRoleProbabilities();
    }

    public IDictionary<RoleContainerBase, CardProbabilities> BuildInitialRoleProbabilities() 
        => BuildFinalRoleProbabilities();

    public IDictionary<RoleContainerBase, CardProbabilities> BuildFinalRoleProbabilities()
    {
        Dictionary<RoleContainerBase, CardProbabilities> cardProbabilities = new();

        Dictionary<RoleTypes, int> counts = _game.BuildRoleCounts();

        // Initial pass
        foreach (RoleContainerBase role in _game.Entities)
        {
            CardProbabilities probabilities = new(_game);

            foreach (GameEventBase @event in _player.Events)
            {
                @event.UpdatePlayerPerceptions(_player, role, probabilities);
            }

            if (probabilities.IsCertain)
            {
                counts[probabilities.LikelyRole] -= 1;
            }

            cardProbabilities[role] = probabilities;
        }


        // Firm up uncertain probabilities
        foreach (KeyValuePair<RoleContainerBase, CardProbabilities> kvp in cardProbabilities)
        {
            if (!kvp.Value.IsCertain)
            {
                kvp.Value.RecalculateProbability(counts);
            }
        }

        return cardProbabilities;
    }
}