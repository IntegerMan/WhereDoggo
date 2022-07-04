namespace MattEland.WhereDoggo.Core.Engine;

public class ContainerRoleProbabilities
{
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
    }

    public override string ToString() => $"Rabbit: {ProbabilityRabbit:P}, Doggo: {ProbabilityDoggo:P}";
}