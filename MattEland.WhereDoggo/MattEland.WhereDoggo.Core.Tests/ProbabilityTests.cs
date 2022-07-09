namespace MattEland.WhereDoggo.Core.Tests;

public class ProbabilityTests
{
    [Test]
    public void ProbabilityShouldBeSplitBetweenAllRoles()
    {
        // Arrange
        ContainerRoleProbabilities probabilities = new(6);
        Dictionary<RoleTypes, int> roleCounts = new()
        {
            [RoleTypes.Werewolf] = 2,
            [RoleTypes.Villager] = 4
        };

        // Act
        probabilities.RecalculateProbability(roleCounts);

        // Assert
        probabilities.Probabilities[RoleTypes.Werewolf].ShouldBe(2 / 6m);
        probabilities.Probabilities[RoleTypes.Villager].ShouldBe(4 / 6m);
    }
}