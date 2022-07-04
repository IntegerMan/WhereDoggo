namespace MattEland.WhereDoggo.Core.Engine;

public class ContainerRoleProbabilities
{
    public ContainerRoleProbabilities(OneNightWhereDoggoGame game)
    {
        int numRoles = game.Entities.Count;
        int numRabbits = CountRolesOfType(game, RoleTypes.Rabbit);
        int numDoggos = CountRolesOfType(game, RoleTypes.Doggo);

        RecalculateProbability(numRoles, numRabbits, numDoggos);
    }

    public void RecalculateProbability(int numRoles, int numRabbits, int numDoggos)
    {
        ProbabilityRabbit = numRabbits / (decimal)numRoles;
        ProbabilityDoggo = numDoggos / (decimal)numRoles;
    }

    public decimal ProbabilityDoggo { get; set; }
    public decimal ProbabilityRabbit { get; set; }

    public void MarkAsCertainOfRole(GameRoleBase role)
    {
        if (role.IsDoggo)
        {
            ProbabilityRabbit = 0;
            ProbabilityDoggo = 1;
        }
        else
        {
            ProbabilityRabbit = 1;
            ProbabilityDoggo = 0;
        }

        IsCertain = true;
    }

    public bool IsCertain { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();

        if (ProbabilityDoggo > 0)
        {
            sb.Append($"Doggo: {ProbabilityDoggo:P1} ");
        }

        if (ProbabilityRabbit > 0)
        {
            sb.Append($"Rabbit: {ProbabilityRabbit:P1} ");
        }

        if (IsCertain)
        {
            sb.Append("CERTAIN ");
        }

        return sb.ToString().Trim();
    }

    private static int CountRolesOfType(OneNightWhereDoggoGame game, RoleTypes role)
    {
        return game.Roles.Count(r => r.Role == role);
    }

    public void MarkAsCannotBeRole(RoleTypes role)
    {
        if (role == RoleTypes.Rabbit)
        {
            ProbabilityRabbit = 0;
            ProbabilityDoggo = 1;
        }
        else
        {
            ProbabilityRabbit = 1;
            ProbabilityDoggo = 0;
        }

        IsCertain = true;
    }
}