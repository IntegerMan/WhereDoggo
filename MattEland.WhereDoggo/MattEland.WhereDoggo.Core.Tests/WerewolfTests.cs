namespace MattEland.WhereDoggo.Core.Tests;

public class WerewolfTests : GameTestsBase
{

    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        }; 
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void LoneWolfWhoSeesAWolfWithOnlyVillagersShouldKnowAllRoles()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.LoneWolfCenterCardStrategy = new SelectSpecificSlotPlacementStrategy(0);
        game.Start();
        game.PerformNightPhase();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        foreach (KeyValuePair<RoleContainerBase, ContainerRoleProbabilities> kvp in probabilities)
        {
            kvp.Value.IsCertain.ShouldBeTrue($"Was not certain of role {kvp.Value}");
        }
    }

    [Test]
    public void WerewolvesShouldKnowOthersAreVillagers()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.LoneWolfCenterCardStrategy = new SelectSpecificSlotPlacementStrategy(1);
        game.Start();
        game.PerformNightPhase();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        GamePlayer player2 = game.Players[1];
        probabilities[player2].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player2].Probabilities[RoleTypes.Werewolf].ShouldBe(0);

        GamePlayer player3 = game.Players[2];
        probabilities[player3].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player3].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
    }

}