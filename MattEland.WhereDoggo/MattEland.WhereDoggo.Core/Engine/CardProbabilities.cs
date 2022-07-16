namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents the probabilities one player places on a <see cref="CardContainer"/> being each role that is in play
/// This allows us to track the perceived probability that a card (player or center card) is each role.
/// This information is then used for deductive reasoning by AI agents.
/// </summary>
public class CardProbabilities
{
    /// <summary>
    /// Instantiates an instance of <see cref="CardProbabilities"/>.
    /// </summary>
    /// <param name="game">The game</param>
    /// <param name="isCenterCard">Whether or not this slot represents a center card</param>
    public CardProbabilities(Game game, bool isCenterCard)
    {
        IsCenterCard = isCenterCard;
        Dictionary<RoleTypes, int> roleCounts = game.BuildRoleCounts();

        RecalculateProbability(roleCounts);
        PossibleRoles = roleCounts.Keys;
    }

    /// <summary>
    /// Instantiates an instance of <see cref="CardProbabilities"/>.
    /// </summary>
    /// <param name="isCenterCard">Whether or not this slot represents a center card</param>
    /// <param name="roleCounts">The count of various roles and their occurrences in play</param>
    public CardProbabilities(IDictionary<RoleTypes, int> roleCounts, bool isCenterCard)
    {
        IsCenterCard = isCenterCard;
        RecalculateProbability(roleCounts);
        PossibleRoles = roleCounts.Keys;
    }

    /// <summary>
    /// Gets the probabilities of the card being each role.
    /// </summary>
    public IDictionary<RoleTypes, decimal> Probabilities { get; } = new Dictionary<RoleTypes, decimal>();

    /// <summary>
    /// Recalculates the probabilities of the card being each role.
    /// </summary>
    /// <param name="originalCounts">The count of roles in the game</param>
    public void RecalculateProbability(IDictionary<RoleTypes, int> originalCounts)
    {
        Probabilities.Clear();

        Dictionary<RoleTypes, int> roleCounts = new();
        foreach (KeyValuePair<RoleTypes, int> kvp in originalCounts)
        {
            roleCounts[kvp.Key] = kvp.Value;
        }

        int remainingRoles = roleCounts.Values.Sum();

        // If we have eliminated roles from consideration, do not consider them at all
        foreach (KeyValuePair<RoleTypes, int> kvp in roleCounts.Where(kvp => CannotBe.Contains(kvp.Key)))
        {
            remainingRoles -= kvp.Value;
            roleCounts[kvp.Key] -= kvp.Value;
        }

        // Now allocate probabilities based on the total of each card that is unaccounted for
        foreach (KeyValuePair<RoleTypes, int> kvp in roleCounts)
        {
            if (CannotBe.Contains(kvp.Key))
            {
                Probabilities[kvp.Key] = 0;
            }
            else if (remainingRoles <= 0) // Shouldn't happen, but guard against div / 0 exceptions
            {
                Probabilities[kvp.Key] = 0;
            }
            else
            {
                Probabilities[kvp.Key] = kvp.Value / (decimal)remainingRoles;
            }
        }

        // If we're certain of any card, mark it as certain
        if (Probabilities.Values.Any(p => p >= 1.0m))
        {
            IsCertain = true;
        }
    }

    /// <summary>
    /// Marks that we are certain that a container has a specific role in it.
    /// This will set its probability to 100%, other role probabilities to 0%, and mark IsCertain as true.
    /// </summary>
    /// <param name="role">The role we are certain of.</param>
    public void MarkAsCertainOfRole(RoleTypes role)
    {
        foreach (RoleTypes key in Probabilities.Keys)
        {
            if (key == role)
            {
                Probabilities[key] = 1;
                CannotBe.Remove(key);
            }
            else
            {
                Probabilities[key] = 0;
                CannotBe.Add(key);
            }
        }

        IsCertain = true;
    }

    /// <summary>
    /// Determines whether or not we are certain of a specific role probability
    /// </summary>
    public bool IsCertain { get; private set; }

    /// <summary>
    /// The probability of the card belonging to various teams
    /// </summary>
    public IDictionary<Teams, decimal> TeamProbabilities
    {
        get
        {
            Dictionary<Teams, decimal> teamProbabilities = new();

            foreach (Teams team in Enum.GetValues<Teams>())
            {
                teamProbabilities[team] = 0;
            }

            foreach (KeyValuePair<RoleTypes, decimal> kvp in Probabilities)
            {
                Teams teams = kvp.Key.DetermineTeam();

                teamProbabilities[teams] += kvp.Value;
            }

            return teamProbabilities;
        }
    }

    /// <summary>
    /// Gets the probable team the card belongs to
    /// </summary>
    public Teams ProbableTeam
    {
        get
        {
            IDictionary<Teams, decimal> probabilities = TeamProbabilities;

            decimal maxProbability = probabilities.Values.Max();

            return probabilities.FirstOrDefault(kvp => kvp.Value >= maxProbability).Key;
        }
    }

    /// <summary>
    /// Gets the probable role the card has
    /// </summary>
    public RoleTypes ProbableRole => Probabilities.MaxBy(kvp => kvp.Value).Key;

    /// <inheritdoc />
    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (KeyValuePair<RoleTypes, decimal> kvp in Probabilities.OrderByDescending(kvp => kvp.Value))
        {
            if (kvp.Value > 0)
            {
                sb.Append($"{kvp.Key.GetFriendlyName()}: {kvp.Value:P1} ");
            }
        }

        if (IsCertain)
        {
            sb.Append("CERTAIN ");
        }

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Marks that this card cannot be a specific role
    /// </summary>
    /// <param name="role">The role that this card cannot be</param>
    public void MarkAsCannotBeRole(RoleTypes role)
    {
        Probabilities[role] = 0;
        CannotBe.Add(role);
    }

    private HashSet<RoleTypes> CannotBe { get; } = new();

    /// <summary>
    /// Calculates the probability that the card is on a specific team
    /// </summary>
    /// <param name="teams">The team to check</param>
    /// <returns>The probability that the card is on that team</returns>
    public decimal CalculateTeamProbability(Teams teams) => TeamProbabilities[teams];

    /// <summary>
    /// Marks the specified role type as in play and not in the center set of cards.
    /// </summary>
    /// <param name="role">The role that must be in play</param>
    public void MarkRoleAsInPlay(RoleTypes role)
    {
        if (IsCenterCard)
        {
            MarkAsCannotBeRole(role);
        }
    }

    /// <summary>
    /// Gets whether or not the card represents a card in the center
    /// </summary>
    public bool IsCenterCard { get; }

    /// <summary>
    /// Gets whether or not the card represents a player card
    /// </summary>
    public bool IsPlayerCard => !IsCenterCard;

    /// <summary>
    /// Gets the unique roles in the game
    /// </summary>
    public IEnumerable<RoleTypes> PossibleRoles { get; }
}