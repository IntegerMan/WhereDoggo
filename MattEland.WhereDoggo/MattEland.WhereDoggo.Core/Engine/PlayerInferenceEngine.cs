namespace MattEland.WhereDoggo.Core.Engine;

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
    public IDictionary<IHasCard, CardProbabilities> BuildInitialRoleProbabilities() 
        => BuildFinalRoleProbabilities();

    /// <summary>
    /// Generates a set of probabilities around card ending positions
    /// </summary>
    /// <returns>
    /// The set of probabilities around the final role a card was.
    /// </returns>
    public IDictionary<IHasCard, CardProbabilities> BuildFinalRoleProbabilities()
    {
        Dictionary<IHasCard, CardProbabilities> cardProbabilities = new();
        Dictionary<RoleTypes, int> counts = _game.BuildRoleCounts();

        // Initial pass
        BuildInitialProbabilitiesAndUncertainCardCounts(cardProbabilities, counts);

        // Firm up uncertain probabilities
        FinalizeUncertainProbabilities(counts, cardProbabilities.Values);

        return cardProbabilities;
    }

    private void BuildInitialProbabilitiesAndUncertainCardCounts(
        IDictionary<IHasCard, CardProbabilities> cardProbabilities,
        IDictionary<RoleTypes, int> counts)
    {
        foreach (IHasCard holder in _game.Entities)
        {
            cardProbabilities[holder] = new CardProbabilities(_game, holder is CenterCardSlot);

            foreach (GameEventBase observedEvent in _player.Events)
            {
                observedEvent.UpdatePlayerPerceptions(_player, holder, cardProbabilities[holder]);
            }

            if (cardProbabilities[holder].IsCertain)
            {
                counts[cardProbabilities[holder].ProbableRole] -= 1;
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

    /// <summary>
    /// Generates a recommendation for a non-werewolf role that is likely to be in the center somewhere 
    /// </summary>
    /// <returns>The role recommendation or null</returns>
    public RoleTypes? DetermineBestCenterRoleClaim()
    {
        IDictionary<IHasCard, CardProbabilities> probabilities = BuildFinalRoleProbabilities();
        
        IEnumerable<CenterCardSlot> centerSlots = probabilities.Keys.OfType<CenterCardSlot>().OrderBy(s => _game.Randomizer.Next());

        Dictionary<RoleTypes, decimal> claimOptions = new();
        foreach (CenterCardSlot centerSlot in centerSlots)
        {
            CardProbabilities cardProbabilities = probabilities[centerSlot];

            // If we are certain of the card, we can claim it
            if (cardProbabilities.IsCertain && 
                cardProbabilities.ProbableTeam == Teams.Villagers && 
                !HasSideEffects(cardProbabilities.ProbableRole))
            {
                return cardProbabilities.ProbableRole;
            }

            // We don't have certainty, so just factor its probabilities in and we'll guess later
            foreach ((RoleTypes key, decimal value) in cardProbabilities.Probabilities)
            {
                // Do not attempt to claim unsafe roles
                if (key.DetermineTeam() != Teams.Villagers) continue;
                
                // Add or increment the probability of a role being present
                if (claimOptions.ContainsKey(key))
                {
                    claimOptions[key] += value;
                }
                else
                {
                    claimOptions[key] = value;
                }
            }
        }
        
        KeyValuePair<RoleTypes, decimal>? best;
        
        // Find the best option without side effects
        best = claimOptions.Where(r => !HasSideEffects(r.Key))
            .MaxBy(kvp => kvp.Value);

        // If we found something safe, use it. Otherwise, use the best unsafe option
        return best != null 
            ? best?.Key 
            : claimOptions.MaxBy(kvp => kvp.Value).Key;
    }

    /// <summary>
    /// Determines whether the specified role has undeniable side effects
    /// that other players can witness. Claiming to be a role with side effects
    /// is a lot more risky because the side effects will not be present.
    /// </summary>
    /// <remarks>
    /// Roles like Robber and Troublemaker have side effects and these effects may
    /// be witnessed by other players in rare circumstances, but these roles are
    /// not considered to have obvious side effects since you need to have the right
    /// role combinations for these to occur.
    /// </remarks>
    /// <param name="role">The role to evaluate</param>
    /// <returns>Whether or not the role has obvious side effects that can be observed by players</returns>
    private static bool HasSideEffects(RoleTypes role) =>
        role switch
        {
            RoleTypes.Mason => true,
            RoleTypes.Thing => true,
            RoleTypes.Exposer => true,
            RoleTypes.Sentinel => true,
            RoleTypes.Revealer => true,
            _ => false
        };
}