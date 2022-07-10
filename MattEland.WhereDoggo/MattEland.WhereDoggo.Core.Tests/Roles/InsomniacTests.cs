using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

[Category("Roles")]
public class InsomniacTests : GameTestsBase
{
    [Test]
    public void InsomniacShouldKnowThemselvesWhenNotMoved()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> finalProbabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        finalProbabilities[player].Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        finalProbabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        finalProbabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void InsomniacShouldHaveAnInsomniacSawOwnCardEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        GameEventBase? e = player.Events.FirstOrDefault(e => e.GetType() == typeof(InsomniacSawOwnCardEvent));

        e.ShouldNotBeNull();
    }
}