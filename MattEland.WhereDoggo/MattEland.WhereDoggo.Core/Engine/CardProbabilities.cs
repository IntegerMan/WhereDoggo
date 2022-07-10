namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents the probabilities one player places on a <see cref="RoleContainerBase"/> being each role that is in play
/// This allows us to track the perceived probability that a card (player or center card) is each role.
/// This information is then used for deductive reasoning by AI agents.
/// </summary>
public class CardProbabilities
{
    public CardProbabilities(Game game)
    {
        Dictionary<RoleTypes, int> roleCounts = game.BuildRoleCounts();

        RecalculateProbability(roleCounts);
    }

    public CardProbabilities(IDictionary<RoleTypes, int> roleCounts)
    {
        RecalculateProbability(roleCounts);
    }

    public IDictionary<RoleTypes, decimal> Probabilities { get; } = new Dictionary<RoleTypes, decimal>();

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
        foreach (KeyValuePair<RoleTypes, int> kvp in roleCounts)
        {
            if (CannotBe.Contains(kvp.Key))
            {
                remainingRoles -= kvp.Value;
                roleCounts[kvp.Key] -= kvp.Value;
            }
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
        foreach (KeyValuePair<RoleTypes, decimal> kvp in Probabilities)
        {
            if (kvp.Key == role)
            {
                Probabilities[kvp.Key] = 1;
                CannotBe.Remove(kvp.Key);
            }
            else
            {
                Probabilities[kvp.Key] = 0;
                CannotBe.Add(kvp.Key);
            }
        }

        IsCertain = true;
    }

    /// <summary>
    /// Determines whether or not we are certain of a specific role probability
    /// </summary>
    public bool IsCertain { get; private set; }

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

    public Teams ProbableTeams
    {
        get
        {
            IDictionary<Teams,decimal> probabilities = TeamProbabilities;
            
            decimal maxProbability = probabilities.Values.Max();

            return probabilities.FirstOrDefault(kvp => kvp.Value >= maxProbability).Key;
        }
    }

    public RoleTypes LikelyRole => Probabilities.MaxBy(kvp => kvp.Value).Key;

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

    public void MarkAsCannotBeRole(RoleTypes role)
    {
        Probabilities[role] = 0;
        CannotBe.Add(role);
    }

    private HashSet<RoleTypes> CannotBe { get; } = new();

    public decimal CalculateTeamProbability(Teams teams) => TeamProbabilities[teams];
}