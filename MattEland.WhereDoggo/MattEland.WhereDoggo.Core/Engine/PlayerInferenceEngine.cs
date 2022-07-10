﻿namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// The inference engine governs decision-making for an individual player making decisions about the game.
/// </summary>
public class PlayerInferenceEngine
{
    private readonly GamePlayer _player;
    private readonly Game _game;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerInferenceEngine"/> class.
    /// </summary>
    /// <param name="player">The player doing the inferring</param>
    /// <param name="game">The game world.</param>
    public PlayerInferenceEngine(GamePlayer player, Game game)
    {
        _player = player;
        _game = game;

        BuildFinalRoleProbabilities();
    }

    /// <summary>
    /// Generates a set of probabilities around card starting positions
    /// </summary>
    /// <remarks>
    /// This is currently identical to the final role probabilities since roles that move cards are not yet supported.
    /// </remarks>
    /// <returns>
    /// The set of probabilities around the initial role a card was.
    /// </returns>
    public IDictionary<RoleContainerBase, CardProbabilities> BuildInitialRoleProbabilities() 
        => BuildFinalRoleProbabilities();

    /// <summary>
    /// Generates a set of probabilities around card ending positions
    /// </summary>
    /// <returns>
    /// The set of probabilities around the final role a card was.
    /// </returns>
    public IDictionary<RoleContainerBase, CardProbabilities> BuildFinalRoleProbabilities()
    {
        Dictionary<RoleContainerBase, CardProbabilities> cardProbabilities = new();
        Dictionary<RoleTypes, int> counts = _game.BuildRoleCounts();

        // Initial pass
        BuildInitialProbabilitiesAndUncertainCardCounts(cardProbabilities, counts);

        // Firm up uncertain probabilities
        FinalizeUncertainProbabilities(counts, cardProbabilities.Values);

        return cardProbabilities;
    }

    private void BuildInitialProbabilitiesAndUncertainCardCounts(
        IDictionary<RoleContainerBase, CardProbabilities> cardProbabilities,
        IDictionary<RoleTypes, int> counts)
    {
        foreach (RoleContainerBase role in _game.Entities)
        {
            cardProbabilities[role] = new CardProbabilities(_game);

            foreach (GameEventBase observedEvent in _player.Events)
            {
                observedEvent.UpdatePlayerPerceptions(_player, role, cardProbabilities[role]);
            }

            if (cardProbabilities[role].IsCertain)
            {
                counts[cardProbabilities[role].LikelyRole] -= 1;
            }
        }
    }

    private static void FinalizeUncertainProbabilities(IDictionary<RoleTypes, int> counts,
        IEnumerable<CardProbabilities> probabilities)
    {
        foreach (CardProbabilities cardProbs in probabilities.Where(cp => !cp.IsCertain))
        {
            cardProbs.RecalculateProbability(counts);
        }
    }
}