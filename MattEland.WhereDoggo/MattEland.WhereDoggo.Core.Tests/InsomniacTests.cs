using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests;

public class InsomniacTests : GameTestsBase
{
    [Test]
    public void InsomniacShouldKnowThemselvesWhenNotMoved()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new InsomniacRole(),
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
        finalProbabilities[player].Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        finalProbabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        finalProbabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void InsomniacShouldHaveAnInsomniacSawOwnCardEvent()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new InsomniacRole(),
            new WerewolfRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole()
        };

        // Act
        OneNightWhereDoggoGame game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        GameEventBase? e = player.Events.FirstOrDefault(e => e.GetType() == typeof(InsomniacSawOwnCardEvent));

        e.ShouldNotBeNull();
    }
}