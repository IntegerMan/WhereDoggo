using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests;

/// <summary>
/// Tests related to the Sentinel Role
/// </summary>
public class SentinelTests : GameTestsBase
{
    [Test]
    public void SentinelsShouldKnowTheyAreSentinels()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new SentinelRole(),
            new WerewolfRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole()
        };
        OneNightWhereDoggoGame game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> finalProbabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        finalProbabilities[player].Probabilities[RoleTypes.Sentinel].ShouldBe(1);
        finalProbabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        finalProbabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void SentinelThatPlacesTokenShouldResultInCardWithToken()
    {
        Assert.Inconclusive("Not Implemented yet");
    }

    [Test]
    public void SentinelThatPlacesTokenShouldHaveAppropriateEvent()
    {
        Assert.Inconclusive("Not Implemented yet");
    }
    
    [Test]
    public void SentinelMayChooseToNotPlaceTokenShouldHaveAppropriateEvent()
    {
        Assert.Inconclusive("Not Implemented yet");
    }
}