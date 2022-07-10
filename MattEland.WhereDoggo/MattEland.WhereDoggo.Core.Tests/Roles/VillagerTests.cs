namespace MattEland.WhereDoggo.Core.Tests.Roles;

[Category("Roles")]
public class VillagerTests : GameTestsBase
{
    [Test]
    public void VillagersShouldThinkTheyAreVillagers()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Villager,
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
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
    }

    [Test]
    public void VillagersShouldHaveMixedProbabilitiesOnOtherPlayers()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Villager,
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
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        // 2 Doggos, 3 Rabbits in 5 other players
        GamePlayer secondPlayer = game.Players[1];
        probabilities[secondPlayer].Probabilities[RoleTypes.Villager].ShouldBe(3.0M / 5.0M);
        probabilities[secondPlayer].Probabilities[RoleTypes.Werewolf].ShouldBe(2.0M / 5.0M);
    }
}