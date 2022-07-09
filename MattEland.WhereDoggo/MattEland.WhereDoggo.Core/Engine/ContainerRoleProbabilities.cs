using MattEland.WhereDoggo.Core.Roles;

namespace MattEland.WhereDoggo.Core.Engine;

public class ContainerRoleProbabilities
{
    public ContainerRoleProbabilities(OneNightWhereDoggoGame game)
    {
        int numRoles = game.Entities.Count;

        Dictionary<RoleTypes, int> roleCounts = game.BuildRoleCounts();

        RecalculateProbability(numRoles, roleCounts);
    }

    public IDictionary<RoleTypes, decimal> Probabilities { get; } = new Dictionary<RoleTypes, decimal>();

    public void RecalculateProbability(int numRoles, IDictionary<RoleTypes, int> roleCounts)
    {
        Probabilities.Clear();

        foreach (KeyValuePair<RoleTypes, int> kvp in roleCounts)
        {
            if (CannotBe.Contains(kvp.Key))
            {
                roleCounts[kvp.Key] -= kvp.Value;
            }
        }

        foreach (KeyValuePair<RoleTypes, int> kvp in roleCounts)
        {
            if (CannotBe.Contains(kvp.Key))
            {
                Probabilities[kvp.Key] = 0;
            }
            else
            {
                Probabilities[kvp.Key] = kvp.Value / (decimal)numRoles;
            }
        }

        if (Probabilities.Values.Any(p => p >= 1.0m))
        {
            IsCertain = true;
        }
    }

    public void MarkAsCertainOfRole(RoleTypes role)
    {
        foreach (KeyValuePair<RoleTypes, decimal> kvp in Probabilities)
        {
            if (kvp.Key == role)
            {
                Probabilities[kvp.Key] = 1;
            }
            else
            {
                Probabilities[kvp.Key] = 0;
            }
        }

        IsCertain = true;
    }

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

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (KeyValuePair<RoleTypes, decimal> kvp in Probabilities.OrderByDescending(kvp => kvp.Value))
        {
            if (kvp.Value > 0)
            {
                sb.Append($"{kvp.Key}: {kvp.Value:P1} ");
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